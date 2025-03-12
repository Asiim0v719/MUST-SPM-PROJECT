
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class UI_HP : MonoBehaviour
{
    [SerializeField] private EntityStats stats;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI hp;


    private void Start()
    {
        slider.maxValue = stats.maxHealth.GetValue();
        slider.value = stats.maxHealth.GetValue();
        hp.text = stats.maxHealth.GetValue() + "/" + stats.maxHealth.GetValue();
    }


    private void Update()
    {
        slider.value = stats.currentHealth;
        hp.text = stats.currentHealth + "/" + stats.maxHealth.GetValue();

    }


    
}