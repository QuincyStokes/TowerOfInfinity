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
        enemyTurn =  true;
        if(enemyTurn)
        {
            Slide();
        }
        if(enemyTurn)
        {
            Walk(playerPosition);
            //if distance to player == 1, attack
            // else if distance = =2, walk
            //else, slide
        }
        if(enemyTurn)
        {
            Attack(playerPosition);
        }
    }
    private void Walk(Vector2 playerPosition)
    {
        animator.SetTrigger("Walk");
        
    }
    private void Slide()
    {
        animator.SetTrigger("Slide");
        //slides in the direction of the player, stops when it collides with something
            //when it hits something he stops (will rely on OnTriggerEnter to stop)

        animator.SetTrigger("StopAnimation");
    }
    private void Attack(Vector2 playerPosition)
    {
        animator.SetTrigger("Attack");
        //he has an attack hitbox
            //change it towards the player
            //enable it, wait a tiny bit of time, then disable it
    }
}

