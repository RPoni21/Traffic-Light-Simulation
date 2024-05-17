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
    GameObject active;
    float saveTime;
    int track = 0;
    int path1traffic = 0;
    int path2traffic = 0;
    int path3traffic = 0;
    int path4traffic = 0;
    int totalTraffic = 0;
    List<int> traffic = new List<int>();
    int current;
    int toChange;
    public Text trafficText;
    public Text path1Text;
    public Text path2Text;
    public Text path3Text;
    public Text path4Text;

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

        traffic.Add(path1traffic);
        traffic.Add(path2traffic);
        traffic.Add(path3traffic);
        traffic.Add(path4traffic);

        current = 0;
        toChange = 1;
        saveTime = timeToChange;


    }

    // Update is called once per frame
    void Update()
    {
        basicSystem();

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
            active = lights[track];
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
            active = lights[track]; 

            frontTrack++;
            if(frontTrack > 3) frontTrack -= 4;
            lights[frontTrack].gameObject.layer = 10;
            
            }
    }



    public void IncrementTraffic(int mode) {
        switch(mode) {
            case 1:
                traffic[0]++;
                break;

            case 2:
                traffic[1]++;
                break;

            case 3:
                traffic[2]++;
                break;

            case 4:
                traffic[3]++;
                break;
        }

        totalTraffic++;
        UpdatePathText(mode);
        UpdateTrafficText();
    }

        public void DecrementTraffic(int mode) {
        switch(mode) {
            case 1:
                traffic[0]--;
                break;

            case 2:
                traffic[1]--;
                break;

            case 3:
                traffic[2]--;
                break;

            case 4:
                traffic[3]--;
                break;
        }

        totalTraffic--;
        UpdatePathText(mode);
        UpdateTrafficText();
    }

    private void HandleTraffic()
    {
        for(int i = 0; i < traffic.Count; i++)
        {
            if ((float)traffic[i] >= (float)current*1.5f) {
            
                

            }
        }
    }

    private void flip(int red, int green)
    {
        lights[red].gameObject.layer =8;
        lights[green].gameObject.layer = 9;
    }

    private void UpdateTrafficText() {
        trafficText.text = "Total cars: " + totalTraffic;
    }


    private void UpdatePathText(int mode)
    {
        switch(mode)
        {
            case 1:
                path1Text.text = "Cars on Path 1: " + traffic[0];
                break;
            case 2:
                path2Text.text = "Cars on Path 2: " + traffic[1];
                break;
            case 3:
                path3Text.text = "Cars on Path 3: " + traffic[2];
                break;
            case 4:
                path4Text.text = "Cars on Path 4: " + traffic[3];
                break;
        }
    }

    public int GetPath1Traffic() {
        return path1traffic;
    }

    

    }