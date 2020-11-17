using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemy;
    [SerializeField]
    private float _enemyDelay=5f;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private float _minPowerupDelay = 3f;
    [SerializeField]
    private float _maxPowerupDelay = 7f;
    [SerializeField]
    private GameObject _powerUp;
    [SerializeField]
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnPowerups());
    }

    IEnumerator SpawnEnemies()
    {
        while (player.Lives > 0)
        {
            float randomX = Random.Range(-9.5f, 9.5f);
            Vector3 position = new Vector3(randomX, 8f, 0f);
            GameObject enemy = Instantiate(_enemy, position, Quaternion.identity);
            enemy.transform.SetParent(_enemyContainer.transform);
            yield return new WaitForSeconds(_enemyDelay);
        }
    }
    IEnumerator SpawnPowerups()
    {
        while(player.Lives > 0) {
            float randomTime = Random.Range(_minPowerupDelay, _maxPowerupDelay);
            yield return new WaitForSeconds(randomTime);
            float randomX = Random.Range(-9.5f, 9.5f);
            Vector3 position = new Vector3(randomX, 8f, 0f);
            Instantiate(_powerUp, position, Quaternion.identity);
        }
    }
}
