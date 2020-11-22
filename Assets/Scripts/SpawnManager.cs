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
    private int _enemiesInWave = 10; // The number of enemies to defeat to get through the wave
    [SerializeField]
    private float _waveMultiplier = 1.5f; // How much to multiply the enemies by for tthe next wave
    [SerializeField]
    private GameObject[] _powerUps;
    [SerializeField]
    private GameObject[] _rarePowerUps;
    [SerializeField]
    private Player player;

    private int _waveID = 1;
    private List<Enemy> enemies = new List<Enemy>();
    private int _enemiesDefeated = 0;

    // Start is called before the first frame update
    void Start()
    {
    }
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnPowerups());
    }
    IEnumerator SpawnEnemies()
    {
        while (player.Lives > 0)
        {
            yield return new WaitForSeconds(3.0f);
            float randomX = Random.Range(-9.5f, 9.5f);
            Vector3 position = new Vector3(randomX, 8f, 0f);
            if (enemies.Count < _waveID)
            {
                GameObject enemy = Instantiate(_enemy, position, Quaternion.identity);
                enemies.Add(enemy.GetComponent<Enemy>());
                enemy.transform.SetParent(_enemyContainer.transform);
            }
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
            bool spawnRare = (Random.Range(0, 10) > 6);
            if (spawnRare)
            {
                Instantiate(_rarePowerUps[Random.Range(0, _rarePowerUps.Length)], position, Quaternion.identity);
            } else
            {
                Instantiate(_powerUps[Random.Range(0, _powerUps.Length)], position, Quaternion.identity);
            }
        }
    }
    public void DestroyEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
        _enemiesDefeated++;
        if (_enemiesDefeated == _enemiesInWave)
        {
            _waveID++;
            _enemiesInWave = (int)(_enemiesInWave * _waveMultiplier);
            _enemiesDefeated = 0;
        }
    }
}
