using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour {

    /** Spawns spawnedObject on spawnPoints which are maximum maxDistance far from this transform.position and have no children. */

    public int amount = 0; // Leave 0 for endless
    public GameObject spawnedObject;
    public Transform[] spawnPoints;
    public float maxDistance = 0.3f; // [m]
    public KeyCode debugKeyToRefill = KeyCode.None;

    int currentAmount = 0;
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(debugKeyToRefill))
            currentAmount = 0;

        foreach (Transform spawn in spawnPoints)
        {
            if(spawn.childCount == 0 && Vector3.Distance(spawn.position, transform.position) <= maxDistance)
            {
                if(amount == 0 || currentAmount++ < amount)
                    Instantiate(spawnedObject, spawn.position, spawn.rotation, spawn);
            }
        }
	}
}
