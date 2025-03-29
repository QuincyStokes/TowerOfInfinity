using System.Collections;
using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine.Audio;

public class Penguin : BaseBoss
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
        }
        if(enemyTurn)
        {
            Attack(playerPosition);
        }
    }
    private void Walk(Vector2 playerPosition)
    {

    }
    private void Slide()
    {
        
    }
    private void Attack(Vector2 playerPosition)
    {
        
    }
}

