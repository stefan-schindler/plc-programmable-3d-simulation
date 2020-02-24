using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionSensor : PlcInput {

    public Transform target;
    public Vector3 positionOffset; // Offset from initial local position when the sensor state is set to 1
    public float threshold = 0.01f; // [m]
    public bool debugValue;

    Vector3 targetPosition;

    void Start()
    {
        targetPosition = target.localPosition + positionOffset;
    }

    void Update()
    {
        
        if(name == "PackagePistonEndPosSensor")
        {
            //Debug.Log(Vector3.Distance(target.localPosition, targetPosition));
        }

        if (Vector3.Distance(target.localPosition, targetPosition) <= threshold)
        {
            if(!Value) Value = true;
        }
        else
        {
            if (Value) Value = false;
        }

        debugValue = Value;
    }


}
