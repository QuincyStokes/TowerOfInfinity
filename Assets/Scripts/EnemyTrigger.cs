using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{  
    private BaseEnemy enemy;
    void Start()
    {
        enemy = gameObject.GetComponent<BaseEnemy>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            //enemy.UpdateHealth();
            enemy.enabled = true;
        }
    }
}
