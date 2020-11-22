using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    [SerializeField]
    private int _powerupID; // 0= TripleShot 1=Speed 2=Shield
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y < -5f)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player)
            {
                switch (_powerupID)
                {
                    case 0 :
                        player.ActivateTripleShot();
                        break;
                    case 1:
                        player.ActivateSpeedBoost();
                        break;
                    case 2:
                        player.ActivateShields();
                        break;
                    case 3:
                        player.AddToAmmo(15);
                        break;
                    case 4:
                        player.IncreaseHealth();
                        break;
                    case 5:
                        player.ActivateScatterShot();
                        break;
                    case 6: player.ActivateReducedSpeed();
                        break;
                    default:
                        break;

                }
            }
            _audioSource.Play();
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject,1f);
        }
    }
}
