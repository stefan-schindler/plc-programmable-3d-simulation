using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabHands : MonoBehaviour {

    public PlcOutput output;
    public Vector3 rotationOffset;
    public float animationDuration;

    Vector3 initialRotation;
    bool lastOutputState;
    Coroutine animationCoroutine;

    // Use this for initialization
    void Start () {
        initialRotation = transform.localEulerAngles;
        lastOutputState = output.Value;
	}
	
	// Update is called once per frame
	void Update () {
        bool currentState = output.Value;
		if(lastOutputState != currentState)
        {
            if (animationCoroutine != null)
                StopCoroutine(animationCoroutine);
            animationCoroutine = StartCoroutine(AnimateRotation(currentState));
            lastOutputState = currentState;
        }
	}

    IEnumerator AnimateRotation(bool active)
    {

        Vector3 startRotation = transform.localEulerAngles;
        Vector3 destinationRotation = active ? initialRotation + rotationOffset : initialRotation;

        float fullDistance = rotationOffset.magnitude;

        Vector3 moveDirection = destinationRotation - startRotation;

        // Find what direction is shorter to reach the target angle -> CW or CCW
        float newAngle = moveDirection.x > 0 ? 360 - moveDirection.x : 360 + moveDirection.x;
        if (Mathf.Abs(newAngle) < Mathf.Abs(moveDirection.x))
            moveDirection.x = newAngle * -Mathf.Sign(moveDirection.x);

        newAngle = moveDirection.y > 0 ? 360 - moveDirection.y : 360 + moveDirection.y;
        if (Mathf.Abs(newAngle) < Mathf.Abs(moveDirection.y))
            moveDirection.y = newAngle * -Mathf.Sign(moveDirection.y);

        newAngle = moveDirection.z > 0 ? 360 - moveDirection.z : 360 + moveDirection.z;
        if (Mathf.Abs(newAngle) < Mathf.Abs(moveDirection.z))
            moveDirection.z = newAngle * -Mathf.Sign(moveDirection.z);

        float currentDistance = moveDirection.magnitude;

        moveDirection.Normalize();

        float duration = currentDistance / fullDistance * animationDuration;
        float passedTime = 0;


        while (true)
        {
            bool shouldBreak = false;
            passedTime += Time.deltaTime;
            float time = passedTime / duration;

            if (time > 1)
            {
                time = 1;
                shouldBreak = true;
            }

            transform.localEulerAngles = startRotation + moveDirection * time * currentDistance;

            if (shouldBreak)
                break;

            yield return new WaitForEndOfFrame();
        }
    }
}
