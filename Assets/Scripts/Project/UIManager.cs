using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using GameAnalyticsSDK;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    public static UIManager Instante;

    [Header("Buttons")]
    [SerializeField] GameObject SettingButton;
    

    [Header("Panels")]
    [SerializeField] GameObject SettingPanels;

    [Header("Colors")]
    [SerializeField] Color GreyColor;
    [SerializeField] Color WhiteColor;
    [SerializeField] Image HapticImage;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI GoldTMP;
    [SerializeField] int TotalGoldAmount = 0;
    [SerializeField] TextMeshProUGUI LevelText;
    [Header("Levels")]
    [SerializeField] GameObject[] Levels;
    [Header("Level Event")]
    public string LevelEvent = "";
    public int LevelIndex = 0;
    public int FakeLevel = 0;
    private void Awake()
    {
        Instante = this;
        //GameAnalytics.Initialize();
    }
    void Start() 
    {   
        TotalGoldAmount = PlayerPrefs.GetInt("TotalGold");
        LevelIndex = PlayerPrefs.GetInt("LevelIndex", 1);
        FakeLevel = PlayerPrefs.GetInt("FakeLevel", 0);
        if (FakeLevel >= Levels.Length)
            FakeLevel = 0;

        Levels[FakeLevel].SetActive(true);
        //LevelEvent = "Level " + LevelIndex.ToString();
        //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, LevelEvent);

        LevelText.text = "LEVEL " + LevelIndex.ToString();
        GoldText(0);
    }
    public void GoldText(int _goldValue)
    {
        TotalGoldAmount += _goldValue;
        GoldTMP.text = TotalGoldAmount.ToString();
        PlayerPrefs.SetInt("TotalGold", TotalGoldAmount);
    }
    public void LevelComplete()
    {
        //LevelEvent = "Level " + LevelIndex.ToString();
        //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, LevelEvent);
        LevelIndex++;
        PlayerPrefs.SetInt("LevelIndex", LevelIndex);
        FakeLevel++;
        PlayerPrefs.SetInt("FakeLevel", FakeLevel);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void RestartLevel()
    {
        //LevelEvent = "Level " + LevelIndex.ToString();
        //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, LevelEvent);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OptionsOpen()
    {
        SettingPanels.SetActive(true);
        SettingButton.SetActive(false);
        MoveControll.Instante.IsMove = false;
    }
    public void OptionsClose()
    {
        SettingPanels.SetActive(false);
        SettingButton.SetActive(true);
        MoveControll.Instante.IsMove = true;
    }
    public void Haptic()
    {
        if(PlayerPrefs.GetInt("Haptic")==0)
        {
            Vibration.IsOpen = false;
            PlayerPrefs.SetInt("Haptic", 1);
            HapticImage.color = GreyColor;
        }
        else if (PlayerPrefs.GetInt("Haptic") == 1)
        {
            Vibration.IsOpen = true;
            PlayerPrefs.SetInt("Haptic", 0);
            HapticImage.color = WhiteColor;
        }
    }
}
