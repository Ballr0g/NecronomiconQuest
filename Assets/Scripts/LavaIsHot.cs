using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaIsHot : MonoBehaviour
{
    private PlayerHealth _playerObject;
    void Start()
    {
        _playerObject = GameObject.Find("Player/PlayerObject").GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "PlayerObject")
        {
            _playerObject.PlayerDeath(deathMessage: "You tried to swim in lava... Big oof");
        }
    }
}