using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class FileBuilder : MonoBehaviour
{

    public static FileBuilder Instance { get; private set;}

    float[] generatorRatesInitial = new float[8];
    float weightInitial;
    float lightDurationInitial;
    float timeCheckInitial;
    float timeCutInitial;
    int counter;
    
    string initialString = "";
    string finalString = "";
    string resultsString = "";
    string changes = "";
    string folderPath = "Results";
    string counterFile = "counterxt.txt";
    
    
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

    private void Start() {
        Directory.CreateDirectory(folderPath);

        
        string counterPath = Path.Combine(folderPath, counterFile);
        if(File.Exists(counterPath)) {
            string counterVal = File.ReadAllText(counterPath);
            int.TryParse(counterVal, out counter);
        } else {
            File.WriteAllText(counterPath, counter.ToString());
        }
    }

    public string CollectValues(){
        generatorRatesInitial = Settings.Instance.GetRates();
        weightInitial = TrafficLight.Instance.GetDynamicWeight();
        lightDurationInitial = TrafficLight.Instance.GetTime();
        timeCheckInitial = TrafficLight.Instance.GetTimeToSet();
        timeCutInitial = TrafficLight.Instance.GetTimeToCut();


        string thisString = "";
        string initialFinal = UIScript.Instance.GetFirstStart() ? "Initial " : "Final ";

        thisString += initialFinal + "Dynamic Weight: " + weightInitial.ToString("F2") + '\n';
        thisString += initialFinal + "Light Duration: " + lightDurationInitial.ToString("F2") + '\n';
        thisString += initialFinal + "Time Check Coefficient: " + timeCheckInitial.ToString("F2") + '\n';
        thisString += initialFinal + "Initial Time to Cut Coefficient: " + timeCutInitial.ToString("f2") + '\n';

        thisString += initialFinal.ToUpper() + "CAR RATES \n\n";


        for (int i = 0; i < 8; i++) {
            thisString += "CP" + (i+1) + ((i % 2 == 0) ? "L" : "R") + ": " + generatorRatesInitial[i] + '\n';
        }

        if(UIScript.Instance.GetFirstStart())
            initialString = thisString;
        else
            finalString = thisString;

         return thisString;
    }

    public string GetAverages() {
        string thisString = "";

        thisString += "Time elapsed: " + TrafficLight.Instance.GetTimeElapsed().ToString("F4") + '\n';
        thisString += "Total average cars on intersection: " + TrafficLight.Instance.GetTotalAvg().ToString("F4") + '\n';
        thisString += "Total average time to cross: " + TrafficLight.Instance.GetTotalTimeToCross().ToString("F4") + '\n';
        for(int i = 0; i < 4; i++)
        thisString += $"Average cars on Path {i+1}: " + TrafficLight.Instance.GetPathAverage(i).ToString("F4") + '\n';
        for(int i = 0; i < 4; i++)
        thisString += $"Average time to cross on Path {i+1}: " + TrafficLight.Instance.GetPathTimeToCross(i).ToString("F4") + '\n';

        resultsString = thisString;
        return thisString;
    }

    public void AddChange(string change) {
        if(!change.Equals(""))
            changes += "Changed " + change +" at time " + TrafficLight.Instance.GetTimeElapsed().ToString("F2") + '\n';
    }

    public void LogResult() {
        CollectValues();
        GetAverages();
        string logFileName = $"result{counter}.txt";
        string resultFilePath = Path.Combine(folderPath, logFileName);

        string toLog = $"{resultsString}\n{initialString}\n{finalString}\n===CHANGES===\n{changes}";
        File.WriteAllText(resultFilePath, toLog);

        counter++;
        File.WriteAllText(Path.Combine(folderPath, counterFile), counter.ToString());
    }

    public string GetChanges() {
        return changes;
    }
}
