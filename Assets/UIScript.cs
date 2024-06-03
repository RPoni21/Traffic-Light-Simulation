using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public  GameObject regularCanvas;
    public GameObject menuCanvas;
    public GameObject settingsCanvas;
    public Button pauseButton;
    public Button setSpeedButton;
    public Button menuButton;
    public Button closeMenuButton;
    public Button dynamicButton;
    public Button trafficLightButton;
    public InputField dynamicInput;
    public InputField trafficLightInput;
    private bool isPaused = false;
    private float[] scales = {1, 2, 5};
    private int currentScale = 0;

    void Start(){
        pauseButton.GetComponentInChildren<Text>().text = "Pause";
        setSpeedButton.GetComponentInChildren<Text>().text = "1X";
        menuButton.GetComponentInChildren<Text>().text = "Menu";
        pauseButton.onClick.AddListener(Pause);
        setSpeedButton.onClick.AddListener(ChangeTimeScale);
        menuButton.onClick.AddListener(ShowMenu);
        closeMenuButton.onClick.AddListener(CloseMenu);
        dynamicButton.onClick.AddListener(ChangeWeight);
        trafficLightButton.onClick.AddListener(ChangeTrafficLight);
        menuCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
    }

    void Pause(){
        if(isPaused){
            Time.timeScale = scales[currentScale];
            pauseButton.GetComponentInChildren<Text>().text = "Pause";
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

    void ChangeWeight() {
        try {
            float weight = float.Parse(dynamicInput.text, CultureInfo.InvariantCulture.NumberFormat);

            TrafficLight.Instance.SetDynamicWeight(weight);
            dynamicInput.text = TrafficLight.Instance.GetDynamicWeight().ToString();

        } catch (Exception e) {
            dynamicInput.text = "Enter a float";
        }
    }

    void ChangeTrafficLight() {
        try {
            float time = float.Parse(trafficLightInput.text, CultureInfo.InvariantCulture.NumberFormat);

            TrafficLight.Instance.SetTime(time);
            trafficLightInput.text = TrafficLight.Instance.GetTime().ToString();

        } catch (Exception e) {
            trafficLightInput.text = "Enter a float";
        }
    }
}
