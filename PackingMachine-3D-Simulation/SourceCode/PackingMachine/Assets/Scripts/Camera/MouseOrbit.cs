using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOrbit : MonoBehaviour {

    [Header("General")]
    public Transform target;
    new public Transform camera;

    [Header("Controls")]
    public string scrollAxis = "Mouse ScrollWheel";
    public string xAxis = "Mouse X";
    public string yAxis = "Mouse Y";
    public MouseButton mouseButton = 0;

    [Header("Position")]
    public float distance = 10;
    public float minDistance = 7;
    public float maxDistance = 15;

    [Header("Rotation limits")]
    public float minY = -50;
    public float maxY = 50;

    [Header("Sensitivity")]
    public float speed = 70;
    [Range(0, 1)]
    public float damping = 0.1f;

    float x, y;

    public enum MouseButton
    {
        LMB = 0,
        RMB = 1
    }

	// Use this for initialization
	void Start () {
        distance = Vector3.Distance(camera.position, target.position);
        x = camera.rotation.eulerAngles.y;
        y = camera.rotation.eulerAngles.x;
    }
	
	// Update is called once per frame
	void LateUpdate () {

        distance += -Input.GetAxis(scrollAxis) * distance;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        if (Input.GetMouseButton((int)mouseButton))
        {
            x += Input.GetAxis(xAxis) * speed;
            y -= Input.GetAxis(yAxis) * speed;
        }

        y = ClampAngle(y, minY, maxY);

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 position = rotation * new Vector3(0, 0, -distance) + target.position;

        camera.position = Vector3.Lerp(camera.position, position, 1 - damping);
        camera.rotation = Quaternion.Lerp(camera.rotation, rotation, 1 - damping);
    }

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}


