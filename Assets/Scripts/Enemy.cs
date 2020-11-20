using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private GameObject _laserPrefab;
    private Player _player;
    private Animator _anim;
    private AudioSource _audioSource;
    private bool _canFire = true;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        StartCoroutine(RandomShoot());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -5f) {
            float randomX = Random.Range(-9.5f, 9.5f);
            transform.position = new Vector3(randomX, 8f,0f);
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
            Destroy(gameObject, 2.5f);
        }
        else if (other.transform.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player)
            {
                _player.AddToScore(10);
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
            Destroy(gameObject, 2.5f);
        }
    }
    IEnumerator RandomShoot()
    {
        while (true)
        {
            if (_canFire)
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, -1.4f, 0), Quaternion.identity);
            }
            float randomWait = Random.Range(3f,7f);
            yield return new WaitForSeconds(randomWait);
        }

    }
}
