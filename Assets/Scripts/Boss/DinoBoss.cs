using System.Collections;
using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine.Audio;

public class DinoBoss : BaseEnemy
{
    // Start is called before the first frame update

    //things the enemy needs
    //health
    //functions that change the "format" it's displayed in, depending on what type of number it is  
    //text for health

    public GameObject projectilePrefab;
    //public PlayerHealth playerHealth;



    new void Start()
    {
        base.Start();
    }
    protected override void TakeTurn(Vector2 playerPosition)
    {
        enemyTurn =  true;
        
        if(enemyTurn && !mustAttack)
        {
            Move(playerPosition);
        }
        if(enemyTurn)
        {
            if(mustAttack)
            {
                HideExclamation();
                Attack(playerPosition);
                
            }
            else
            {
                // show exclamation mark
                ShowExclamation();
            }
        }
    }

    public override void UpdateHealth()
    {
        if(health == "0") {
            BossRoomHandler.Instance.OpenDoor();
            RewardManager.Instance.EnemyKilled();
            Destroy(gameObject);
            
        } else {
            healthText.text = health;
        }
        
    }
    


    void Attack(Vector2 playerPosition)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        GameObject projectile1 = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        GameObject projectile2 = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        GameObject projectile3 = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        GameObject projectile4 = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Vector2 velocity = new Vector2(playerPosition.x - transform.position.x, playerPosition.y - transform.position.y);

        //can set the text of the projectile
        //get player health to determine what operation to do
        
        StartCoroutine(AttackAnimationTimer());
        AudioManager.Instance.PlayOneShotVariedPitch(attackSFX, 1f, SFXamg, .1f);
        projectile.GetComponent<Projectile>().FireProjectile(velocity, this.gameObject);
        velocity = new Vector2(playerPosition.x - transform.position.x-0.4f, playerPosition.y - transform.position.y-0.4f);
        
        projectile1.GetComponent<Projectile>().FireProjectile(velocity, this.gameObject);
        
        velocity = new Vector2(playerPosition.x - transform.position.x+0.4f, playerPosition.y - transform.position.y-0.4f);
        
        projectile2.GetComponent<Projectile>().FireProjectile(velocity, this.gameObject);

        velocity = new Vector2(playerPosition.x - transform.position.x-0.8f, playerPosition.y - transform.position.y-0.8f);
        
        projectile3.GetComponent<Projectile>().FireProjectile(velocity, this.gameObject);
        
        
        velocity = new Vector2(playerPosition.x - transform.position.x+0.8f, playerPosition.y - transform.position.y-0.8f);
        
        projectile4.GetComponent<Projectile>().FireProjectile(velocity, this.gameObject);
    }

    private IEnumerator AttackAnimationTimer()
    {
        animator.SetInteger("State", 2);
        yield return new WaitForSeconds(.5f);
        animator.SetInteger("State", 0);
    }

}
