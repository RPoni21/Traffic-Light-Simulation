using System;
using System.Collections;
using UnityEngine;

public class Generate : MonoBehaviour
{
    [SerializeField] private float carRate;
    [SerializeField] private GameObject carType;
    float tempRate;
    
    
    void Start()
    {
        if(carRate != 0){
        tempRate = carRate;
            Instantiate(carType, new Vector3(0,0,0), Quaternion.identity);
        }
    }

    void Update() {
        if (carRate != 0) {
        if (tempRate > 0) {
            tempRate -= Time.deltaTime;
        } else {
            Instantiate(carType, new Vector3(0,0,0), Quaternion.identity);
            tempRate = carRate;
        }
        }
    }

    public void CarRate(float rate){
        carRate = rate;
        tempRate += Math.Abs(carRate - rate);
    }

    public float GetCarRate(){
        return carRate;
    }
}
