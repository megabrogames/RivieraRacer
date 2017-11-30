using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {
    Vector3 rotEuler;
    
    // Use this for initialization
    void Start()
    {
        rotEuler = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        // NOTE: Some of the update code has been put in CarUserControl.Update because of this namespace shit!
        
        transform.eulerAngles = new Vector3(rotEuler.x, rotEuler.y, rotEuler.z);
    }

}
