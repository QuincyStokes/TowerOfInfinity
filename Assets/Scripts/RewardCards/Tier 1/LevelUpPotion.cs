using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelUpPotion : BaseReward
{
    [SerializeField] private GameObject hittablePotion;
    [SerializeField] private float potionMoveTime;
    [SerializeField] private Vector2 startingPos;

    private void Awake()
    {
    }

    public override void Reward()
    {
        StartCoroutine(StartHittablePotion());        
    }

    private IEnumerator StartHittablePotion()
    {
        startingPos = Camera.main.ScreenToWorldPoint(new Vector2(60, 53));
        Debug.Log($"Starting Pos set to {startingPos}");
        Time.timeScale = 1;
        
        yield return new WaitForSeconds(.1f);
        Instantiate(hittablePotion, startingPos, Quaternion.identity);
        
        RewardManager.Instance.DisableRewardMenu(); 
    }
    
}
