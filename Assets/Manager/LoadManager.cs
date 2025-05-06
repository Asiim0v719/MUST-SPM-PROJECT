using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class LoadManager : MonoBehaviour
{
    public GameObject loadScreen;
    public Slider slider;
    public TextMeshProUGUI text;
    [SerializeField] private GameObject savePanel;
    [SerializeField] private GameObject worldGeneratePanel;

    // 新增：场景名称参数（更灵活）
    public void LoadScene()
    {
        savePanel.SetActive(false);
        worldGeneratePanel.SetActive(false);
        StartCoroutine(LoadLevel());
    }

    // 修改后的协程
    IEnumerator LoadLevel()
    {
        loadScreen.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync("PlayerControl");
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            text.text = $"{progress * 100:F0}%";

            if (operation.progress >= 0.9f)
            {
                text.text = "Press Any Button To Continue";
                if (Input.anyKeyDown)
                {
                    operation.allowSceneActivation = true;
                }
            }
            yield return null;
        }
    }
}