using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKManager2 : MonoBehaviour
{
    public HandJoint2 root;
    public HandJoint2 end;

    public GameObject target;

    public float threshold = 0.05f;
    public float rate = 10.0f;

    private void Update()
    {
        if (GetDistance(end.transform.position, target.transform.position) > threshold)
        {
            HandJoint2 current = root;

            while (current != null)
            {
                float slope = CalculateSlope(current);
                current.Rotate(slope * rate);
                current = current.GetChild();
            }
        }
    }

    float CalculateSlope(HandJoint2 joint)
    {
        float deltaTheta = 0.01f;
        float distanceOne = GetDistance(end.transform.position, target.transform.position);
        
        joint.Rotate(deltaTheta);
        
        float distanceTwo = GetDistance(end.transform.position, target.transform.position);
        
        joint.Rotate(-deltaTheta);

        return (distanceOne - distanceTwo) / deltaTheta;
    }
    
    float GetDistance(Vector3 pointOne, Vector3 pointTwo)
    {
        return Vector3.Distance(pointOne, pointTwo);
    }
}
