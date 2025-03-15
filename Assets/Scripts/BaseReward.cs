using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "NewBaseReward", menuName = "UpgradeReward/NewReward")]
public abstract class BaseReward : MonoBehaviour
{

    [Header("UI References")]
    public Image buttonIcon;
    public TMP_Text descriptionText;

    [Header("Properties")]
    [SerializeField] private Sprite icon;
    [SerializeField] private string description;
    public string Description 
    { 
        get => description; 
        set => description = value; 
    }

    Sprite Icon {
        get; set;
    }

    [Header("Audio")]
    public AudioClip confirmSFX;
    public AudioMixerGroup UIamg;

    public void OnRewardSelected()
    {
        AudioManager.Instance.PlayOneShot(confirmSFX, 1f, UIamg);
        Reward();
    }

    public abstract void Reward();
    public virtual void Start()
    {
        descriptionText.text = Description;
        buttonIcon.sprite = icon;
    }




///how to change enemy health based on hits
///for each enemy store this
///    [ (timeHit, type), (timeHit, type), ...] 
///Then from there, we have some important data:
///         # of hits it took to defeat a certain enemy 
///              (this would be the amount of tuples in our list)
//               We can compare this to the optimal # of hits
///         #average time between hits. 
//              A higher time may mean the player is slow at this particular type, and needs practice

//take all of these factors into consideration
//   create more enemeis with the type of math they are the worst at
//   For example, if the player's # of hits is consistently far above the average for multiplication type enemies, spawn more of them





















}
