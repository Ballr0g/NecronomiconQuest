using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpawnpoint : MonoBehaviour
{
    [SerializeField]
    private Transform _spawnPoint;
    private PlayerHealth playerHandle;

    private void Start()
    {
        playerHandle = GameObject.Find("Player/PlayerObject").GetComponent<PlayerHealth>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "PlayerObject")
        {
            playerHandle.SetSpawnPoint(_spawnPoint);
            enabled = false;
        }
    }
}
