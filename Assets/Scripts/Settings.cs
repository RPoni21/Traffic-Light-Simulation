using System.Collections;
using System.Collections.Generic;
using System.Data;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public static Settings Instance { get; private set;}
    public Generate[] generators; 

    public InputField[] inputs;

    public Button[] buttons;

    Dictionary<int, string> changes = new Dictionary<int, string>();

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

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 8; i++) {
            int index = i;
            buttons[i].onClick.AddListener(() => ClickEvent(index));
        
    }
    }
    
    void ClickEvent(int index)
    {
        if (float.TryParse(inputs[index].text, out float rate))
        {
            if (rate != generators[index].GetCarRate()) {
                changes[index] = "spawn rate of " + generators[index].gameObject.name + " from " + generators[index].GetCarRate() + " to " + rate; 
            }
            generators[index].CarRate(rate);
        }
        else
        {
            Debug.LogError("Invalid input. Please enter a valid number.");
        }
    }

    public float[] GetRates(){
        float[] toReturn = new float[generators.Length];
        for (int i = 0; i < 8; i++){
            toReturn[i] = generators[i].GetCarRate();
        }

        return toReturn;
    }

    public Dictionary<int, string> GetChanges() {
        return changes;
    }

    public void ClearChanges() {
        changes.Clear();
    }
}

