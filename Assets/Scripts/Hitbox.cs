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
            DinoBoss boss = other.gameObject.GetComponentInParent<DinoBoss>();
            if(boss != null)
            {
                boss.ChangeHealth(playerAttack.performOperation());
                return;
            }
        }
        else if (other.CompareTag("HittablePotion"))
        {
            HittablePotion hittablePotion = other.GetComponent<HittablePotion>();
            if(hittablePotion != null)
            {
                hittablePotion.ChangePotionHealth(playerAttack.performOperation());
                Destroy(hittablePotion.gameObject);
                //slide the potion ui back
            }
        }
        
        //instead of just destroying, perform the corresponding sword attack damage
    }

}
