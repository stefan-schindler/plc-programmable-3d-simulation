using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabActuator : PlcOutput {


    public float maxDistance = 0.1f; // [m]
    public Vector3 rayDirection = new Vector3(0, -1, 0);
    public string filterTag; // If empty no tag filtering is done

    bool isObjectGrabbed = false;
    Transform grabbedTransform;

    bool wasKinematic;
    Transform wasParent;

    public override bool Value
    {
        set
        {
            if (value != base.Value)
            {
                base.Value = value;

                if (value)
                {
                    // Try to Grab - if filterTags is not empty it goes up and grabs the topmost object in the parenting hierarchy with the filterTag

                    RaycastHit hitInfo = new RaycastHit();

                    if (debugByClick)
                        Debug.DrawLine(transform.position, transform.position + rayDirection.normalized * maxDistance, Color.cyan, 0.3f);

                    if (Physics.Raycast(transform.position, rayDirection, out hitInfo, maxDistance))
                    {
                        if (filterTag.Length == 0)
                        {
                            isObjectGrabbed = true;
                            grabbedTransform = hitInfo.transform;
                        }
                        else
                        {
                            grabbedTransform = hitInfo.transform;

                            Transform currentTransform = grabbedTransform;
                            while (currentTransform != null)
                            {
                                //Debug.Log(currentTransform.tag + " == " + filterTag);
                                if (currentTransform.tag.Equals(filterTag))
                                {
                                    grabbedTransform = currentTransform;
                                    isObjectGrabbed = true;
                                }

                                currentTransform = currentTransform.parent;
                            }
                        }

                        // Parent
                        if (isObjectGrabbed)
                        {
                            wasParent = grabbedTransform.parent;
                            grabbedTransform.parent = transform;

                            // Make the rigidbody kinematice if possible
                            if (grabbedTransform.GetComponent<Rigidbody>() != null)
                            {
                                wasKinematic = grabbedTransform.GetComponent<Rigidbody>().isKinematic;
                                grabbedTransform.GetComponent<Rigidbody>().isKinematic = true;
                            }
                        }
                    }
                }
                else if (isObjectGrabbed)
                {
                    isObjectGrabbed = false;

                    if (grabbedTransform.GetComponent<Rigidbody>() != null)
                        grabbedTransform.GetComponent<Rigidbody>().isKinematic = wasKinematic;

                    grabbedTransform.parent = wasParent;
                    grabbedTransform = null;
                }
            }

        }
    }

}
