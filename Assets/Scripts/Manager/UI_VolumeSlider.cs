using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_VolumeSlider : MonoBehaviour
{
    public Slider slider;
    public string parameter;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float multiplier;

    public AudioMixer AudioMixer { get { return audioMixer; } }
    public float Multiplier { get { return multiplier; } }


    public void SliderValue(float _value)
    {
        if (_value <= 0)
            _value = 0.001f;

        var value = Mathf.Log10(_value);

        audioMixer.SetFloat(parameter, value * multiplier);
        PlayerPrefs.SetFloat(parameter, slider.value);

    }

    public void LoadSlider(float _value)
    {
        if (_value >= 0.001f)
            slider.value = _value;
    }
}

