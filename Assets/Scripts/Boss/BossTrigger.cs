using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    private BaseBoss boss;
    void Start()
    {
        boss = gameObject.GetComponent<BaseBoss>();
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
