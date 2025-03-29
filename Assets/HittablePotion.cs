using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class HittablePotion : MonoBehaviour
{
    private GameObject potionUI;    

    [SerializeField] private float moveSpeed;
    Vector2 targetPos;
    

    public void ChangePotionHealth(string attack)
    {
        PlayerHealth.instance.ChangePotionHealth(attack);
        PlayerMovement.movementLocked = false;
    }

    private void Awake()
    {
        potionUI = GameObject.Find("PotionImage");
        potionUI.SetActive(false);
        targetPos = new Vector2(PlayerHealth.instance.transform.position.x+1, PlayerHealth.instance.transform.position.y);
        PlayerMovement.FacePlayer(0);
        PlayerMovement.movementLocked = true;

    }

    private void Start()
    {
        StartCoroutine(MoveToPlayer());
    }
    private IEnumerator MoveToPlayer()
    {   
        float step = Time.deltaTime * moveSpeed;

        while(Vector3.Distance(transform.position, targetPos) > .001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
            yield return null;
        }
    }

    private void OnDisable()
    {
        potionUI.SetActive(true);
        PlayerMovement.movementLocked = false;
        
    }


}
