using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationsSensor : PlcInput
{

    public Transform target;
    public Vector3[] rotationOffsets; // Offset from initial local position when the sensor state is set to 1
    public float threshold = 0.5f; // [degree]
    public bool debugValue;
    public bool checkX = true, checkY = true, checkZ = true;

    Vector3[] targetRotations;
    Vector3 lastRotation;

    int count;
    void Start()
    {
        count = rotationOffsets.Length;
        targetRotations = new Vector3[count];
        lastRotation = transform.localEulerAngles;
        for(int i=0; i<count; i++)
            targetRotations[i] = target.localEulerAngles + rotationOffsets[i];
    }

    void Update()
    {
        bool value = false;
        for (int i = 0; i < count; i++)
        {
            // Check if is currently in threshold range
            bool okX = Mathf.Abs(Mathf.DeltaAngle(target.localEulerAngles.x, targetRotations[i].x)) <= threshold;
            bool okY = Mathf.Abs(Mathf.DeltaAngle(target.localEulerAngles.y, targetRotations[i].y)) <= threshold;
            bool okZ = Mathf.Abs(Mathf.DeltaAngle(target.localEulerAngles.z, targetRotations[i].z)) <= threshold;

            value = (!checkX || okX) && (!checkY || okY) && (!checkZ || okZ);
            if (value)
                break;

            // Check if haven't passed by
            float deltaX = Mathf.DeltaAngle(target.localEulerAngles.x, targetRotations[i].x);
            float lastDeltaX = Mathf.DeltaAngle(lastRotation.x, targetRotations[i].x);
            okX = Mathf.Abs(deltaX)  <= 170 && Mathf.Abs(lastDeltaX) <= 170 && deltaX * lastDeltaX <= 0;

            float deltaY = Mathf.DeltaAngle(target.localEulerAngles.y, targetRotations[i].y);
            float lastDeltaY = Mathf.DeltaAngle(lastRotation.y, targetRotations[i].y);
            okY = Mathf.Abs(deltaY) <= 170 && Mathf.Abs(lastDeltaY) <= 170 && deltaY * lastDeltaY <= 0;

            float deltaZ = Mathf.DeltaAngle(target.localEulerAngles.z, targetRotations[i].z);
            float lastDeltaZ = Mathf.DeltaAngle(lastRotation.z, targetRotations[i].z);
            okZ = Mathf.Abs(deltaZ) <= 170 && Mathf.Abs(lastDeltaZ) <= 170 && deltaZ * lastDeltaZ <= 0;

            value = (!checkX || okX) && (!checkY || okY) && (!checkZ || okZ);
            if (value)
                break;
            
        }

        lastRotation = transform.localEulerAngles;

        if (Value != value)
            Value = value;
        
        debugValue = Value;
    }
}
