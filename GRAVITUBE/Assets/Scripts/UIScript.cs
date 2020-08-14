using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System;

public class UIScript : MonoBehaviourPunCallbacks
{
    public static bool pauseMenu = false;
    public static bool gameOver = false;
    public Canvas pauseoroverCanvas;
    public TextMeshProUGUI pauseTitle;
    public Slider loadingBar;
    public Image FadeImageOverlay;
    public TextMeshProUGUI highScoreTextMainMenu,currencyTextMainMenu;
    public GameObject UpgradesPanel;
    public GameObject SettingsPanel;

    public Slider mouseSensitivitySlider;
    public TextMeshProUGUI sensitivityValueText;

    public GameObject namePanel;
    public GameObject mainPanel;

    public static int mouseSensitivity;
    public static string playerName="";

    public MessagePanel messagePanel;

    private void Start()
    {
        mouseSensitivity = PlayerPrefs.GetInt("SENSITIVITY");
        playerName = PlayerPrefs.GetString("NAME");
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (string.IsNullOrEmpty(playerName)){
             
                namePanel.SetActive(true);
                mainPanel.SetActive(false);
            }
            else
            {
  
                namePanel.SetActive(false);
                mainPanel.SetActive(true);
            }
        }
        if (mouseSensitivity == 0) mouseSensitivity = 3;
        if (mouseSensitivitySlider != null)
        {
            mouseSensitivitySlider.value = mouseSensitivity;
        }
        if (loadingBar != null)
        {
            loadingBar.gameObject.SetActive(false);
        }
        if (highScoreTextMainMenu!= null)
        {
            highScoreTextMainMenu.text = "HIGH SCORE: "+PlayerPrefs.GetInt("HIGHSCORE");
        }
        if (currencyTextMainMenu != null)
        {
            currencyTextMainMenu.text = "X " + PlayerController.currencyMeter;
        }

