using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastSensor : PlcInput {

    public Transform origin, receiver;

    public bool debugValue;

    float distance;
    Vector3 direction;
    
    // Use this for initialization
    void Start () {
        distance = Vector3.Distance(origin.position, receiver.position);
        direction = (receiver.position - origin.position).normalized;
	}
	
	// Update is called once per frame
	void Update () {
        bool value = Physics.Raycast(origin.position, direction, distance);

        if (value != Value)
            Value = value;

        debugValue = value;
	}
}
