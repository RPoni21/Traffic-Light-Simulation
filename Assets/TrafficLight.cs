using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TrafficLight : MonoBehaviour
{
    // Start is called before the first frame update
    public static TrafficLight Instance { get; private set;}
    float timeToChange = 15;
    List<GameObject> lights = new List<GameObject>();
    GameObject active;
    float saveTime;

    int path1traffic = 0;
    int path2traffic = 0;
    int path3traffic = 0;
    int path4traffic = 0;
    int totalTraffic = 0;
    List<int> traffic = new List<int>();

    int currentTrack;
    int toChange;
    bool setToChange = true;

    public Text trafficText;
    public Text path1Text;
    public Text path2Text;
    public Text path3Text;
    public Text path4Text;
    public Text avgTrafficText;
    public Text avgPath1Text;
    public Text avgPath2Text;
    public Text avgPath3Text;
    public Text avgPath4Text;
    public Text avgCrossingText;
    public Text avgCrossing1Text;
    public Text avgCrossing2Text;
    public Text avgCrossing3Text;
    public Text avgCrossing4Text;
    public Text greenLightText;
    public Text timeElapsedText;

    private float timeElapsed = 0;
    private float avgTimeToCross = 0;
    private float allTimeToCross = 0;
    private float crossesSent = 1;
    private float[] crossesPath = {0, 0, 0 , 0};
    private float[] crossesSentPath = {1, 1, 1, 1};
    private float[] avgTimeToCrossPath = {0, 0, 0, 0};
    private float avgTraffic;   
    private float[] avgTrafficPath = new float[4];
    private float allObjectsEver = 0;
    private float[] allObjectsPath = {0, 0, 0, 0};
    private float dynamicWeight = 0.75f;
    private float timeToSet = 0.5f;
    private float timeToCut = 0.5f;


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

    void Start()
    {
        Time.timeScale = 0;
        lights.Add(transform.GetChild(0).GetChild(3).gameObject);
        lights.Add(transform.GetChild(1).GetChild(3).gameObject);
        lights.Add(transform.GetChild(2).GetChild(3).gameObject);
        lights.Add(transform.GetChild(3).GetChild(3).gameObject);


        traffic.Add(path1traffic);
        traffic.Add(path2traffic);
        traffic.Add(path3traffic);
        traffic.Add(path4traffic);

        StartBasic();

        currentTrack = 0;
        toChange = 1;
        saveTime = timeToChange;


    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        basicSystem();
        CalculateAverages();
        greenLightText.text = timeToChange.ToString("F2"); 
        timeElapsedText.text = timeElapsed.ToString("F2");

    }

    void basicSystem(){
        if (timeToChange >= saveTime) {
            timeToChange -= Time.deltaTime;
            
        } else if (timeToChange > 0)
        {
            HandleTraffic();
            timeToChange -= Time.deltaTime;
        }
        else
        {
            
            Debug.Log("Time out, changing from " + currentTrack + " to " + toChange);
            timeToChange = saveTime;

            lights[currentTrack].gameObject.layer = 8;
            currentTrack = toChange;
            toChange++;
            if (toChange > 3) toChange = 0;
            lights[currentTrack].gameObject.layer = 9;
            setToChange = true;

            }
    }

    void StartBasic()
    {
        lights[0].gameObject.layer = 9;
        lights[1].gameObject.layer = 8;
        lights[2].gameObject.layer = 8;
        lights[3].gameObject.layer = 8;
    }

    void CurrentGreenAndFrontRightGreen() {
           if(timeToChange >= 0) {
            timeToChange -= Time.deltaTime;
        } else {
            int frontTrack = currentTrack+2;
            if(frontTrack > 3) frontTrack -= 4;
            //Debug.Log("Time out");
            
            timeToChange = saveTime;

            lights[currentTrack].gameObject.layer = 8;
            lights[frontTrack].gameObject.layer = 8;
            
            currentTrack++;

            if(currentTrack > 3) currentTrack = 0;
            lights[currentTrack].gameObject.layer = 9;
            active = lights[currentTrack]; 

            frontTrack++;
            if(frontTrack > 3) frontTrack -= 4;
            lights[frontTrack].gameObject.layer = 10;
            
            }
    }

    public void IncrementTraffic(int mode) {
        if(mode > 0 && mode < 5) {
        traffic[mode-1]++;

        totalTraffic++;
        UpdatePathText(mode);
        UpdateTrafficText();
        }
    }

        public void DecrementTraffic(int mode, float cross) {
        if(mode > 0 && mode < 5){
        traffic[mode-1]--;

        totalTraffic--;
        CalculateAvgCrossingTime(mode, cross);
        UpdatePathText(mode);
        UpdateTrafficText();
        }
    }
    private void HandleTraffic()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i > 3) i = 0;
            if (setToChange && timeToChange <= (saveTime * timeToSet) && ((float)traffic[currentTrack] < (float)traffic[(i + currentTrack) % 4] * dynamicWeight))
            {
                timeToChange *= timeToCut;

                toChange = (i + currentTrack) % 4;
                setToChange = false;

            }
        }
    }

    private void UpdateTrafficText() {
        trafficText.text = "Total cars: " + totalTraffic;
    }


    private void UpdatePathText(int mode)
    {
        switch(mode)
        {
            case 1:
                path1Text.text = "Cars on Path 1: " + traffic[0].ToString();
                break;
            case 2:
                path2Text.text = "Cars on Path 2: " + traffic[1].ToString();
                break;
            case 3:
                path3Text.text = "Cars on Path 3: " + traffic[2].ToString();
                break;
            case 4:
                path4Text.text = "Cars on Path 4: " + traffic[3].ToString();
                break;
        }
    }

    private void CalculateAverages(){
        allObjectsEver += totalTraffic * Time.deltaTime;
        for(int i = 0; i < 4; i++)
            allObjectsPath[i]  += traffic[i] * Time.deltaTime;

        avgTraffic = allObjectsEver / timeElapsed;
        for(int i = 0; i < 4; i++)
            avgTrafficPath[i] = allObjectsPath[i] / timeElapsed;

        avgTrafficText.text = "Avg Total Cars " + avgTraffic.ToString("F2");
        avgPath1Text.text = "P1 Avg Cars: "  + avgTrafficPath[0].ToString("F2");
        avgPath2Text.text = "P2 Avg Cars: " + avgTrafficPath[1].ToString("F2");
        avgPath3Text.text = "P3 Avg Cars: " + avgTrafficPath[2].ToString("F2");
        avgPath4Text.text = "P4 Avg Cars: " + avgTrafficPath[3].ToString("F2");

    }

    private void CalculateAvgCrossingTime(int mode, float cross) {
        allTimeToCross += cross;
        crossesSent++;
        
        crossesSentPath[mode-1]++;
        crossesPath[mode-1] += cross;

        avgTimeToCross = allTimeToCross/crossesSent;
        avgCrossingText.text = "Average time to cross: " + avgTimeToCross.ToString("F3");

          for(int i = 0; i < 4; i++)
            avgTimeToCrossPath[i] = crossesPath[i] / crossesSentPath[i];

        avgCrossing1Text.text = "P1 Avg Cross: "  + avgTimeToCrossPath[0].ToString("F2");
        avgCrossing2Text.text = "P2 Avg Cross: " + avgTimeToCrossPath[1].ToString("F2");
        avgCrossing3Text.text = "P3 Avg Cross: " + avgTimeToCrossPath[2].ToString("F2");
        avgCrossing4Text.text = "P4 Avg Cross: " + avgTimeToCrossPath[3].ToString("F2");
    }

    public float GetDynamicWeight() {
        return dynamicWeight;
    }

    public void SetDynamicWeight(float weight) {
        dynamicWeight = weight;
    }

    public void SetTime(float time) {
        timeToChange += time - saveTime;
        if (timeToChange < 0) timeToChange = 0;
        saveTime = time;
    }

    public float GetTime(){
        return saveTime;
    }

    public void SetTimeToSet(float time) {
        Debug.Log(saveTime * time);
        timeToSet = time;
    }

    public float GetTimeToSet() {
        return timeToSet;
    }
        public void SetTimeToCut(float time) {
        timeToCut = time;
    }

    public float GetTimeToCut() {
        return timeToCut;
    }

    public float[] GetAvgTrafficPath() {
        return avgTrafficPath;
    }

    public float GetTimeElapsed() {
        return timeElapsed;
    }

    public float GetPathAverage(int index) {
        return avgTrafficPath[index];
    }

    public float GetPathTimeToCross(int index) {
         return avgTimeToCrossPath[index];
    }

    public float GetTotalAvg() {
        return avgTraffic;
    }

    public float GetTotalTimeToCross() {
        return avgTimeToCross;
    }
}