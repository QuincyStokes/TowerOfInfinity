using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    PlayerAttack playerAttack;

    void Start() {
        playerAttack = GetComponentInParent<PlayerAttack>();
    }
    void OnTriggerEnter2D(Collider2D other) {    
        if(other.CompareTag("Enemy")) {
            //deal whatever amount of damage
            Enemy enemy = other.gameObject.GetComponentInParent<Enemy>();
            if(enemy != null)
            {
                enemy.ChangeHealth(playerAttack.performOperation());
                return;
            }
            Bosslev1 boss = other.gameObject.GetComponentInParent<Bosslev1>();
            if(boss != null)
            {
                boss.ChangeHealth(playerAttack.performOperation());
                return;
            }

        }
        
        //instead of just destroying, perform the corresponding sword attack damage
    }

}
