using System;
using System.Collections.Generic;
using System.Globalization;
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
    public GameObject loggingErrorBackground;
    public GameObject loggingSuccessBackground;
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
    public Button logButton;
    public InputField dynamicInput;
    public InputField trafficLightInput;
    public InputField timeToSetInput;
    public InputField timeToCutInput;
    private bool isPaused = true;
    private float[] scales = {1, 2, 5};
    private int currentScale = 0;
    private bool firstStart = true;
    private Dictionary<string, string> changes = new Dictionary<string, string>();

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
        logButton.onClick.AddListener(LogResults);
        menuCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
        avgTrafficNumbers.SetActive(false);
        crossingAvgBackground.SetActive(false);
        loggingErrorBackground.SetActive(false);
        loggingSuccessBackground.SetActive(false);
    }

    void Pause(){
        if(isPaused){
            Dictionary<int, string> carChanges = Settings.Instance.GetChanges();
            Time.timeScale = scales[currentScale];
            pauseButton.GetComponentInChildren<Text>().text = "Pause";

            if(firstStart){
                Debug.Log(FileBuilder.Instance.CollectValues());   
                firstStart = false;
            }

            else {

            foreach (string key in changes.Keys) {
                FileBuilder.Instance.AddChange(changes[key]);
            }

            changes.Clear();

            foreach(int key in carChanges.Keys) {
                FileBuilder.Instance.AddChange(carChanges[key]);
            }
            Settings.Instance.ClearChanges();
            Debug.Log(FileBuilder.Instance.GetChanges());
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
            if(!firstStart && weight != TrafficLight.Instance.GetDynamicWeight()) {
                    changes["Weight"] = "weight to " + weight;
            }
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
            if(!firstStart && time != TrafficLight.Instance.GetTime()) {
                    changes["LightTime"] = "traffic light time from " + TrafficLight.Instance.GetTimeToSet().ToString("F2") +  " to " + time.ToString("F2");
            }
            TrafficLight.Instance.SetTime(time);
            trafficLightInput.text = TrafficLight.Instance.GetTime().ToString();

        } catch (Exception e) {
            trafficLightInput.text = "Enter a positive float";
        }
    }
        // Time to set refers to the time when the traffic light begins to check
        // the other lanes for allocating the next streetlight to be turned on.
        void ChangeTimeToSet() {
        try {
            float time = float.Parse(timeToSetInput.text, CultureInfo.InvariantCulture.NumberFormat);
            if (time < 0 || time > 1) throw new Exception();

            if(!firstStart && time != TrafficLight.Instance.GetTimeToSet()) {
                    changes["TimeToSet"] = "time to set from " + TrafficLight.Instance.GetTimeToSet().ToString("F2") +  " to " + time.ToString("F2");
            }
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

            if(!firstStart && time != TrafficLight.Instance.GetTimeToCut()) {
                    changes["TimeToCut"] = "Time to cut from " + TrafficLight.Instance.GetTimeToCut().ToString("F2") +  " to " + time.ToString("F2");
            }

            TrafficLight.Instance.SetTimeToCut(time);
            timeToCutInput.text = TrafficLight.Instance.GetTimeToCut().ToString();

        } catch (Exception e) {
            timeToCutInput.text = "Enter a positive float between 0 and 1";
        }
    }

    void LogResults(){
      if(TrafficLight.Instance.GetTimeElapsed() < 60) {
        loggingErrorBackground.SetActive(true);
        return;
      } 
      loggingErrorBackground.SetActive(false);
      loggingSuccessBackground.SetActive(true);
      FileBuilder.Instance.LogResult();
    }

    public bool GetFirstStart() {
        return firstStart;
    }
}
