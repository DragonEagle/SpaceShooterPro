﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _ammo = 15;
    [SerializeField]
    private int _maxAmmo = 15;
    [SerializeField]
    private float _maxThrusters = 5f;
    private int _score=0;
    private int _shields = 0;
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _boostedSpeed = 8.5f;
    [SerializeField]
    private float _reducedSpeed = 1.75f;
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
    private GameObject _scatterShotPrefab;
    [SerializeField]
    private float _scatterShotDuration = 5f;
    [SerializeField]
    private GameObject _homingLaserPrefab;
    [SerializeField]
    private float _homingLaserDuration = 5f;
    [SerializeField]
    private float _speedBoostDuration = 5f;
    [SerializeField]
    private GameObject _shieldVisual;

    [Header("Damage Indicators")]
    [SerializeField]
    private GameObject _rightEngineFire;
    [SerializeField]
    private GameObject _leftEngineFire;
    private float _thrustersRemaining;

    private bool _isSpeedBoost = false;
    private bool _isSpeedReduced = false;

    private bool _isTripleShot = false;
    private bool _isScatterShot = false;
    private bool _isHomingLaser = false;
    private Camera _mainCamera;
//    private bool _isShieldsActive = false;

    private AudioSource audioSource;

    public int Lives { get { return _lives; } }
    public int Score { get { return _score; } }
    public int Shields { get { return _shields; } }
    public int Ammo { get { return _ammo; } }
    public int MaxAmmo { get {return _maxAmmo; } }
    public float Thrusters { get { return _thrustersRemaining / _maxThrusters; } }

    // Start is called before the first frame update
    void Start()
    {
        //  Take the current position == Start position (0,0,0)
        transform.position = new Vector3(0,0,0);
        audioSource = GetComponent<AudioSource>();
        _mainCamera = Camera.main;
        if (!audioSource) {
            Debug.LogError("The player doesn't have an audiosource");
        } else
        {
            audioSource.clip = _laserSound;
        }
        _thrustersRemaining = _maxThrusters;
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
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            _thrustersRemaining += Time.deltaTime / 2;
        }
        if (_thrustersRemaining > _maxThrusters)
        {
            _thrustersRemaining = _maxThrusters;
        }
    }
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveVector = new Vector3(horizontalInput, verticalInput, 0);

        if (Input.GetKey(KeyCode.LeftShift) && _thrustersRemaining > 0)
        {
            moveVector *= _trusterMultiplier;
            _thrustersRemaining -= Time.deltaTime;
        }

        if (_isSpeedReduced)
        {
            moveVector *= _reducedSpeed;
        }
        else if (_isSpeedBoost)
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
        if (_isHomingLaser)
        {
            Instantiate(_homingLaserPrefab, transform.position + new Vector3(0f, 1.1f, 0f), Quaternion.identity);
        }
        else if (_isScatterShot)
        {
            Instantiate(_scatterShotPrefab, transform.position, Quaternion.identity);
        }
        else if (_isTripleShot)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
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
        StartCoroutine(ShakeCamera());
        audioSource.clip = _explosionSound;
        audioSource.Play();
        if (_lives == 0)
        {
            AudioSource.PlayClipAtPoint(_explosionSound, transform.position);
            Destroy(gameObject);
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
    public void ActivateScatterShot()
    {
        _isScatterShot = true;
        StartCoroutine(CooldownScatterShot());
    }
    IEnumerator CooldownScatterShot()
    {
        yield return new WaitForSeconds(_scatterShotDuration);
        _isScatterShot = false;
    }
    public void ActivateHomingLaser()
    {
        _isHomingLaser = true;
        StartCoroutine(CooldownHomingLaser());
    }
    IEnumerator CooldownHomingLaser()
    {
        yield return new WaitForSeconds(_homingLaserDuration);
        _isHomingLaser = false;
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
    public void ActivateReducedSpeed()
    {
        _isSpeedReduced = true;
        StartCoroutine(CooldownSpeedReduce());
    }
    IEnumerator CooldownSpeedReduce()
    {
        yield return new WaitForSeconds(_speedBoostDuration);
        _isSpeedReduced = false;
    }
    public void ActivateShields()
    {
        //        _isShieldsActive = true;
        _shields = 3;
        _shieldVisual.SetActive(true);
    }
    public void AddToAmmo(int amount)
    {
        _ammo += amount;
        if (_ammo > _maxAmmo)
        {
            _ammo = _maxAmmo;
        }
    }
    public void IncreaseHealth()
    {
        if (_lives < 3)
        {
            _lives++;
            if (Lives == 3)
            {
                _rightEngineFire.SetActive(false);
            }
            else if (_lives == 2)
            {
                _leftEngineFire.SetActive(false);
            }
        }
    }
    public void AddToScore(int amount)
    {
        _score += amount;
    }
    IEnumerator ShakeCamera()
    {
        Vector3 origPos = _mainCamera.transform.position;
        _mainCamera.transform.Translate(new Vector3(Random.Range(-0.1f,0.1f),Random.Range(-0.1f,0.1f),0));
        yield return new WaitForSeconds(0.05f);
        _mainCamera.transform.position = origPos;
        yield return new WaitForSeconds(0.05f);
        _mainCamera.transform.Translate(new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0));
        yield return new WaitForSeconds(0.05f);
        _mainCamera.transform.position = origPos;
        yield return new WaitForSeconds(0.05f);
        _mainCamera.transform.Translate(new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0));
        yield return new WaitForSeconds(0.05f);
        _mainCamera.transform.position = origPos;
        yield return new WaitForSeconds(0.05f);
        _mainCamera.transform.Translate(new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0));
        yield return new WaitForSeconds(0.05f);
        _mainCamera.transform.position = origPos;
        yield return new WaitForSeconds(0.05f);
        _mainCamera.transform.Translate(new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0));
        yield return new WaitForSeconds(0.05f);
        _mainCamera.transform.position = origPos;
        yield return new WaitForSeconds(0.05f);
        _mainCamera.transform.Translate(new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0));
        yield return new WaitForSeconds(0.05f);
        _mainCamera.transform.position = origPos;
    }
}
