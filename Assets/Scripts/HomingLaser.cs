using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingLaser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8;

    Enemy target;

    // Start is called before the first frame update
    void Start()
    {
        Enemy[] targets = FindObjectsOfType<Enemy>();
        Vector3 pos = transform.position;
        float distance = Mathf.Infinity;

        foreach(Enemy enemy in targets)
        {
            float curDistance = (enemy.transform.position - pos).sqrMagnitude;
            if(curDistance < distance)
            {
                distance = curDistance;
                target = enemy;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            Vector3 moveVector = target.transform.position - transform.position;
            moveVector = moveVector.normalized;
            transform.Translate(moveVector * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(transform.up * _speed * Time.deltaTime);
        }
        if (transform.position.y > 8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }
    }
}
