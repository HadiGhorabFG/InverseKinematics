using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKManager : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float samplingDistance = 1.0f;
    [SerializeField] private float learningRate = 0.1f;
    [SerializeField] private float distanceThreshold = 1.0f;
    
    public List<HandJoint> Joints = new List<HandJoint>();
    public float[] angles;

    
    
    private void Start()
    {
        angles = new float[Joints.Count];
        for (int i = 0; i < Joints.Count; i++)
        {
            angles[i] = Joints[i].LocalRot.y;
        }
        ForwardKinematic(angles);
    }

    private void Update()
    {
        InverseKinematics(target.transform.position, angles);
    }

    public void InverseKinematics(Vector3 target, float[] angles)
    {
        if (DistanceFromTarget(target, angles) < distanceThreshold)
            return;
        
        for (int i = Joints.Count - 1; i >= 0; i--)
        {
            // Gradient descent
            // Update : Solution -= LearningRate * Gradient
            float gradient = PartialGradient(target, angles, i);
            angles[i] -= learningRate * gradient;
            
            //angles[i] = Mathf.Clamp(angles[i], Joints[i].MinAngle, Joints[i].MaxAngle);
            Joints[i].transform.localRotation = Quaternion.Euler(0, angles[i], 0);

            // Early termination
            if (DistanceFromTarget(target, angles) < distanceThreshold) 
                return;
        }
    }

    public Vector3 ForwardKinematic(float[] angles)
    {
        // this function returns the position of the end factor in world space
        Vector3 prevPoint = Joints[0].transform.position;
        Quaternion rotation = Quaternion.identity;

        for (int i = 1; i < Joints.Count; i++)
        {
            rotation *= Quaternion.AngleAxis(angles[i - 1], Joints[i - 1].Axis);
            Vector3 nextPoint = prevPoint + rotation * Joints[i].StartOffset;

            prevPoint = nextPoint;
        }

        //Debug.Log(prevPoint);
        return prevPoint;
    }
 
    public float DistanceFromTarget(Vector3 target, float[] angles)
    {
        Vector3 point = ForwardKinematic(angles);
        return Vector3.Distance(target, point);
    }
    
    public float PartialGradient(Vector3 target, float[] angles, int i)
    {
        // Saves the angle,
        // it will be restored later
        float angle = angles[i];

        // Gradient : [F(x+SamplingDistance) - F(x)] / h
        float f_x = DistanceFromTarget(target, angles);

        angles[i] += samplingDistance;
        float f_x_plus_d = DistanceFromTarget(target, angles);

        float gradient = (f_x_plus_d - f_x) / samplingDistance;

        // Restores
        angles[i] = angle;

        return gradient;
    }
}