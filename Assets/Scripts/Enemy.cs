using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private float _ramSpeed = 6f;
    [SerializeField]
    private float _ramDistance = 1f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _shieldVisual;
    [SerializeField]
    private int _chanceOfShield = 3;
    private Player _player;
    private Animator _anim;
    private AudioSource _audioSource;
    private bool _canFire = true;
    private bool _hasShield = false;

    private SpawnManager _spawnManager;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _spawnManager = FindObjectOfType<SpawnManager>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _hasShield = (Random.Range(0, 10) < _chanceOfShield);
        if (_hasShield)
        {
            _shieldVisual.SetActive(true);
        }

        StartCoroutine(RandomShoot());
    }

    // Update is called once per frame
    void Update()
    {
        if (_player)
        {
            if ((Vector3.Distance(transform.position, _player.transform.position) < _ramDistance) && (transform.position.y > _player.transform.position.y))
            {
                Vector3 moveVector = _player.transform.position - transform.position;
                moveVector = moveVector.normalized;
                transform.Translate(moveVector * _ramSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
            }
            if (transform.position.y < -5f)
            {
                float randomX = Random.Range(-9.5f, 9.5f);
                transform.position = new Vector3(randomX, 8f, 0f);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player)
            {
                player.Damage();
            }
            Dammage();
        }
        else if (other.transform.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player)
            {
                _player.AddToScore(10);
            }
            Dammage();
        }
    }
    private void Dammage()
    {
        if (_hasShield)
        {
            _hasShield = false;
            _shieldVisual.SetActive(false);
            return;
        }
        _speed = 0;
        if (_anim)
        {
            _anim.SetTrigger("OnEnemyDeath");
        }
        if (_audioSource)
        {
            _audioSource.Play();
        }
        _canFire = false;
        Destroy(GetComponent<Collider2D>());
        _spawnManager.DestroyEnemy(this);
        Destroy(gameObject, 2.5f);
    }
    IEnumerator RandomShoot()
    {
        while (true)
        {
            if (_canFire)
            {
                GameObject laser = Instantiate(_laserPrefab, transform.position + new Vector3(0, -1.4f, 0), Quaternion.identity);
                if (transform.position.y < _player.transform.position.y)
                {
                    EnemyLaser enemyLaser = laser.GetComponent<EnemyLaser>();
                    if (enemyLaser)
                    {
                        enemyLaser.SetReverse();
                    }
                }
            }
            float randomWait = Random.Range(3f,7f);
            yield return new WaitForSeconds(randomWait);
        }

    }
}
