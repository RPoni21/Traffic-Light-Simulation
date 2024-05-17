using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLight : MonoBehaviour
{

    [SerializeField] private Material green;
    [SerializeField] private Material red;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.gameObject.layer == 9)
            this.GetComponent<MeshRenderer>().material = green;
        else
            this.GetComponent<MeshRenderer>().material = red;
    }

    
}
