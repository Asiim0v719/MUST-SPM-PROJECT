using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Text volumeText; // 显示当前音量的Text组件
    public AudioMixer audioMixer; // 引用你的AudioMixer
    public string exposedParamName = "FXVolume"; // AudioMixer中暴露的参数名称
    public float volumeStep = 1f; // 每次调整的音量步长
    public float minVolume = -80f; // 最小音量，通常是-80dB（静音）
    public float maxVolume = 20f; // 最大音量，通常是20dB
    public Button rightButton;
    public Button leftButton;
    [SerializeField] private float currentVolumeDecibels; // 当前音量（以分贝为单位）

    void Start()
    {
        // 初始化音量，获取当前设置的音量值
        audioMixer.GetFloat(exposedParamName, out currentVolumeDecibels);
        UpdateVolumeText();
    }

    // 左箭头点击事件
    public void DecreaseVolume()
    {
        // 减少音量
        currentVolumeDecibels = Mathf.Max(currentVolumeDecibels - volumeStep, minVolume);
        UpdateAudioMixerVolume();
        UpdateVolumeText();
    }

    // 右箭头点击事件
    public void IncreaseVolume()
    {
        // 增加音量
        currentVolumeDecibels = Mathf.Min(currentVolumeDecibels + volumeStep, maxVolume);
        UpdateAudioMixerVolume();
        UpdateVolumeText();
    }

    // 更新音量显示
    void UpdateVolumeText()
    {
        // // 将分贝值转换为用户友好的百分比显示
        // float volumePercentage = Mathf.InverseLerp(minVolume, maxVolume, currentVolumeDecibels) * 100;
        // volumeText.text = Mathf.RoundToInt(volumePercentage).ToString();

        volumeText.text = ((currentVolumeDecibels - minVolume) / (maxVolume - minVolume) * 10).ToString();
        switch (volumeText.text)
        {
            case "0":
                leftButton.interactable = false;
                break;
            case "10":
                rightButton.interactable = false;
                break;
            default:
                leftButton.interactable = true;
                rightButton.interactable = true;
                break;
        }
    }

    // 更新AudioMixer的音量
    void UpdateAudioMixerVolume()
    {
        audioMixer.SetFloat(exposedParamName, currentVolumeDecibels);
    }
}