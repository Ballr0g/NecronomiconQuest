using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverSystem : Interactable
{
    public WallDirector[] gates;

    private float _cooldown = 0;

    protected override void Update()
    {
        if (_cooldown <= 0)
            base.Update();
        else
            _cooldown -= Time.deltaTime;
    }
    public override void Interact()
    {
        _cooldown = 2.1f;
        Playable.Play();
        foreach (WallDirector gate in gates)
        {
            gate.Play();
        }
    }
}
