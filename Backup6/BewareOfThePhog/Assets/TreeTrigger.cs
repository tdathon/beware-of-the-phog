using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeTrigger : MonoBehaviour
{
    public bool InTree = false;
    void Start()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        InTree = true;
       // Debug.Log("IN TREE");
    }
    private void OnTriggerExit(Collider other)
    {
        InTree = false;
        //Debug.Log("Left TREE");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
