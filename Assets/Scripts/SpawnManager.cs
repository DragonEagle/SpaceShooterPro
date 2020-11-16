using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemy;
    [SerializeField]
    private float _delay=5f;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (player.Lives > 0)
        {
            float randomX = Random.Range(-9.5f, 9.5f);
            Vector3 position = new Vector3(randomX, 8f, 0f);
            GameObject enemy = Instantiate(_enemy, position, Quaternion.identity);
            enemy.transform.SetParent(_enemyContainer.transform);
            yield return new WaitForSeconds(_delay);
        }
    }
}
