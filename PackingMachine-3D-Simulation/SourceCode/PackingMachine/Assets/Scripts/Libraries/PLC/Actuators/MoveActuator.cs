using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveActuator : PlcOutput {


    public Vector3 targetPositionOffset;
    public Vector3 targetRotationOffset;
    public float animationDuration; // [s]

    Vector3 initialPosition;
    Vector3 initialRotation;

    Coroutine positionCouroutine, rotationCoroutine;

    public override bool Value {
        set {
            if (value != base.Value)
            {
                base.Value = value;

                // Position
                if (targetPositionOffset != Vector3.zero)
                {
                    if (positionCouroutine != null)
                        StopCoroutine(positionCouroutine);
                    positionCouroutine = StartCoroutine(AnimatePosition(value));
                }

                // Rotation
                if (targetRotationOffset != Vector3.zero)
                {
                    if (rotationCoroutine != null)
                        StopCoroutine(rotationCoroutine);
                    rotationCoroutine = StartCoroutine(AnimateRotation(value));
                }
            }
        }
    }

    void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localEulerAngles;
    }


    IEnumerator AnimatePosition(bool active)
    {

        Vector3 startPosition = transform.localPosition;
        Vector3 destinationPosition = active ? initialPosition + targetPositionOffset : initialPosition;

        float fullDistance = targetPositionOffset.magnitude;

        Vector3 moveDirection = destinationPosition - startPosition;
        float currentDistance = moveDirection.magnitude;
        moveDirection.Normalize();
        
        float duration = currentDistance / fullDistance * animationDuration;
        float passedTime = 0;
        

        while (true)
        {
            bool shouldBreak = false;
            passedTime += Time.deltaTime;
            float time = passedTime / duration;

            if (time > 1) { 
                time = 1;
                shouldBreak = true;
            }

            transform.localPosition = startPosition + moveDirection * time * currentDistance;

            if (shouldBreak)
                break;

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator AnimateRotation(bool active)
    {

        Vector3 startRotation = transform.localEulerAngles;
        Vector3 destinationRotation = active ? initialRotation + targetRotationOffset : initialRotation;

        float fullDistance = targetRotationOffset.magnitude;

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
