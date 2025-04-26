using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreasePotion : BaseReward
{
    public int value;
    public override void Reward()
    {
        PlayerHealth.instance.ChangePotionHealth("+"+value);
        RewardManager.Instance.DisableRewardMenu(); 
    }
}
