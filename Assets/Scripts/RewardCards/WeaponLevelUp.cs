using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class WeaponLevelUp : BaseReward 
{
    /// <summary>
    /// 
    /// </summary>
    public int weaponToLevel;
    public int levelsToAdd;
    
    public override void Reward()
    {
        WeaponHandler.Instance.weapons[weaponToLevel].LevelUp(levelsToAdd);
        WeaponHandler.Instance.UpdateLevelUI();
        RewardManager.Instance.DisableRewardMenu();
    }

}
