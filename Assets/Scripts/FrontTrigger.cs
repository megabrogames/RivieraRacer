using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontTrigger : MonoBehaviour {
    [HideInInspector] public bool isFinished = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FinishLine")
        {
            Debug.Log("Finish collision detected");
            isFinished = true;
        }
    }
}
