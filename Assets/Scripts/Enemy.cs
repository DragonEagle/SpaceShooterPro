using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    private Player _player;
    private Animator _anim;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _anim = GetComponent<Animator>();
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
            Destroy(gameObject, 2.37f);
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
            Destroy(gameObject, 2.37f);
        }
    }
}
