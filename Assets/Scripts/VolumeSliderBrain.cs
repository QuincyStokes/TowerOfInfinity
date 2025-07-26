using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderBrain : MonoBehaviour
{
    public Slider volumeSlider;
    private void Start()
    {
        volumeSlider.onValueChanged.AddListener(AudioManager.Instance.HandleVolumeSliderChanged);
    }
}
