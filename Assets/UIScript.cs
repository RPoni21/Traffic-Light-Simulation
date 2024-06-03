using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    
    public static UIScript Instance { get; private set;}
    public  GameObject regularCanvas;
    public GameObject menuCanvas;
    public GameObject settingsCanvas;
    public GameObject trafficNumbers;
    public GameObject avgTrafficNumbers;
    public GameObject crossingAvgBackground;
    public Button pauseButton;
    public Button totalsAverageButton;
    public Button setSpeedButton;
    public Button menuButton;
    public Button closeMenuButton;
    public Button dynamicButton;
    public Button trafficLightButton;
    public Button timeToSetButton;
    public Button timeToCutButton;
    public Button settingsButton;
    public Button settingsBackButton;
    public InputField dynamicInput;
    public InputField trafficLightInput;
    public InputField timeToSetInput;
    public InputField timeToCutInput;
    private bool isPaused = true;
    private float[] scales = {1, 2, 5};
    private int currentScale = 0;
    private bool firstStart = true;

    private void Awake() {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start(){
        Time.timeScale = 0;
        pauseButton.GetComponentInChildren<Text>().text = "Pause";
        setSpeedButton.GetComponentInChildren<Text>().text = "1X";
        menuButton.GetComponentInChildren<Text>().text = "Menu";
        pauseButton.onClick.AddListener(Pause);
        settingsButton.onClick.AddListener(switchToSettings);
        settingsBackButton.onClick.AddListener(goBackToMenu);
        totalsAverageButton.onClick.AddListener(SwitchTotalsAverages);
        setSpeedButton.onClick.AddListener(ChangeTimeScale);
        menuButton.onClick.AddListener(ShowMenu);
        closeMenuButton.onClick.AddListener(CloseMenu);
        dynamicButton.onClick.AddListener(ChangeWeight);
        trafficLightButton.onClick.AddListener(ChangeTrafficLightTime);
        timeToSetButton.onClick.AddListener(ChangeTimeToSet);
        timeToCutButton.onClick.AddListener(ChangeTimeToCut);
        menuCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
        avgTrafficNumbers.SetActive(false);
        crossingAvgBackground.SetActive(false);
    }

    void Pause(){
        if(isPaused){
            Time.timeScale = scales[currentScale];
            pauseButton.GetComponentInChildren<Text>().text = "Pause";

            if(firstStart){
                FileBuilder.Instance.CollectInitial();   
                firstStart = false;
            }
        } else {
            Time.timeScale = 0;
             pauseButton.GetComponentInChildren<Text>().text = "Resume";
        }
            isPaused = !isPaused;
    }

    void ChangeTimeScale() {
        currentScale = (currentScale + 1) % 3;
        setSpeedButton.GetComponentInChildren<Text>().text = scales[currentScale] + "X";
        if(!isPaused){
            Time.timeScale = scales[currentScale];
        }
    }

    void ShowMenu(){
        regularCanvas.SetActive(false);
        menuCanvas.SetActive(true);
        isPaused = false;
        Pause();
    }
    
    void CloseMenu(){
        regularCanvas.SetActive(true);
        menuCanvas.SetActive(false);
    }

    void SwitchTotalsAverages() {
        if (trafficNumbers.activeSelf){
        trafficNumbers.SetActive(false);
        avgTrafficNumbers.SetActive(true);
        crossingAvgBackground.SetActive(true);
        totalsAverageButton.GetComponentInChildren<Text>().text = "Totals";
        }
        
        else {
            trafficNumbers.SetActive(true);
            avgTrafficNumbers.SetActive(false);
            crossingAvgBackground.SetActive(false);
        totalsAverageButton.GetComponentInChildren<Text>().text = "Averages";
        }
    }

    void switchToSettings() {
        menuCanvas.SetActive(false);
        settingsCanvas.SetActive(true);
    }

    void goBackToMenu() {
        settingsCanvas.SetActive(false);
        menuCanvas.SetActive(true);
    }

    void ChangeWeight() {
        try {
            float weight = float.Parse(dynamicInput.text, CultureInfo.InvariantCulture.NumberFormat);
            if(weight < 0 || weight > 1) throw new Exception();
            TrafficLight.Instance.SetDynamicWeight(weight);
            dynamicInput.text = TrafficLight.Instance.GetDynamicWeight().ToString();

        } catch (Exception e) {
            dynamicInput.text = "Enter a positive float between 0 and 1";
        }
    }

    void ChangeTrafficLightTime() {
        try {
            float time = float.Parse(trafficLightInput.text, CultureInfo.InvariantCulture.NumberFormat);
            if(time < 0) throw new Exception();
            TrafficLight.Instance.SetTime(time);
            trafficLightInput.text = TrafficLight.Instance.GetTime().ToString();

        } catch (Exception e) {
            trafficLightInput.text = "Enter a positive float";
        }
    }
        // Time to set refers to the time when the traffic light begins to check
        // the other lanes for allocating  the next streetlight to be turned on.
        void ChangeTimeToSet() {
        try {
            float time = float.Parse(timeToSetInput.text, CultureInfo.InvariantCulture.NumberFormat);
            if (time < 0 || time > 1) throw new Exception();
            TrafficLight.Instance.SetTimeToSet(time);
            timeToSetInput.text = TrafficLight.Instance.GetTimeToSet().ToString();

        } catch (Exception e) {
            timeToSetInput.text = "Enter a positive float between 0 and 1";
        }
    }
       void ChangeTimeToCut() {
        try {
            float time = float.Parse(timeToCutInput.text, CultureInfo.InvariantCulture.NumberFormat);
            if (time < 0 || time > 1) throw new Exception();
            TrafficLight.Instance.SetTimeToCut(time);
            timeToCutInput.text = TrafficLight.Instance.GetTimeToCut().ToString();

        } catch (Exception e) {
            timeToCutInput.text = "Enter a positive float between 0 and 1";
        }
    }

    public bool GetFirstStart() {
        return firstStart;
    }
}
