using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggTPSript : MonoBehaviour
{   
    public Transform SecondEGGPosition;
    private void OnTriggerEnter(Collider other)
    {
        transform.position = SecondEGGPosition.transform.position;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
