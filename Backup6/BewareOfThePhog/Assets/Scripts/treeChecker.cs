using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treeChecker : MonoBehaviour
{
    public jayhawkAI jayhawkAI;
    public Collider[] hitColliders;
   void Update()
    {
        jayhawkAI.inTree = false;
        hitColliders = Physics.OverlapSphere(transform.position, 0);

        foreach(Collider hitcollider in hitColliders)
        {
            if(hitcollider.gameObject.name == "TreeColliderCheck")
            {
                jayhawkAI.inTree = true;
            }
           
        }

    }


}
