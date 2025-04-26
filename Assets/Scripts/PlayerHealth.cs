using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;

    [Header("Player Health")]
    private TMP_Text healthText;
    public string health = "20";

    [Header("Player Potion")]
    public bool hasPotion;
    public string potionHealth = "0";
    public TMP_Text potionHealthText;
    public GameObject potionSlot;

    [Header("References")]
    [SerializeField] private GameObject loseScreen; 


    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }   
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        healthText = GetComponentInChildren<TMP_Text>();
        healthText.text = health.ToString();

        //TESTING
        hasPotion = true;
        ChangePotionHealth("+5");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            if(hasPotion)
            {
                hasPotion = false;
                ChangeHealth("+" + instance.potionHealth);
                potionSlot.SetActive(false);
            }
        }
    } 
    public void ChangeHealth(string attack)
    {
        ExpressionTree tree = new ExpressionTree();
        tree.BuildFromInfix(health+attack);
        tree.InorderTraversal();
        health = tree.Evaluate().ToString();
        UpdateHealth();
    }

    public void ChangePotionHealth(string modification)
    {
        ExpressionTree tree = new ExpressionTree();
        tree.BuildFromInfix(potionHealth + modification);
        tree.InorderTraversal();
        potionHealth = tree.Evaluate().ToString();
        UpdatePotionHealth();
    }

    public void UpdatePotionHealth()
    {
        if(potionHealth == "0")
        {
            hasPotion = false;
            //change potion image
        }
        else
        {
            potionHealthText.text = potionHealth;
        }
    }

    public void GetPotion(string value)
    {
        hasPotion = true;
        instance.potionHealth = value;
        potionSlot.SetActive(true);
        UpdatePotionHealth();
    }

    public void UpdateHealth(){
        if(health == "0") {
            loseScreen.SetActive(true);
            Destroy(gameObject);
        } else {
            healthText.text = health;
        }
    }
}
