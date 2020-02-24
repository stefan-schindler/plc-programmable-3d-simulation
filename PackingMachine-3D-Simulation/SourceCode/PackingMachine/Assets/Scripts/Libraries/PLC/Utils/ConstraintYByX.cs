using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstraintYByX : MonoBehaviour {

    public float xLimit;
    public bool positiveDirection;

    public float yLimit;

    float localOffsetY;

    void Start()
    {
        localOffsetY = transform.localPosition.z;
    }
	
	// Update is called once per frame
	void LateUpdate () {

        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, localOffsetY);

        if (positiveDirection && transform.position.x >= xLimit || !positiveDirection && transform.position.x <= xLimit)
        {
            if (transform.position.y < yLimit)
                transform.position = new Vector3(transform.position.x, yLimit, transform.position.z);
        }
	}
}
