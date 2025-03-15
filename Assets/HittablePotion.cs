using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class HittablePotion : MonoBehaviour
{
    private GameObject potionUI;    

    [SerializeField] private float moveSpeed;
    public void ChangePotionHealth(string attack)
    {
        PlayerHealth.instance.ChangePotionHealth(attack);
        PlayerMovement.movementLocked = false;
    }

    private void Awake()
    {
        potionUI = GameObject.Find("PotionImage");
        potionUI.SetActive(false);
        
    }

    private void OnDisable()
    {
        potionUI.SetActive(true);
        
    }


}
