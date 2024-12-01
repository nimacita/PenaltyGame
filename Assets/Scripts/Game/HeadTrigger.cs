using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadTrigger : MonoBehaviour
{

    public UnityEngine.Events.UnityEvent HeadDamageMethod;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            if (HeadDamageMethod != null)
            {
                HeadDamageMethod.Invoke();
            }
        }
    }

}
