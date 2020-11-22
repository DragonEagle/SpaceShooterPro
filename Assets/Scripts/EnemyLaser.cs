using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8;
    [SerializeField]
    private GameObject _explosionPrefab;
    private bool _reverse = false;

    // Update is called once per frame
    void Update()
    {
        if (_reverse)
        {
            HandleReverseMovement();
        } else
        {
            HandleMovement();
        }
    }
    void HandleReverseMovement()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        if (transform.position.y > 8)
        {
            Destroy(gameObject);
        }
    }
    void HandleMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -8)
        {
            Destroy(gameObject);
        }
    }
    public void SetReverse()
    {
        _reverse = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag =="Player")
        {
            Player player = other.GetComponent<Player>();
            if (player)
            {
                player.Damage();
            }
            Destroy(gameObject);
        }
        else if (other.tag == "Powerup")
        {
            GameObject explosion = Instantiate(_explosionPrefab, other.transform.position, Quaternion.identity);
            explosion.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
