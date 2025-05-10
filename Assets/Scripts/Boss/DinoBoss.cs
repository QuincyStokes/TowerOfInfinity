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

    public LayerMask projectileLayer;
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
    
    void Move(Vector2 playerPosition) {
       
        if(Vector2.Distance((Vector2)transform.position, playerPosition) < 8)
        {
            RaycastHit2D enemyHit;
            
            float xDif = transform.position.x - playerPosition.x;
            float yDif = transform.position.y - playerPosition.y;
            if(Mathf.Abs(xDif) > 3 || Mathf.Abs(yDif) > 3) // is away from player
            {
                enemyTurn = false;
                if(Mathf.Abs(xDif)> Mathf.Abs(yDif) )
                {
                    if(xDif>=0)
                    {
                        //this is left movement
                        Vector2 start = transform.position;
                        Vector2 end = new Vector2(transform.position.x-xDif, transform.position.y);
                        collider2d.enabled = false;
                        outerCollider2d.enabled = false;
                        enemyHit = Physics2D.Linecast(start, end, projectileLayer);
                        collider2d.enabled = true;
                        outerCollider2d.enabled = true;
                        if(enemyHit.transform == null)
                        {
                            thisObject.transform.localScale = new Vector2(-10f, 10f);
                            StartCoroutine(SmoothMovement(new Vector3(transform.position.x-1, transform.position.y, 0)));
                        }
                        else
                        {
                            Debug.Log(enemyHit.transform.gameObject.name);
                        }
                       
                    }
                    else{
                        //this is right movement
                        Vector2 start = transform.position;
                        Vector2 end = new Vector2(transform.position.x+xDif, transform.position.y);
                        collider2d.enabled = false;
                        outerCollider2d.enabled = false;
                        enemyHit = Physics2D.Linecast(start, end, projectileLayer);
                        collider2d.enabled = true;
                        outerCollider2d.enabled = true;
                        if(enemyHit.transform == null)
                        {
                            thisObject.transform.localScale = new Vector2(10f, 10f);
                            StartCoroutine(SmoothMovement(new Vector3(transform.position.x+1, transform.position.y, 0)));
                        }
                    }
                    
                }
                else {
                    if(yDif>=0)
                    {
                        //this is down movement
                        Vector2 start = transform.position;
                        Vector2 end = new Vector2(transform.position.x, transform.position.y-yDif);
                        collider2d.enabled = false;
                        outerCollider2d.enabled = false;
                        enemyHit = Physics2D.Linecast(start, end, projectileLayer);
                        collider2d.enabled = true;
                        outerCollider2d.enabled = true;
                        if(enemyHit.transform == null)
                        {
                            StartCoroutine(SmoothMovement(new Vector3(transform.position.x, transform.position.y-1, 0)));
                        }
                    }
                    else
                    {
                        //this is up movement
                        Vector2 start = transform.position;
                        Vector2 end = new Vector2(transform.position.x, transform.position.y+yDif);
                        collider2d.enabled = false;
                        outerCollider2d.enabled = false;
                        enemyHit = Physics2D.Linecast(start, end, projectileLayer);
                        collider2d.enabled = true;
                        outerCollider2d.enabled = true;
                        if(enemyHit.transform == null)
                        {
                            StartCoroutine(SmoothMovement(new Vector3(transform.position.x, transform.position.y+1, 0)));
                        }
                    }
                }
            } 
        }
       
    }

    protected IEnumerator SmoothMovement (Vector3 end)
    {
        
        //Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
        //Square magnitude is used instead of magnitude because it's computationally cheaper.
        //isPlayerMoving = true;
        rb2D.bodyType = RigidbodyType2D.Dynamic;
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        animator.SetInteger("State", 1);
        //While that distance is greater than a very small amount (Epsilon, almost zero):
        while(sqrRemainingDistance > float.Epsilon)
        {
            //Find a new position proportionally closer to the end, based on the moveTime
            Vector2 newPostion = Vector2.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);

            //Call MovePosition on attached Rigidbody2D and move it to the calculated position.
            rb2D.MovePosition (newPostion);

            //Recalculate the remaining distance after moving.
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            //Return and loop until sqrRemainingDistance is close enough to zero to end the function
            yield return null;
        }
        animator.SetInteger("State", 0);
        rb2D.bodyType = RigidbodyType2D.Static;
        //isPlayerMoving = false;
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
