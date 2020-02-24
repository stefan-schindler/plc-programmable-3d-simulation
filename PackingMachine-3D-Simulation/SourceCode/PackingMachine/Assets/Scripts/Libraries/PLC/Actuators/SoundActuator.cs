using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundActuator : PlcOutput {

    public bool debugValue;

	public override bool Value
    {
        set
        {
            if(value != Value)
            {
                base.Value = value;
                if(value)
                    GetComponent<AudioSource>().Play();
                else
                    GetComponent<AudioSource>().Stop();
            }
        }
    }

    void Update()
    {
        debugValue = Value;
    }
}


