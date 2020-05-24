using DungeonGame.AIEntities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Zombie_Trap_Manager : MonoBehaviour
{
    [SerializeField]
    private Zombie_AI _zombieGuy;
    [SerializeField]
    private PlayableDirector _doors;
    private bool hasTriggered = false;
    [SerializeField]
    private Zombie_Target[] targets;

    private PlayerHealth player;

    private void Start() => player = GameObject.Find("Player/PlayerObject").GetComponent<PlayerHealth>();

    void Update()
    {
        if (!hasTriggered && Zombie_Target.targetsHit == 6)
        {
            _zombieGuy.currentState = AIStates.Dead;
            _zombieGuy.OnDeadStateEnter();
            _doors.Play();
            hasTriggered = true;
            ForceDisableTargets();
            player.currentRespawnPosition = GameObject.Find("Player_SpawnPoints/Final_Spawnpoint").transform;
        }
    }

    public void ForceDisableTargets()
    {
        foreach (var target in targets)
        {
            target.ForceDisable();
        }
    }
}
