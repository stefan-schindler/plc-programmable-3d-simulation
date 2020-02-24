using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Package : MonoBehaviour {

    private HashSet<Transform> ballsInside = new HashSet<Transform>();
    private PackageCap cap;

    /** Makes this package child of the cap and removes the rigidbody component. Makes ballsInside child of this package and 
     * removes the rigidbody components of them. */
    public void EnclosePackage(PackageCap packageCap)
    {
        this.cap = packageCap;

        // Make static
        Destroy(GetComponent<Rigidbody>());

        // Make inside balls children
        foreach(Transform ball in ballsInside)
        {
            if(ball.GetComponent<Rigidbody>() != null)
                Destroy(ball.GetComponent<Rigidbody>());
            ball.transform.parent = transform;
        }

        // Parent and reposition
        transform.parent = packageCap.transform;
        //transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -1.444244f);
        StartCoroutine(AnimateToLocalPosition(new Vector3(0, 0, -1.444244f)));

        // Freeze rotation of parent cap
        packageCap.GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
    }


    public void AddBall(Transform ball)
    {
        ballsInside.Add(ball);
    }

    IEnumerator AnimateToLocalPosition(Vector3 targetLocalPosition)
    {
        float maxDistancePerSecond= 1f;

        Vector3 add = targetLocalPosition - transform.localPosition;
        add.Normalize();
        add *= maxDistancePerSecond;

        float lastDistance = float.PositiveInfinity;

        while (true)
        {
            Vector3 newLocalPositon = transform.localPosition + add * Time.deltaTime;

            float distance = Vector3.Distance(newLocalPositon, targetLocalPosition);

            if (distance > lastDistance)
            {
                transform.localPosition = targetLocalPosition;
                break;
            }
            transform.localPosition = newLocalPositon;
            lastDistance = distance;

            yield return new WaitForEndOfFrame();
        }
    }

}
