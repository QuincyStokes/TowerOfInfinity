
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Audio;
using System.Collections;

public class Enemy : BaseEnemy
{

    public GameObject projectilePrefab;
    //public PlayerHealth playerHealth;


    void Awake()
    {
        if(GameManager.instance.GetCurrentLevel() == 1)
        {
            health = ((int)Random.Range(1+GameManager.instance.numOfEnemy/1.5f, GameManager.instance.numOfEnemy*1.5f+4)).ToString();
        }
        else if(GameManager.instance.GetCurrentLevel() == 2)
        {
            int n = Random.Range(1, 100);

            if(n <25) // 25% chance to have positive health
            {
                health = ((int)Random.Range(4+GameManager.instance.numOfEnemy/1.5f, GameManager.instance.numOfEnemy*1.6f+7)).ToString();
            }
            else  // 75 % chance to have negative health
            {
                health = ((int)Random.Range(-GameManager.instance.numOfEnemy*1.5f-4, -GameManager.instance.numOfEnemy/1.5f-2)).ToString();
            }
        }
        else if(GameManager.instance.GetCurrentLevel() == 3)
        {
            int upper = (int)Random.Range(4+GameManager.instance.numOfEnemy*3, GameManager.instance.numOfEnemy*4+6);
            int lower = (int)Random.Range(2, GameManager.instance.numOfEnemy/2+2);
            int n = Random.Range(1, 100);
            if (n < 20)
            {
                health = ((int)(upper * 0.7)).ToString();
            }
            else if (GameManager.instance.numOfEnemy > 4)
            {
                while (upper % lower == 0 || lower % 7 == 0 || lower % 11 == 0 || lower % 13 == 0 || lower % 17 == 0 || lower % 19 == 0 || lower % 23 == 0 || lower % 29 == 0 || lower % 31 == 0 || lower % 37 == 0 || lower % 41 == 0 || lower % 43 == 0 || lower % 47 == 0)
                {
                    lower++;
                }
                health = upper.ToString() + "/" + lower.ToString();
            }
            else
            {
                health = upper.ToString() + "/" + lower.ToString();
            }
            if (n > 10 && n < 60)
            {
                health = "-" + health;
            }
        }
        else if(GameManager.instance.GetCurrentLevel() == 4)
        {
            int upper = (int)Random.Range(6+GameManager.instance.numOfEnemy*7, GameManager.instance.numOfEnemy*10+8);
            int lower = (int)Random.Range(2+GameManager.instance.numOfEnemy, GameManager.instance.numOfEnemy*3+6);
            int n = Random.Range(1, 100);
            if(n < 40)
            {
                health = (upper*3).ToString();
            }
            else
            {
                while(upper % lower ==0 || lower%11==0 || lower%13==0 || lower%17==0 || lower%19==0 || lower%23==0 || lower%29==0 || lower%31==0 || lower%37==0 || lower%41==0 || lower%43==0 || lower%47==0 || lower%53==0 || lower%59==0 || lower%61==0 || lower%67==0 || lower%71==0 || lower%73==0 || lower%79==0)
                {
                    lower++;
                }
                health = upper.ToString()+"/"+lower.ToString();
            }
            if(n>20 && n<70)
            {
                health = "-"+health;
            }
        }
    }


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

    void Attack(Vector2 playerPosition)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Vector2 velocity = new Vector2(playerPosition.x - transform.position.x, playerPosition.y - transform.position.y);

        //can set the text of the projectile
        //get player health to determine what operation to do
        
        StartCoroutine(AttackAnimationTimer());
        AudioManager.Instance.PlayOneShotVariedPitch(attackSFX, 1f, SFXamg, .1f);
        projectile.GetComponent<Projectile>().FireProjectile(velocity, this.gameObject);
    }

    private IEnumerator AttackAnimationTimer()
    {
        animator.SetInteger("State", 2);
        yield return new WaitForSeconds(.5f);
        animator.SetInteger("State", 0);
    }
    //hello

}
