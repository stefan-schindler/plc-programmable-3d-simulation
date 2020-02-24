using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageCap : MonoBehaviour {

    Package enclosedPackage;
    Vector3 lastPosition;

    bool _isPreparedToEnclose;
    bool IsPreparedToEnclose
    {
        get {
            return _isPreparedToEnclose;
        }
        set
        {
            _isPreparedToEnclose = value;
            SetTrigger(value);
        }
    }

    void Start()
    {
        lastPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (!enclosedPackage)
        {
            if (GetComponent<Rigidbody>().isKinematic && !IsPreparedToEnclose) //  && lastPosition == transform.position
            {
                IsPreparedToEnclose = true;
            }
            else if (IsPreparedToEnclose)
            {
                IsPreparedToEnclose = false;
            }
            lastPosition = transform.position;
        }
    }

    void OnTriggerStay(Collider collider)
    {
        Package package = collider.GetComponentInParent<Package>();
        if (!enclosedPackage && IsPreparedToEnclose && package != null)
        {
            package.EnclosePackage(this);
            enclosedPackage = package;
            SetTrigger(false);
        }
    }

    void SetTrigger(bool isTrigger)
    {
        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            if(!collider.tag.Equals("TriggerOnly"))
                collider.isTrigger = isTrigger;
        }
    }

}
