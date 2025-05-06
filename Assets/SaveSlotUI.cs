using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System;
using Unity.VisualScripting;

public class SaveSlotUI : MonoBehaviour
{
    // 基础配置
    [SerializeField] private int slotNumber;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private Button deleteButton;
    [SerializeField] private TextMeshProUGUI saveTimeText;

    // UI 引用
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject worldGenePanel;
    
    // 系统组件
    [SerializeField] private LoadManager loadManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ConfirmationDialog confirmationDialog;

    private void Start()
    {
        InitializeSaveSlot();
    }

    private void InitializeSaveSlot()
    {
        // 绑定删除按钮事件
        if(deleteButton != null)
            deleteButton.onClick.AddListener(OnDeleteButtonClicked);
            
        RefreshSlotUI();
    }

    private void RefreshSlotUI()
    {
        string savePath = GetSavePath();
        bool saveExists = File.Exists(savePath);

        statusText.text = saveExists ? "Continue" : "New Game";
        saveTimeText.text = saveExists ? GetFileTimeString(savePath) : "No Data";

        if (statusText.text == "Continue")
        {
            deleteButton.GameObject().SetActive(true);
        }
        else
        {
            deleteButton.GameObject().SetActive(false);
        }
    }


    private string GetFileTimeString(string path)
    {
        try
        {
            DateTime lastWriteTime = File.GetLastWriteTime(path);
            return lastWriteTime.ToString("yyyy-MM-dd HH:mm");
        }
        catch (Exception e)
        {
            Debug.LogError($"获取文件时间失败: {e.Message}");
            return "Invalid Time";
        }
    }

    public void OnButton()
    {
        gameManager.selectedSlot = slotNumber;

        if (statusText.text == "Continue")
        {
            gameManager.isNewGame = false;
            panel.SetActive(false);
            loadManager.LoadScene();
        }
        else
        {
            gameManager.isNewGame = true;
            panel.SetActive(false);
            worldGenePanel.SetActive(true);
        }
    }

    #region 删除功能实现
    private void OnDeleteButtonClicked()
    {
        if(File.Exists(GetSavePath()))
        {
            confirmationDialog.ShowDialog(
                "Confirmation",
                $"Confirm Delete Save {slotNumber} ?",
                DeleteSave
            );
        }
    }

    private void DeleteSave()
    {
        try
        {
            File.Delete(GetSavePath());
            RefreshSlotUI();
        }
        catch (Exception e)
        {
            Debug.LogError($"删除失败: {e.Message}");
            // confirmationDialog.ShowDialog("error", "删除操作失败");
        }
    }
    #endregion

    #region 工具方法
    private string GetSavePath()
    {
        return Path.Combine(
            Application.persistentDataPath, 
            "Saves", 
            $"save{slotNumber}.json"
        );
    }
    #endregion
}



