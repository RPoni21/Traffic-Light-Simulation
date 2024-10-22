using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLoad : MonoBehaviour
{
    
    [SerializeField] private int mode;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Increment(){ 
        TrafficLight.Instance.IncrementTraffic(mode);
    }

    public void Decrement(float cross) {
        TrafficLight.Instance.DecrementTraffic(mode, cross);
    }
}
