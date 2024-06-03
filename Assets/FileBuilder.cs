using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FileBuilder : MonoBehaviour
{
    // Start is called before the first frame update

    public static FileBuilder Instance { get; private set;}

    float[] generatorRatesInitial = new float[8];
    float weightInitial;
    float lightDurationInitial;
    float timeCheckInitial;
    float timeCutInitial;

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
    
    public void PrintThis() {
        Debug.Log("Printed!");
    }

    public void CollectInitial(){
        generatorRatesInitial = Settings.Instance.GetRates();
        weightInitial = TrafficLight.Instance.GetDynamicWeight();
        lightDurationInitial = TrafficLight.Instance.GetTime();
        timeCheckInitial = TrafficLight.Instance.GetTimeToSet();
        timeCutInitial = TrafficLight.Instance.GetTimeToCut();

        string initialString = "";

        initialString += "Initial Dynamic Weight: " + weightInitial.ToString("F2") + '\n';
        initialString += "Initial Light Duration: " + lightDurationInitial.ToString("F2") + '\n';
        initialString += "Initial Time Check Coefficient: " + timeCheckInitial.ToString("F2") + '\n';
        initialString += "Initial Time to Cut Coefficient: " + timeCutInitial.ToString("f2") + '\n';

        initialString += "INITIAL CAR RATES \n\n";

        for (int i = 0; i < 8; i++) {
            initialString += "CP" + (i+1) + ((i % 2 == 0) ? "L" : "R") + ": " + generatorRatesInitial[i] + '\n';
        }

         Debug.Log(initialString);
    }
}
