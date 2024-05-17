using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrafficLight : MonoBehaviour
{
    // Start is called before the first frame update
    public static TrafficLight Instance { get; private set;}
    float timeToChange = 15;
    List<GameObject> lights = new List<GameObject>();
    float saveTime;
    int track = 0;
    int path1traffic = 0;
    int path2traffic = 0;
    int path3traffic = 0;
    int path4traffic = 0;
    int totalTraffic = 0;
    public Text trafficText;
    public Text pathText;

    private void Awake() {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keeps the instance alive across scenes
        }
    }

    void Start()
    {
        lights.Add(transform.GetChild(0).GetChild(3).gameObject);
        lights.Add(transform.GetChild(1).GetChild(3).gameObject);
        lights.Add(transform.GetChild(2).GetChild(3).gameObject);
        lights.Add(transform.GetChild(3).GetChild(3).gameObject);
        saveTime = timeToChange;


    }

    // Update is called once per frame
    void Update()
    {
        CurrentGreenAndFrontRightGreen();

    }

    void basicSystem(){
          if(timeToChange >= 0) {
            timeToChange -= Time.deltaTime;
        } else {
            Debug.Log("Time out");
            timeToChange = saveTime;
            lights[track].gameObject.layer = 8;
            track++;
            if(track > 3) track = 0;
            lights[track].gameObject.layer = 9;
            }
    }

    void CurrentGreenAndFrontRightGreen() {
           if(timeToChange >= 0) {
            timeToChange -= Time.deltaTime;
        } else {
            int frontTrack = track+2;
            if(frontTrack > 3) frontTrack -= 4;
            Debug.Log("Time out");
            
            timeToChange = saveTime;

            lights[track].gameObject.layer = 8;
            lights[frontTrack].gameObject.layer = 8;
            
            track++;

            if(track > 3) track = 0;
            lights[track].gameObject.layer = 9;

            frontTrack++;
            if(frontTrack > 3) frontTrack -= 4;
            lights[frontTrack].gameObject.layer = 10;
            
            }
    }

    public void IncrementTraffic(int mode) {
        switch(mode) {
            case 1:
                path1traffic++;
                break;

            case 2:
                path2traffic++;
                break;

            case 3:
                path3traffic++;
                break;

            case 4:
                path4traffic++;
                break;
        }

        totalTraffic++;
        UpdateTrafficText();
    }

        public void DecrementTraffic(int mode) {
        switch(mode) {
            case 1:
                path1traffic--;
                break;

            case 2:
                path2traffic--;
                break;

            case 3:
                path3traffic--;
                break;

            case 4:
                path4traffic--;
                break;
        }

        totalTraffic--;
        UpdateTrafficText();
    }

    private void UpdateTrafficText() {
        trafficText.text = "Total cars: " + totalTraffic;
    }

    public int GetPath1Traffic() {
        return path1traffic;
    }

    

    }