using System.Collections;
using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine.Audio;

public class Penguin : BaseEnemy
{
    new void Start()
    {
        health = "999999";
        base.Start();
    }
    // movement: walk        Attack: closley,        Slide: move until hits something. Bump into player, decrease health of both.
    protected override void TakeTurn(Vector2 playerPosition)
    {
        enemyTurn = true;

        float distance = Vector2.Distance(transform.position, playerPosition);

        if (mustAttack) //attack
        {
            HideExclamation();
            Attack(playerPosition);
        }
        else if (distance < 2) //ready an attack
        {
            ShowExclamation();
        }
        else if (distance < 3)
        {
            Walk(playerPosition);
        }
        else
        {
            Slide(playerPosition);
        }
    }
    private void Walk(Vector2 playerPosition)
    {
        animator.SetTrigger("Walk");
        Move(playerPosition);
        //! TODO 
    }
    private void Slide(Vector2 playerPosition)
    {
        animator.SetTrigger("Slide");
        animator.SetBool("IsSliding", true);
        //slides in the direction of the player, stops when it collides with something
        //when it hits something he stops (will rely on OnTriggerEnter to stop)

        //! TODO
        animator.SetTrigger("StopAnimation");
        animator.SetBool("IsSliding", false);

    }
    private void Attack(Vector2 playerPosition)
    {
        animator.SetTrigger("Attack");
        //he has an attack hitbox
        //change it towards the player
        //enable it, wait a tiny bit of time, then disable it

        //! TODO
    }
    

    public void Move(Vector2 playerPosition)
    {

        if (Vector2.Distance((Vector2)transform.position, playerPosition) < 8)
        {
            RaycastHit2D enemyHit;

            float xDif = transform.position.x - playerPosition.x;
            float yDif = transform.position.y - playerPosition.y;
            if (Mathf.Abs(xDif) > 2 || Mathf.Abs(yDif) > 2) // is away from player
            {
                enemyTurn = false;
                if (Mathf.Abs(xDif) > Mathf.Abs(yDif))
                {
                    if (xDif >= 0)
                    {
                        //this is left movement
                        Vector2 start = transform.position;
                        Vector2 end = new Vector2(transform.position.x - 1, transform.position.y);
                        collider2d.enabled = false;
                        outerCollider2d.enabled = false;
                        enemyHit = Physics2D.Linecast(start, end, projectileLayer);
                        collider2d.enabled = true;
                        outerCollider2d.enabled = true;
                        if (enemyHit.transform == null)
                        {
                            thisObject.transform.localScale = new Vector2(-10f, 10f);
                            StartCoroutine(SmoothMovement(new Vector3(transform.position.x - 1, transform.position.y, 0)));
                        }
                        else
                        {
                            Debug.Log(enemyHit.transform.gameObject.name);
                        }

                    }
                    else
                    {
                        //this is right movement
                        Vector2 start = transform.position;
                        Vector2 end = new Vector2(transform.position.x + 1, transform.position.y);
                        collider2d.enabled = false;
                        outerCollider2d.enabled = false;
                        enemyHit = Physics2D.Linecast(start, end, projectileLayer);
                        collider2d.enabled = true;
                        outerCollider2d.enabled = true;
                        if (enemyHit.transform == null)
                        {
                            thisObject.transform.localScale = new Vector2(10f, 10f);
                            StartCoroutine(SmoothMovement(new Vector3(transform.position.x + 1, transform.position.y, 0)));
                        }
                    }

                }
                else
                {
                    if (yDif >= 0)
                    {
                        //this is down movement
                        Vector2 start = transform.position;
                        Vector2 end = new Vector2(transform.position.x, transform.position.y - 1);
                        collider2d.enabled = false;
                        outerCollider2d.enabled = false;
                        enemyHit = Physics2D.Linecast(start, end, projectileLayer);
                        collider2d.enabled = true;
                        outerCollider2d.enabled = true;
                        if (enemyHit.transform == null)
                        {
                            StartCoroutine(SmoothMovement(new Vector3(transform.position.x, transform.position.y - 1, 0)));
                        }
                    }
                    else
                    {
                        //this is up movement
                        Vector2 start = transform.position;
                        Vector2 end = new Vector2(transform.position.x, transform.position.y + 1);
                        collider2d.enabled = false;
                        outerCollider2d.enabled = false;
                        enemyHit = Physics2D.Linecast(start, end, projectileLayer);
                        collider2d.enabled = true;
                        outerCollider2d.enabled = true;
                        if (enemyHit.transform == null)
                        {
                            StartCoroutine(SmoothMovement(new Vector3(transform.position.x, transform.position.y + 1, 0)));
                        }
                    }
                }
            }
        }

    }
}

