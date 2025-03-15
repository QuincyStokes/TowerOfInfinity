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

    private void Awake()
    {
    }

    public override void Reward()
    {
        StartCoroutine(StartHittablePotion());        
    }

    private IEnumerator StartHittablePotion()
    {
        PlayerMovement.movementLocked = true;
        Vector2 targetPos = new Vector2(PlayerHealth.instance.transform.position.x+1, PlayerHealth.instance.transform.position.y);
        Time.timeScale = 1;
        yield return new WaitForSeconds(.1f);
        Instantiate(hittablePotion, targetPos, Quaternion.identity);
        RewardManager.Instance.DisableRewardMenu(); 

        
    }
    
}
