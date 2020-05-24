using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector), typeof(Animator))]
public class WallDirector : MonoBehaviour
{
    public bool isOpen = false;
    private PlayableDirector attachedDirector;
    public PlayableAsset openAnimation, closeAnimation;

    private void Start()
    {
        attachedDirector = GetComponent<PlayableDirector>();
        if (isOpen)
            transform.position = new Vector3(transform.position.x, 3f, transform.position.z);
    }

    public void Play()
    {
        if (isOpen)
            attachedDirector.Play(closeAnimation);
        else
            attachedDirector.Play(openAnimation);
        isOpen = !isOpen;
    }

    public void TechForceOpen()
    {
        transform.position = new Vector3(transform.position.x, 3f, transform.position.z);
        isOpen = true;
    }
}

