using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _lifeSpan = 4.0f;

    private float remainingLife;
    void Start()
    {
        remainingLife = _lifeSpan;
    }

    void FixedUpdate()
    {
        if (remainingLife > 0)
        {
            remainingLife -= Time.fixedDeltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
