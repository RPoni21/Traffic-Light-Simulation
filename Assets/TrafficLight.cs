using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    // Start is called before the first frame update
    float timeToChange = 15;
    List<GameObject> lights = new List<GameObject>();
    float saveTime;
    int track = 0;


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

    }