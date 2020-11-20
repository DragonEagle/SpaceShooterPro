using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _ammo = 15;
    private int _score=0;
    private int _shields = 0;
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _boostedSpeed = 8.5f;
    [SerializeField]
    private float _trusterMultiplier = 1.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private AudioClip _laserSound;
    [SerializeField]
    private AudioClip _explosionSound;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;

    [Header("Powerups")]
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _tripleShotDuration = 5f;
    [SerializeField]
    private float _speedBoostDuration = 5f;
    [SerializeField]
    private GameObject _shieldVisual;

    [Header("Damage Indicators")]
    [SerializeField]
    private GameObject _rightEngineFire;
    [SerializeField]
    private GameObject _leftEngineFire;

    private bool _isSpeedBoost = false;
    private bool _isTripleShot = false;
//    private bool _isShieldsActive = false;

    private AudioSource audioSource;

    public int Lives { get { return _lives; } }
    public int Score { get { return _score; } }
    public int Shields { get { return _shields; } }
    public int Ammo { get { return _ammo; } }

    // Start is called before the first frame update
    void Start()
    {
        //  Take the current position == Start position (0,0,0)
        transform.position = new Vector3(0,0,0);
        audioSource = GetComponent<AudioSource>();
        if (!audioSource) {
            Debug.LogError("The player doesn't have an audiosource");
        } else
        {
            audioSource.clip = _laserSound;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1"))
        {
            if ((Time.time > _canFire) && _ammo > 0)
            {
                ShootLaser();
            }
        }
    }
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveVector = new Vector3(horizontalInput, verticalInput, 0);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveVector *= _trusterMultiplier;
        }

        if (_isSpeedBoost)
        {
            moveVector *= _boostedSpeed;
        }
        else
        {
            moveVector *= _speed;
        }
        transform.Translate(moveVector * Time.deltaTime);

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
        if (transform.position.y < -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }
        else if (transform.position.y > 0f)
        {
            transform.position = new Vector3(transform.position.x, 0f, 0);
        }
    }
    void ShootLaser()
    {
        _canFire = Time.time + _fireRate;
        if (_isTripleShot)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        } else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0f, 1.1f, 0f), Quaternion.identity);
        }
        _ammo--;
        audioSource.clip = _laserSound;
        audioSource.Play();
    }
    public void Damage() 
    {
        if (_shields > 0)
        {
            _shields --;
            if(_shields == 0)
            {
                _shieldVisual.SetActive(false);
            }
            return;
        }
        _lives--;
        audioSource.clip = _explosionSound;
        audioSource.Play();
        if (_lives == 0)
        {
            Destroy(gameObject, 2f);
        } else if (_lives == 2)
        {
            _rightEngineFire.SetActive(true);
        } else if (_lives == 1)
        {
            _leftEngineFire.SetActive(true);
        }
    }
    public void ActivateTripleShot()
    {
        _isTripleShot = true;
        StartCoroutine(CooldownTripleShot());
    }
    IEnumerator CooldownTripleShot()
    {
        yield return new WaitForSeconds(_tripleShotDuration);
        _isTripleShot = false;
    }
    public void ActivateSpeedBoost()
    {
        _isSpeedBoost = true;
        StartCoroutine(CooldownSpeedBoost());
    }
    IEnumerator CooldownSpeedBoost()
    {
        yield return new WaitForSeconds(_speedBoostDuration);
        _isSpeedBoost = false;
    }
    public void ActivateShields()
    {
        //        _isShieldsActive = true;
        _shields = 3;
        _shieldVisual.SetActive(true);
    }
    public void AddToScore(int amount)
    {
        _score += amount;
    }
}
