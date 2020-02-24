using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyMotorActuator : PlcOutput {


    public Vector3 angularSpeed; // [degree per second   
    
    void FixedUpdate()
    {
        if (Value)
        {
            Quaternion newRotation = transform.rotation * Quaternion.Euler(angularSpeed * Time.fixedDeltaTime); 
            GetComponent<Rigidbody>().MoveRotation(newRotation);
        }
    }

}
