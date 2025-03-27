using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;



public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance;
    public GameObject rewardMenu;
    public int count;

    public bool rewardIsUp = false;

    [Header("Tier 3 Rewards")]
    public int tier3Wieght;
    public List<GameObject> tier3Rewards;
    public List<GameObject> tier3PotionRewards;
    public List<GameObject> tier3AdditionRewards;
    public List<GameObject> tier3MultiplicationRewards;
    public List<GameObject> tier3DivisionRewards;
    



    [Header("Tier 2 Rewards")]
    public int tier2Wieght;
    public List<GameObject> tier2Rewards;
    public List<GameObject>  tier2PotionRewards;
    public List<GameObject> tier2AdditionRewards;
    public List<GameObject> tier2MultiplicationRewards;
    public List<GameObject> tier2DivisionRewards;
    public List<GameObject> tier2HasPotionRewards;






    [Header("Tier 1 Rewards")]
    public int tier1Wieght;
    public List<GameObject> tier1Rewards;
    public List<GameObject> tier1PotionRewards;
    public List<GameObject> tier1AdditionRewards;
    public List<GameObject> tier1MultiplicationRewards;
    public List<GameObject> tier1DivisionRewards;
    public List<GameObject> tier1HasPotionRewards;





    private List<GameObject> tier1RewardPool;
    private List<GameObject> tier2RewardPool;
    private List<GameObject> tier3RewardPool;

    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void EnemyKilled()
    {
        if(!rewardIsUp)
        {
            rewardIsUp = true;
            EnableRewardMenu();
           
        }
        else{
            count++;
        }
    }


    public void EnableRewardMenu()
    {
        foreach(Transform child in rewardMenu.transform)
        {
            Destroy(child.gameObject);
        }

        for(int i=0; i<3; i++)
        {
            GameObject curr= Instantiate(GenerateRewardCard());
            curr.transform.SetParent(rewardMenu.transform);
        }
        rewardMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void DisableRewardMenu()
    {
        rewardMenu.SetActive(false);
        Time.timeScale = 1f;
        rewardIsUp = false;
        if(count > 0)
        {
            count--;
            EnemyKilled();
        }
    }

    private GameObject GenerateRewardCard()
    {
        int random = UnityEngine.Random.Range(0, tier3Wieght+tier2Wieght+tier1Wieght);
        print(tier3Wieght+tier2Wieght+tier1Wieght);
        print(random);
        
        if(random< tier3Wieght)
        {
            //return tier3Rewards[Random.Range(0, tier3Rewards.Count())];
            //check conditions for pool
            tier3RewardPool = tier3Rewards;
            
            if(!PlayerHealth.instance.hasPotion)
            {
                tier3RewardPool = tier3RewardPool.Union<GameObject>(tier3PotionRewards).ToList<GameObject>();
            }
            if(GameManager.instance.GetCurrentLevel() >= 2)
            {
                tier3RewardPool = tier3RewardPool.Union<GameObject>(tier3AdditionRewards).ToList<GameObject>();
            }
            if(GameManager.instance.GetCurrentLevel() >= 3)
            {
                tier3RewardPool = tier3RewardPool.Union<GameObject>(tier3MultiplicationRewards).ToList<GameObject>();
            }
            if(GameManager.instance.GetCurrentLevel() >= 4)
            {
                tier3RewardPool = tier3RewardPool.Union<GameObject>(tier3DivisionRewards).ToList<GameObject>();
            }
            return tier3RewardPool[UnityEngine.Random.Range(0, tier3RewardPool.Count())];
        }
        else if(random< tier3Wieght +tier2Wieght)
        {
            tier2RewardPool = tier2Rewards;
            if(!PlayerHealth.instance.hasPotion)
            {
                tier2RewardPool = tier2RewardPool.Union<GameObject>(tier2PotionRewards).ToList<GameObject>();
            }
            else
            {
                tier2RewardPool = tier2RewardPool.Union<GameObject>(tier2HasPotionRewards).ToList<GameObject>();
            }
            if(GameManager.instance.GetCurrentLevel() >= 2)
            {
                tier2RewardPool = tier2RewardPool.Union<GameObject>(tier2AdditionRewards).ToList<GameObject>();
            }
            if(GameManager.instance.GetCurrentLevel() >= 3)
            {
                tier2RewardPool = tier2RewardPool.Union<GameObject>(tier2MultiplicationRewards).ToList<GameObject>();
            }
            if(GameManager.instance.GetCurrentLevel() >= 4)
            {
                tier2RewardPool = tier2RewardPool.Union<GameObject>(tier2DivisionRewards).ToList<GameObject>();
            }
            return tier2RewardPool[UnityEngine.Random.Range(0, tier2RewardPool.Count())];
        }
        else //tier 1
        {
            tier1RewardPool = tier1Rewards;
            if(!PlayerHealth.instance.hasPotion)
            {
                tier1RewardPool = tier1RewardPool.Union<GameObject>(tier1PotionRewards).ToList<GameObject>();
            }
            else
            {
                tier1RewardPool = tier1RewardPool.Union<GameObject>(tier1HasPotionRewards).ToList<GameObject>();
            }
            if(GameManager.instance.GetCurrentLevel() >= 2)
            {
                tier1RewardPool = tier1RewardPool.Union<GameObject>(tier1AdditionRewards).ToList<GameObject>();
            }
            if(GameManager.instance.GetCurrentLevel() >= 3)
            {
                tier1RewardPool = tier1RewardPool.Union<GameObject>(tier1MultiplicationRewards).ToList<GameObject>();
            }
            if(GameManager.instance.GetCurrentLevel() >= 4)
            {
                tier1RewardPool = tier1RewardPool.Union<GameObject>(tier1DivisionRewards).ToList<GameObject>();
            }            
            return tier1RewardPool[UnityEngine.Random.Range(0, tier1RewardPool.Count())];
        }
    }
    ///reward menu prefab
    ///when you instaniate it, it chooses the cards randomly
    /// counter of how many rewards there are currently
    /// when disabled, if count is not zero, do it again


}
