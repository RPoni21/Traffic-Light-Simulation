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
    public Text avgCrossingText;
    public Text greenLightText;

    private float timeElapsed = 0;
    private float avgTimeToCross = 0;
    private float allTimeToCross = 0;
    private float crossesSent = 1;
    private float avgTraffic;   
    private float allObjectsEver = 0;

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
        calculateAverage();
        greenLightText.text = "Green light remaining: " + timeToChange; 

    }

    void basicSystem(){
        if (timeToChange >= saveTime / 1.5f) {
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
            Debug.Log("Current: " + currentTrack);
            Debug.Log("ToChange: " + toChange);
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
            Debug.Log("Time out");
            
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

        public void DecrementTraffic(int mode, float cross) {
        if(mode > 0 && mode < 5){
        traffic[mode-1]--;
        totalTraffic--;
        calculateAvgCrossingTime(cross);
        UpdatePathText(mode);
        UpdateTrafficText();
        }
    }
    private void HandleTraffic()
    {
        
        for (int i = 0; i < 4; i++)
        {
            if (i > 3) i = 0;
            if (setToChange && timeToChange <= (saveTime / 2f) && ((float)traffic[currentTrack] < (float)traffic[(i + currentTrack) % 4] * 0.75f))
            {

                Debug.Log("Changing to track " + (i + 1));
                Debug.Log("Traffic in current: " + traffic[i]);

                timeToChange /= 2;

                toChange = (i + currentTrack) % 4;
                setToChange = false;

            }
        }
    }

    private void flip(int red, int green)
    {
        lights[red].gameObject.layer = 8;
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

    private void calculateAverage(){
        allObjectsEver += totalTraffic * Time.deltaTime;
        avgTraffic = allObjectsEver / timeElapsed;
        avgTrafficText.text = "Average total cars " + avgTraffic;
    }

    private void calculateAvgCrossingTime(float cross) {
        allTimeToCross += cross;
        crossesSent++;
        avgTimeToCross = allTimeToCross/crossesSent;
        avgCrossingText.text = "Average time to cross: " + avgTimeToCross;
    }

}