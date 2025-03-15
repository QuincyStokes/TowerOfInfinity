using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPotionReward : BaseReward
{
    public string potionValue;

    public override void Start()
    {
        base.Start();
    }

    public override void Reward()
    {
       
        PlayerHealth.instance.GetPotion(potionValue);
        RewardManager.Instance.DisableRewardMenu();
    }
}
