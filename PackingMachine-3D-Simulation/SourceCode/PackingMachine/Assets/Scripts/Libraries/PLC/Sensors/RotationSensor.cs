using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSensor : PlcInput {

    public Transform target;
    public Vector3 rotationOffset; // Offset from initial local position when the sensor state is set to 1
    public float threshold = 0.5f; // [degree]
    public bool debugValue;
    
    Vector3 targetRotation;

    void Start()
    {
        targetRotation = target.localEulerAngles + rotationOffset;
    }

    void Update()
    {

        bool okX = Mathf.Abs(Mathf.DeltaAngle(target.localEulerAngles.x, targetRotation.x)) <= threshold;
        bool okY = Mathf.Abs(Mathf.DeltaAngle(target.localEulerAngles.y, targetRotation.y)) <= threshold;
        bool okZ = Mathf.Abs(Mathf.DeltaAngle(target.localEulerAngles.z, targetRotation.z)) <= threshold;

        bool value = okX && okY && okZ;

        //if(name == "PackMotorLeftRotSensor")
        //  Debug.Log(target.localEulerAngles.z +" ->" + targetRotation.z + " = " + Mathf.DeltaAngle(target.localEulerAngles.z, targetRotation.z));

        if (Value != value)
            Value = value;
        

        debugValue = Value;
    }
}
