using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : PlcInput {

    Coroutine animateCouroutine;

    public override bool Value
    {
        protected set
        {
            base.Value = value;
            if (animateCouroutine != null)
                StopCoroutine(animateCouroutine);
            animateCouroutine = StartCoroutine(Animate(value));
        }
    }

    IEnumerator Animate(bool pressed)
    {

        for (float p = transform.localPosition.y; p >= -0.5f && p <= 0; p += pressed ? -0.1f : 0.1f)
        {    
            Vector3 pos = transform.localPosition;
            pos.y = Mathf.Min(Mathf.Max(p, -0.5f), 0);
            transform.localPosition = pos;

            yield return new WaitForFixedUpdate();
        }
    }

    void OnMouseDown()
    {
        Value = !Value;
    }

}
