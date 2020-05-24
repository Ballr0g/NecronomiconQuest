using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DungeonGame.AIEntities;
using UnityEngine.AI;

public class ZombieRoomTrap : MonoBehaviour
{
    public bool hasWorked = false;
    [SerializeField]
    private WallDirector _trapGate;
    GameObject _roomZombie;

    // Start is called before the first frame update
    void Start()
    {
        _roomZombie = GameObject.Find("Mobs/Trap_Zombie");
        _roomZombie.SetActive(false);
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (!hasWorked && other.name == "PlayerObject")
        {
            _roomZombie.SetActive(true);
            _trapGate.Play();
            hasWorked = true;
        }
    }

    public void ForceEnable() => hasWorked = false;
}
