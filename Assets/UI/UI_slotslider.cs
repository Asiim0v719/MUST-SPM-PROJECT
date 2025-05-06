using System;
using UnityEngine;
using UnityEngine.UI;

public class SliderColorChanger : MonoBehaviour
{
    private Slider slider; // Slider 组件
    private Image fillImage; // 填充区域的 Image 组件

    // 定义颜色
    private Color greenColor = Color.green;   // 0.8 以上
    private Color yellowColor = Color.yellow; // 0.4 到 0.8
    private Color redColor = Color.red;       // 0 到 0.4

    void Start()
    {
        // 获取 Slider 组件
        slider = GetComponent<Slider>();
        if (slider == null)
        {
            Debug.LogError("未找到 Slider 组件！");
            return;
        }

        // 获取填充区域的 Image 组件
        fillImage = slider.fillRect.GetComponent<Image>();
        if (fillImage == null)
        {
            Debug.LogError("未找到填充区域的 Image 组件！请确保 Slider 的 Fill Rect 已设置。");
            return;
        }

        // 确保 Slider 的值范围是 0 到 1
        slider.minValue = 0f;
        slider.maxValue = 1f;

        // 初始化颜色
        UpdateColor(slider.value);

        // 监听 Slider 值变化
        slider.onValueChanged.AddListener(UpdateColor);
    }

    private void Update()
    {
        // throw new NotImplementedException();
    }

    void UpdateColor(float value)
    {
        if (value >= 0.8f)
        {
            fillImage.color = greenColor; // 绿色
        }
        else if (value >= 0.4f)
        {
            fillImage.color = yellowColor; // 黄色
        }
        else
        {
            fillImage.color = redColor; // 红色
        }
    }
}