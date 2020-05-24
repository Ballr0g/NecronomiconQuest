using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapActivationLever : Interactable
{
    [SerializeField]
    private GameObject[] _templeBlades;
    public override void Interact()
    {
        Playable.Play();
        foreach (GameObject blade in _templeBlades)
        {
            blade.SetActive(true);
        }
    }

    public void TechDeactivation()
    {
        _canInteractAgain = true;
        foreach (GameObject blade in _templeBlades)
        {
            blade.SetActive(false);
        }
    }
}
