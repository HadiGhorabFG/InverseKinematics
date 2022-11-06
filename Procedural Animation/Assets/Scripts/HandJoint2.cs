using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandJoint2 : MonoBehaviour
{
    public HandJoint2 child;

    public HandJoint2 GetChild()
    {
        return child;
    }

    public void Rotate(float angle)
    {
        transform.Rotate(Vector3.up * angle);
    }
}
