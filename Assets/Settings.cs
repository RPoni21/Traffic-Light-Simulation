using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Generate[] generators; 

    public InputField[] inputs;

    public Button[] buttons;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 8; i++) {
            Debug.Log(generators[i]);
            int index = i;
            buttons[i].onClick.AddListener(() => ClickEvent(index));
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    void ClickEvent(int index)
    {
        if (float.TryParse(inputs[index].text, out float rate))
        {
            generators[index].CarRate(rate);
        }
        else
        {
            Debug.LogError("Invalid input. Please enter a valid number.");
        }
    }
}
}
