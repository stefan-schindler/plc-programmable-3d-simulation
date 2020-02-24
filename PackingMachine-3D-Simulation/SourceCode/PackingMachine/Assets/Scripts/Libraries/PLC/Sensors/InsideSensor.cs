using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideSensor : PlcInput {

    public bool debugValue;
    void Update()
    {
        debugValue = Value;
    }

    void OnTriggerEnter(Collider col)
    {
        Value = true;
    }

    void OnTriggerExit(Collider col)
    {
        Value = false;
    }
}
