using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update

    //things the enemy needs
    //health
    //functions that change the "format" it's displayed in, depending on what type of number it is  
    //text for health
    private TMP_Text healthText;
    private float health;


    void Start()
    {
        healthText = GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SubtractDamage(float damage)
    {
        health -= damage;
        UpdateHealthText();
    }


    void UpdateHealthText(){
        healthText.text = health.ToString();
    }
}