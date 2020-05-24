using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LowerCorridorTrap : MonoBehaviour
{
    public bool hasWorked = false;

    [SerializeField]
    private Transform newSpawnPoint;
    private WallDirector _trapGate;
    GameObject _roomSkeleton;
    Skeleton_AI _roomSkeletonAI;
    NavMeshAgent _skeletonAgent;
    private PlayerHealth player;
    void Start()
    {
        player = GameObject.Find("Player/PlayerObject").GetComponent<PlayerHealth>();
        _trapGate = GameObject.Find("Gates/Openable_Gate_05/gate_05").GetComponent<WallDirector>();
        _roomSkeleton = GameObject.Find("Lower_Corridor_Skeleton");
        _roomSkeletonAI = _roomSkeleton.GetComponent<Skeleton_AI>();
        _roomSkeleton.SetActive(false);
        _skeletonAgent = _roomSkeleton.GetComponent<NavMeshAgent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "PlayerObject" && !hasWorked)
        {
            _trapGate.Play();
            _roomSkeleton.SetActive(true);
            _roomSkeletonAI.ChasePlayer = true;
            _skeletonAgent.isStopped = false;
            player.SetSpawnPoint(newSpawnPoint);
            enabled = false;
            hasWorked = true;
        }
    }
}
