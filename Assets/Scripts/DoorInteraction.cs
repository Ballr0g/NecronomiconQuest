using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DoorInteraction : Interactable
{
    public bool hasKey = false;
    public string openedText;

    private PlayableDirector doorDirector;

    protected override void Start()
    {
        base.Start();
        doorDirector = GameObject.Find("Animated_Door_To_Lava/Door").GetComponent<PlayableDirector>();
    }

    protected override void Update()
    {
        if (hasKey)
            base.Update();
    }

    public override void Interact()
    {
        doorDirector.Play();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.name == "PlayerObject")
        {
            if (_canInteractAgain)
            {
                if (!hasKey)
                    _textUI.SetText(text);
                else
                    _textUI.SetText(openedText);
            }
            _isPlayerClose = true;
        }
    }
}
