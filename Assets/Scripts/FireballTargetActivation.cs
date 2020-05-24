using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FireballTargetActivation : MonoBehaviour
{
    private bool activated = false;
    private PlayableDirector animatedBridge;
    void Start()
    {
        animatedBridge = GameObject.Find("Animated_Bridge/Bridge_Over_The_Lava").GetComponent<PlayableDirector>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!activated && other.name.Contains("PlayerFireBall"))
        {
            activated = true;
            animatedBridge.Play();
        }
    }
}
