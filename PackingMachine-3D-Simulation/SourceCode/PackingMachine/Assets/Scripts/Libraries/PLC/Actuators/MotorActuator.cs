using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorActuator : PlcOutput {


    public Vector3 angularSpeed; // [degree per second]
   
    
    void Update()
    {
        if (Value)
        {
            transform.localEulerAngles += angularSpeed * Time.deltaTime;
        }
    }

    

}
