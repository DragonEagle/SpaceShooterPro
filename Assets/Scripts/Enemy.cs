using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -5f) {
            float randomX = Random.Range(-9.5f, 9.5f);
            transform.position = new Vector3(randomX, 8f,0f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player)
            {
                player.Damage();
            }
            //Damage player
            Destroy(gameObject);
        }
        else if (other.transform.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
