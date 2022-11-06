using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandJoint : MonoBehaviour
{
    public Vector3 Axis;
    public Vector3 StartOffset;
    public Vector3 LocalRot;
    
    public float MinAngle;
    public float MaxAngle;

    private void Awake()
    {
        StartOffset = transform.localPosition;
        LocalRot = transform.localRotation.eulerAngles;
    }

    private void Update()
    {
    }
}
