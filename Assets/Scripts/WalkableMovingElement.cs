using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkableMovingElement : MonoBehaviour
{
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.name == "PlayerObject")
        {
            other.transform.parent.parent = transform;
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.name == "PlayerObject")
        {
            other.transform.parent.parent = null;
        }
    }
}
