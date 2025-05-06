using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Mainmenu : MonoBehaviour
{
    [SerializeField] private GameObject SaveSelectPanel;
    [SerializeField] private LoadManager loadManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private InputField seedInput;
    // [SerializeField] private InputField densityInput;
    [SerializeField] private Slider MapRatioInput;
    [SerializeField] private Slider EnemyRatioInput;
    [SerializeField] private Slider ItemRatioInput;

    [SerializeField] private GameObject savePanel;
    [SerializeField] private GameObject worldGeneratePanel;
    // Start is called before the first frame update
    
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject logo;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SaveSelectPanel.SetActive(false);
        }
    }

    public void NewGame()
    {
        SaveSelectPanel.SetActive(true);
        startButton.SetActive(false);
        exitButton.SetActive(false);
        logo.SetActive(false);
    }

    public void ContinueGame()
    {
        // loadManager.LoadScene();
    }

    public void CreateNewWorld()
    {
        if (string.IsNullOrEmpty(seedInput.text))
        {
            gameManager.mapSeed = GetHashCode();
        }
        else
        {
            gameManager.mapSeed = int.Parse(seedInput.text);
        }
        gameManager.MapRatio = MapRatioInput.value;
        gameManager.EnemyRatio = EnemyRatioInput.value;
        gameManager.ItemRatio = ItemRatioInput.value;
        loadManager.LoadScene();
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void SavePanelBack()
    {
        SaveSelectPanel.SetActive(false);
        startButton.SetActive(true);
        exitButton.SetActive(true);
        logo.SetActive(true);
    }
}
