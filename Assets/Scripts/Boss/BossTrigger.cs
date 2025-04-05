using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    private BaseEnemy boss;
    void Start()
    {
        boss = gameObject.GetComponent<BaseEnemy>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            //enemy.UpdateHealth();
            boss.enabled = true;
        }
    }
}
