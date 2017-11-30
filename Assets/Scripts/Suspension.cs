using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspension : MonoBehaviour
{
    public GameObject wheel; // The wheel that the script needs to referencing to get the postion for the suspension


    private Vector3 m_TargetOriginalPosition;
    private Vector3 m_Origin;


    private void Start()
    {
        m_TargetOriginalPosition = wheel.transform.localPosition;
        m_Origin = transform.localPosition;
    }


    private void Update()
    {
        transform.localPosition = m_Origin + (wheel.transform.localPosition - m_TargetOriginalPosition);
    }
}
