using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    
    private AudioSource audioSource;
    public enum UIState
    {
        None,        
        Map,    
        Settings,
        Controls,
        PauseMenu,   
        Bag,  
        Craft
    }

    // 当前 UI 状态
    [SerializeField]private UIState currentState = UIState.None;
    
    [Space] 
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject SettingUI;
    [SerializeField] private GameObject ControlsUI;
    [SerializeField] private GameObject MapUI;
    [SerializeField] private GameObject BagUI;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject CraftUI;
    [SerializeField] private GameObject BlackOverlay;
    [SerializeField] private GameObject DarkScreen;
    
    [SerializeField] public  bool GameIsPaused = false;
    [SerializeField] private InventoryManager inventoryUI; // 背包 UI 脚本
    [SerializeField] private Save save;
    
    private bool isPauseOpen = false;
    private bool isMapOpen = false;
    private bool isBagOpen = false;
    private bool isCraftOpen = false;
    
    private GameManager gameManager;

    private void Start()
    {
        // 在场景中搜索第一个匹配的GameManager组件
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager != null)
        {
            Debug.Log("成功找到GameManager");
        }
        else
        {
            Debug.LogError("未找到GameManager！请确保它存在于场景中");
        }
        
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            audioSource.Play();
            HandleEscUse();
        }

        if (Input.GetKeyDown(KeyCode.M)&&(currentState == UIState.None||currentState == UIState.Map))
        {
            audioSource.Play();
            MapControl();
        }
        
        if (Input.GetKeyDown(KeyCode.B)&&(currentState == UIState.None||currentState == UIState.Bag))
        {
            audioSource.Play();
            BagUIControl();
        }
        
        if (Input.GetKeyDown(KeyCode.C)&&(currentState == UIState.None||currentState == UIState.Craft))
        {
            audioSource.Play();
            CraftUIControl();
        }
    }
    private void HandleEscUse()
    {
        switch (currentState)
        {
            case UIState.Map:
                MapUI.SetActive(false);
                ChangeUIState(UIState.None);
                GameResume();
                break;
            
            case UIState.Bag:
                BagUI.SetActive(false);
                ChangeUIState(UIState.None);
                GameResume();
                break;

            case UIState.Settings:
                SettingUI.SetActive(false);
                PauseMenu.SetActive(true);
                ChangeUIState(UIState.PauseMenu);
                break;
            
            case UIState.Controls:
                SettingUI.SetActive(false);
                ControlsUI.SetActive(false);
                PauseMenu.SetActive(true);
                ChangeUIState(UIState.PauseMenu);
                break;

            case UIState.PauseMenu: //close pausemenu and resume the game
                ControlPauseMenu();
                ChangeUIState(UIState.None);
                GameResume();
                break;

            case UIState.None: // open pause menu and stop the game
                ControlPauseMenu();
                ChangeUIState(UIState.PauseMenu);
                GamePause();
                break;
        }
    }
    public void ChangeUIState(UIState newState)
    {
        currentState = newState;
        // UpdateUIPanels();
    }
    public void MapControl()
    {
        if (!isMapOpen)
        {
            ChangeUIState(UIState.Map);
            MapUI.SetActive(true);
            isMapOpen = true;
            GamePause();
        }
        else
        {
            ChangeUIState(UIState.None);
            MapUI.SetActive(false);
            isMapOpen = false;
            GameResume();
        }
    }
    public void ControlPauseMenu()
    {
        if(currentState == UIState.PauseMenu)
        {
            PauseMenu.SetActive(false);
            isPauseOpen = !isPauseOpen;
        }
        else
        {
            PauseMenu.SetActive(true);
            isPauseOpen = !isPauseOpen;

        }
    }
    // ReSharper disable Unity.PerformanceAnalysis
    public void BagUIControl()
    {
        if (!isBagOpen)
        {
            inventoryUI.UpdateBagUI();
            ChangeUIState(UIState.Bag);
            BagUI.SetActive(true);
            isBagOpen = true;
            GamePause();
        }
        else
        {
            ChangeUIState(UIState.None);
            BagUI.SetActive(false);
            isBagOpen = false;
            GameResume();
        }
    }
    
    public void CraftUIControl()
    {
        if (!isCraftOpen)
        {
            ChangeUIState(UIState.Craft);
            CraftUI.SetActive(true);
            isCraftOpen = true;
            GamePause();
        }
        else
        {
            ChangeUIState(UIState.None);
            CraftUI.SetActive(false);
            isCraftOpen = false;
            GameResume();
        }
    }
    public void Quit()
    {
        save.SaveAllData(gameManager.selectedSlot);
        Application.Quit();
    }
    public void GamePause()
    {
        Time.timeScale = 0.0f;
        GameIsPaused = true;
    }
    public void GameResume()
    {
        Time.timeScale = 1.0f;
        GameIsPaused = false;
        ChangeUIState(UIState.None);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single); // 保留游戏场景
    }

    #region Button function
    
    public void SettingButton()
    {
        PauseMenu.SetActive(false);
        ControlsUI.SetActive(false);
        SettingUI.SetActive(true);
        ChangeUIState(UIState.Settings);
    }

    public void ControlsButton()
    {
        SettingUI.SetActive(false);
        ControlsUI.SetActive(true);
        ChangeUIState(UIState.Controls);
    }

    public void BackButton()
    {
        SettingUI.SetActive(false);
        ControlsUI.SetActive(false);
        PauseMenu.SetActive(true);
        ChangeUIState(UIState.PauseMenu);
    }

    public void ResumeButton()
    {
        PauseMenu.SetActive(false);
        GameResume();
        ChangeUIState(UIState.None);
    }

    public void PauseButton()
    {
        PauseMenu.SetActive(true);
        GamePause();
        ChangeUIState(UIState.PauseMenu);
    }
    
    public void MapButton()
    {
        ChangeUIState(UIState.Map);
        MapUI.SetActive(true);
    }

    public void ActivateDarkScreen()
    {
        DarkScreen.SetActive(true);
    }
    public void CloseDarkScreen()
    {
        DarkScreen.SetActive(false);
        PlayerRespawn();
    }

    private void PlayerRespawn()
    {
        PlayerManager.instance.player.anim.SetBool("Die", false);
        PlayerManager.instance.player.transform.position = PlayerManager.instance.player.guide.position + new Vector3(-1, 0, 0);
        PlayerManager.instance.player.stateMachine.ChangeState(PlayerManager.instance.player.idleState);
    }
    #endregion
}