using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageBallCatcher : MonoBehaviour {

    public string filterTag;
	
	void OnTriggerEnter(Collider collider)
    {
        if(filterTag.Length == 0 || collider.tag.Equals(filterTag))
        {
            GetComponentInParent<Package>().AddBall(collider.transform);
        }
    }
}
