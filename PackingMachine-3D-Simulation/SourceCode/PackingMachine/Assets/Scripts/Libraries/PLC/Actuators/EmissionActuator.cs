using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionActuator : PlcOutput {

    public GameObject target;
    public int materialIndex;
    public Color onEmissionColor = Color.yellow, offEmissionColor = Color.black;
    public GameObject[] halos;

    public override bool Value {
        set {
            base.Value = value;        
            foreach(GameObject halo in halos)
            {
                halo.SetActive(value);
            }
            target.GetComponent<MeshRenderer>().materials[materialIndex].SetColor("_EmissionColor", value ? onEmissionColor : offEmissionColor);
        }
    }

}
