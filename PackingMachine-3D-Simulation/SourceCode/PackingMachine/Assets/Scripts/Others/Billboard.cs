using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {

    Transform lookTarget;
    Vector3 rotOffset = new Vector3(90, 0, 0);

    void Start()
    {
        lookTarget = Camera.main.transform; 
    }

    void LateUpdate()
    {
        transform.LookAt(lookTarget);
        transform.Rotate(rotOffset, Space.Self);
    }
    
}