        PlayerController.loadSavedData();
      
    }
    


    public void LoadSinglePlayer()
    {
        loadingBar.gameObject.SetActive(true);
        TubeGenerator.Reset();
        StartCoroutine(AsyncLoadScene(1));
        
    }

    IEnumerator AsyncLoadScene(int sceneIndex)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!loadOperation.isDone)
        {
            if(loadingBar!= null)
            loadingBar.value = Mathf.Clamp01(loadOperation.progress/0.9f);
            yield return null;
        }
    }

    public void LoadRoomLobby()
    {
        
        TubeGenerator.Reset();
       
        StartCoroutine(checkInternetConnection((isConnected) => {
            if (isConnected)
            {
                SceneManager.LoadScene(2);
            }
            else
            {

                messagePanel.printMessage("NO INTERENT, CHECK YOUR INTERNET CONNECTION!");
            }   
        
        }));
        
    }

    IEnumerator checkInternetConnection(Action<bool> action)
    {
        WWW www = new WWW("http://google.com");
        yield return www;
        if (www.error != null)
        {
            action(false);
        }
        else
        {
            action(true);
        }
    }

    public void Update()
    {
        if(mouseSensitivitySlider!=null && sensitivityValueText != null)
        {
            sensitivityValueText.text = mouseSensitivitySlider.value + "";
        }
        if (FadeImageOverlay != null)
        {
            FadeImageOverlay.color = Color.Lerp(FadeImageOverlay.color, new Color(FadeImageOverlay.color.r, FadeImageOverlay.color.g, FadeImageOverlay.color.b, 0), Time.deltaTime * 10);
          
        }
        if (currencyTextMainMenu != null)
        {
            currencyTextMainMenu.text = PlayerController.currencyMeter + "";
        }
        Time.timeScale = 1;
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            
            if (!pauseMenu && Input.GetKeyDown(KeyCode.Escape)&&!gameOver)
            {
                pauseMenu = true;
                pauseoroverCanvas.gameObject.SetActive(true);
                
            }
            else if (pauseoroverCanvas && Input.GetKeyDown(KeyCode.Escape) && !gameOver)
            {
                pauseoroverCanvas.GetComponent<CanvasGroup>().alpha = 0;
                pauseoroverCanvas.gameObject.SetActive(false);
                pauseMenu = false;
            }
            if (pauseMenu)
            {
                pauseoroverCanvas.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(pauseoroverCanvas.GetComponent<CanvasGroup>().alpha, 1, Time.deltaTime * 10);
                if(pauseoroverCanvas.GetComponent<CanvasGroup>().alpha>0.9f)
                   Time.timeScale = 0;
                pauseTitle.text = "PAUSED";
            }
         
            if (gameOver)
            {
                pauseoroverCanvas.gameObject.SetActive(true);
                pauseoroverCanvas.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(pauseoroverCanvas.GetComponent<CanvasGroup>().alpha, 1, Time.deltaTime * 10);
                if (pauseoroverCanvas.GetComponent<CanvasGroup>().alpha > 0.9f)
                    Time.timeScale = 0;

                pauseTitle.text = "OUT OF FUEL";
            }
           
            }
    }
    
    void playSound(int id=0)
    {
        
        try
        {
            if (GetComponents<AudioSource>()[id] != null)
            {
                try
                {
                    GetComponents<AudioSource>()[id].Play();

                }
                catch (System.ArgumentNullException a) { }


            }
        }
        catch (System.IndexOutOfRangeException i) { }
    }
    public void OnClickExit()
    {
        playSound();

        PlayerPrefs.SetInt("CURRENCY", PlayerController.currencyMeter);
        if (PlayerController.highScore < PlayerController.score)
        {
            PlayerController.highScore =(int) PlayerController.score;
            PlayerPrefs.SetInt("HIGHSCORE", PlayerController.highScore);
        }
        TubeGenerator.Reset();
        
        SceneManager.LoadScene(0);

    }

    public void OnClickExitGame()
    {
        playSound();
        Application.Quit();
    }

    public void OnClickUpgradeBut()
    {
        playSound();
        if (UpgradesPanel != null)
        {
            NameFieldProp.my_InputField.text = "";
            SettingsPanel.GetComponent<Animator>().SetBool("UpgradeMode", false);

            bool upgradeMode = UpgradesPanel.GetComponent<Animator>().GetBool("UpgradeMode");
            UpgradesPanel.GetComponent<Animator>().SetBool("UpgradeMode", !upgradeMode);
            SettingsPanel.GetComponent<Animator>().SetBool("UpgradeMode", false);
            
        }
    }

    public void OnClickSettingsBut()
    {
        playSound();
        if (SettingsPanel != null)
        {
            mouseSensitivitySlider.value = mouseSensitivity;  
            bool settingsMode = SettingsPanel.GetComponent<Animator>().GetBool("UpgradeMode");
            SettingsPanel.GetComponent<Animator>().SetBool("UpgradeMode", !settingsMode);
            UpgradesPanel.GetComponent<Animator>().SetBool("UpgradeMode", false);
        }

    }

    public void OnClickApply(bool fresh)
    {
        playSound();
        if (!fresh)
        {
            PlayerPrefs.SetInt("SENSITIVITY", (int)mouseSensitivitySlider.value);
            mouseSensitivity = (int)mouseSensitivitySlider.value;
            if (!string.IsNullOrEmpty(NameFieldProp.name))
           {
                PlayerPrefs.SetString("NAME", NameFieldProp.name);
                playerName = NameFieldProp.name;
            }
                NameFieldProp.my_InputField.text = "";
            SettingsPanel.GetComponent<Animator>().SetBool("UpgradeMode", false);
        }
        else
        {
            PlayerPrefs.SetString("NAME", NameFieldProp.name);
            namePanel.SetActive(false);
            NameFieldProp.my_InputField.text = "";
            playerName = NameFieldProp.name;
            mainPanel.SetActive(true);
        }
        }

    public void OnClickCancelBut()
    {
        playSound();
        NameFieldProp.my_InputField.text = "";
        mouseSensitivitySlider.value = mouseSensitivity;
        SettingsPanel.GetComponent<Animator>().SetBool("UpgradeMode", false);
    }

    public void Upgrade(int id)
    {
        playSound(1);
        switch (id)
        {
            case 1:
                PlayerController.speedBonus++;
                PlayerController.currencyMeter -= PlayerController.speedBonus * 500;
                PlayerPrefs.SetInt("SPEEDBONUS", PlayerController.speedBonus);
                PlayerPrefs.SetInt("CURRENCY", PlayerController.currencyMeter);
                break;

            case 2:
                PlayerController.forceBonus++;
                PlayerPrefs.SetInt("FORCEBONUS", PlayerController.forceBonus);
                PlayerController.currencyMeter -= PlayerController.forceBonus * 500;
                PlayerPrefs.SetInt("CURRENCY", PlayerController.currencyMeter);
                break;
            case 3:
                PlayerController.magnetBonus++;
                PlayerPrefs.SetInt("MAGNETBONUS", PlayerController.magnetBonus);
                PlayerController.currencyMeter -= PlayerController.magnetBonus * 500;
                PlayerPrefs.SetInt("CURRENCY", PlayerController.currencyMeter);
                break;
            case 4:
                PlayerController.boostBonus++;
                PlayerPrefs.SetInt("BOOSTBONUS", PlayerController.boostBonus);
                PlayerController.currencyMeter -= PlayerController.boostBonus * 500;
                PlayerPrefs.SetInt("CURRENCY", PlayerController.currencyMeter);
                break;
            case 5:
                PlayerController.fuelBonus++;
                PlayerPrefs.SetInt("FUELBONUS", PlayerController.fuelBonus);
                PlayerController.currencyMeter -= PlayerController.fuelBonus * 500;
                PlayerPrefs.SetInt("CURRENCY", PlayerController.currencyMeter);
                break;
        }
    }

    public void OnClickLeaveLobby()
    {
        playSound();
        SceneManager.LoadScene(0);
    }

    public void OnClickLeaveGame()
    {
        playSound();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (messagePanel != null)
        {
            if (newMasterClient != PhotonNetwork.LocalPlayer)
                messagePanel.printMessage("HOST LEFT THE GAME! " + newMasterClient.NickName + " is the new Host!");
            else
                messagePanel.printMessage("HOST LEFT THE GAME! You are the new Host!");
        }
        }



}
