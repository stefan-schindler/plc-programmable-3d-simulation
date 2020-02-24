using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyParent : MonoBehaviour {

    public Transform parent;

    Vector3 positionOffset;
    Quaternion rotationOffset;
    new Rigidbody rigidbody;

	// Use this for initialization
	void Start () {
        positionOffset = transform.position - parent.position;
        //rotationOffset = transform.rotation.  parent.rotation;
        rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        rigidbody.MovePosition(parent.position + positionOffset);
	}
}
