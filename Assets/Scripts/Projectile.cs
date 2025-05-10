using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Projectile : MonoBehaviour
{
    private TMP_Text damageText;
    private string damage = "-2";
    private Rigidbody2D rb;
    private GameObject thisParent;
    private PlayerHealth playerHealthObject;
   

    public void Start()
    {
        
        
    }
    void Awake()
    {
        playerHealthObject = GameObject.Find("Player").GetComponent<PlayerHealth>();
        damageText = GetComponent<TMP_Text>();

    }
    public void setDamage(string playerHealth)
    {
        //ex. playerHeatlh is a positve integer
        // damage = "-" + (int)(Math.Random()*2*level) + level

        //ex. playerHeatlh is a negative integer
        // damage = "+" + (int)(Math.Random()*2*level) + level

        //ex. playerHealth is a fraction
        // damage = "*" + (int)(Math.Random()*2) + level -1;    50% chance to get the right denominator;

        // "/" is implemented by a boss.
        if(playerHealth.Contains('/'))
        {
            damage = '*' + ((int)(UnityEngine.Random.Range(1, 4) * 2)-1).ToString() ; //currently doesn't take into account the level
        }
        else if(playerHealth[0] == '-')
        {
            damage = '+' + ((int)UnityEngine.Random.Range(1, 8)).ToString(); //currently doesn't take into account the level
        }
        else
        {
            damage = '-' + ((int)UnityEngine.Random.Range(1, 8)).ToString(); //currently doesn't take into account the level
        }
        UpdateDamage();

        
        
    }

    public void FireProjectile(Vector2 velocity, GameObject parent)
    {  
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = velocity;
    
        setDamage(playerHealthObject.health);
       
        thisParent = parent;
    }
    public void ChangeDamage(string attack)
    {
        ExpressionTree tree = new ExpressionTree();
        tree.BuildFromInfix(damage+attack);
        tree.InorderTraversal();
        damage = tree.Evaluate().ToString();
        UpdateDamage();
    }
    public void UpdateDamage(){
        if(damage == "0") {
            Destroy(gameObject);
        } else {
            if(damageText != null)
            {
                damageText.text = damage;
            }
            else
            {
                Debug.Log("Failed to update damage");
            }
           
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")) {
            other.gameObject.GetComponent<PlayerHealth>().ChangeHealth(damage);
            Destroy(gameObject);
        }
        else if(other.CompareTag("Hit Box"))
        {
            Debug.Log($"Hits Projectile");
        }
        else if(!other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
        
    }
}
