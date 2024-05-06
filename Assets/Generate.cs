using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour
{
    [SerializeField] private GameObject carType;
    void Start()
    {
        StartCoroutine(generate());
    }

    IEnumerator generate() {
        Instantiate(carType, new Vector3(0,0,0), Quaternion.identity);
        
        yield return new WaitForSecondsRealtime(3);

        StartCoroutine(generate());
    }
}
