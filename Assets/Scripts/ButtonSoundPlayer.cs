using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundPlayer : MonoBehaviour
{
    public AudioSource audioSource; // 引用你的AudioSource
    // [SerializeField]private Button[] buttons;
    private void Start()
    {
        Button[] buttons = GetComponentsInChildren<Button>(true);
        // 假设你的按钮是这个脚本所在GameObject上的子对象
        foreach (var button in buttons)
        {
            if (button != null)
            {
                // Debug.Log(button.name);
                button.onClick.AddListener(PlaySound);
            }
            else
            {
                Debug.LogError("No Button component found in childrens!");
            }
        }
 
    }

    private void PlaySound()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }
    }
}