using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlcProgram : MonoBehaviour {

    protected abstract void ProgramCycle();

    private PlcConnection plc;
    private float startupDelay = 0.5f;

    protected bool GetInput(int index)
    {
        return plc.inputs[index].Value;
    }

    protected bool GetOutput(int index)
    {
        return plc.outputs[index].Value;
    }

    protected void SetOutput(int index, bool value)
    {
        plc.outputs[index].Value = value;
    }


    // Use this for initialization
    void Start () {
        plc = GetComponent<PlcConnection>();
	}

    // Update is called once per frame
    float passedTime;
    float startTime = -1;
    bool isRunning = false;
	void Update () {
        if (!isRunning)
        {
            if (startTime == -1)
                startTime = Time.time;
            passedTime = Time.time - startTime;

            if (passedTime < startupDelay)
                return;
            else
                isRunning = true;
        }

        ProgramCycle();	
	}
}
