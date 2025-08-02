using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderBrain : MonoBehaviour
{
    public Slider volumeSlider;
    private void Start()
    {
        volumeSlider.onValueChanged.AddListener(AudioManager.Instance.HandleVolumeSliderChanged);
    }

    private void Awake()
    {
        //print("Setting Volume Slider to mixerVolume" + AudioManager.Instance.mixerVolume);
        float volume;
        AudioManager.Instance.masterMixer.GetFloat("Volume", out volume);
        volumeSlider.value = volume;
        
    }
}
