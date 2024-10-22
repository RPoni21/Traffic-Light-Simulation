using System;
using System.Collections;
using UnityEngine;

public class Generate : MonoBehaviour
{
    [SerializeField] private float carRate;
    [SerializeField] private GameObject carType;
    float tempRate;
    bool generated = false;
    
    
    void Start()
    {
        if(carRate != 0){
        tempRate = carRate;
        }
    }

    void Update() {
        if (carRate != 0) {
        if(!UIScript.Instance.GetFirstStart() && !generated) {
            Instantiate(carType, new Vector3(0,0,0), Quaternion.identity);
            generated = true;
            }

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
        tempRate += rate - carRate;
        if(tempRate < 0){tempRate = 0;}
    }

    public float GetCarRate(){
        return carRate;
    }
}
