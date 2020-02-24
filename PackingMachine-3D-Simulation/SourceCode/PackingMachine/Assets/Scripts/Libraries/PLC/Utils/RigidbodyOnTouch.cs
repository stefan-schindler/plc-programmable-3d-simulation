using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyOnTouch : MonoBehaviour {

    /** I kinematic rigidbody collides with this, the isKinematic is set to false of the object that collided. */

    public bool removeParent = true; // If true the parent of the rigidbody is set to null.

	void OnTriggerEnter(Collider other)
    {
        Rigidbody otherRigidbody = other.GetComponent<Rigidbody>();
        if (otherRigidbody != null)
        {
            otherRigidbody.isKinematic = false;
            if(removeParent)
                otherRigidbody.transform.parent = null;
        }
    }
}
