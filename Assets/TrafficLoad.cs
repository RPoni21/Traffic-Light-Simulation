using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLoad : MonoBehaviour
{
    
    [SerializeField] private int mode;
    float timeToChange = 1;
    int load = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timeToChange >= 0) {
            timeToChange -= Time.deltaTime;
        } else {
            timeToChange = 1;
        }
    }

    public void Increment(){ 
        load++;
        TrafficLight.Instance.IncrementTraffic(mode);
    }

    public void Decrement() {
        load--;
        TrafficLight.Instance.DecrementTraffic(mode);
    }

    // void OnTriggerEnter(Collider other)
    // {
    //     load++;
    //     Debug.Log("Path collided");
    // }
}
