using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickOnTouch : MonoBehaviour {

    /** If Rigidbody collides with this collider it sets the rigidbody to kinematic state and parents it onto itself. */

    public bool onlyOnCollisionWithThisCollider = true;
    public string filterTag;

    void OnCollisionEnter(Collision collision)
    {
        if (!onlyOnCollisionWithThisCollider || collision.contacts[0].thisCollider == GetComponent<Collider>())
        {
            Rigidbody otherRigidbody = collision.collider.GetComponentInParent<Rigidbody>();
            if (otherRigidbody != null)
            {
                if (filterTag.Length == 0 || otherRigidbody.tag.Equals(filterTag))
                {
                    otherRigidbody.isKinematic = true;
                    otherRigidbody.transform.parent = transform;
                }
            }
        }
    }
}
