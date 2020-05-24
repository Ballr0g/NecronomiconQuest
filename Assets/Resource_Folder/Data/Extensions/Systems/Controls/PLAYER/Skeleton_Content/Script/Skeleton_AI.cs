using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.Playables;

public class Skeleton_AI : MonoBehaviour
{
    public float Speed;
    public bool YouDied;
    public bool ChasePlayer;

    private NavMeshAgent _agent;
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private Animator _anim;
    [SerializeField]
    private float _attackGrace = 0.7f;

    private float attackCountdown;
    private bool attacked = false;
    private bool nearPlayer = false;
    private PlayerHealth playerKill;
    private LowerCorridorTrap trapHandle;
    private Transform respawnPoint;
    private WallDirector gate;
    private TrapActivationLever trapLever;
    private TextMeshProUGUI uiText;
    private Coroutine textTipRoutine;
    private PlayableDirector treasureRoomDirector;

    void Start()
    {
        uiText = GameObject.Find("Canvas/UI_Instructions").GetComponent<TextMeshProUGUI>();
        respawnPoint = GameObject.Find("Mobs/Corridor_Skeleton_Start").transform;
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = Speed;
        _anim = transform.Find("Skeleton_01_Rigged").GetComponent<Animator>();
        attackCountdown = _attackGrace;
        if (_target.name == "PlayerObject")
        {
            playerKill = _target.GetComponent<PlayerHealth>();
        }
        trapHandle = GameObject.Find("Skeleton_Trap_Trigger").GetComponent<LowerCorridorTrap>();
        gate = GameObject.Find("Gates_System/Gates/Openable_Gate_05/gate_05").GetComponent<WallDirector>();
        trapLever = GameObject.Find("Trap_Activation_Lever/Lever_Interaction_Zone").GetComponent<TrapActivationLever>();
        treasureRoomDirector = GameObject.Find("Openable_Wall_Segment_01/Openable_Wall_01").GetComponent<PlayableDirector>();
    }

    void Update()
    {
        if (!_anim.GetBool("Death"))
        {
            if (ChasePlayer)
            {
                _agent.SetDestination(_target.position);
            }

            if (_agent.velocity.magnitude > 0.2f)
            {
                _anim.SetBool("Walk", true);
            }

            if (_agent.velocity.magnitude < 0.2f)
            {
                _anim.SetBool("Walk", false);
            }

            if (_agent.remainingDistance < 2.5f)
            {
                _anim.SetBool("Attack", true);
                nearPlayer = true;
                _agent.speed = 0;


                attackCountdown -= Time.deltaTime;
                if (attackCountdown <= 0 && !attacked && nearPlayer)
                {
                    playerKill.PlayerDeath(OnPlayerDeath, "You died! Looks like your skull was cracked by some skeleton dude...");
                    _agent.speed = 0;
                    attacked = true;
                }
            }

            if (_agent.remainingDistance >= 2.5f)
            {
                _anim.SetBool("Attack", false);
                _agent.speed = Speed;
                attackCountdown = _attackGrace;
                attacked = false;
                nearPlayer = false;
            }

            if (YouDied)
            {
                _anim.SetBool("Death", true);
                Speed = 0;
                PlayDeath();
            }
        }
    }

    public void OnPlayerDeath()
    {
        transform.position = respawnPoint.position;
        _agent.speed = 0;
        _agent.isStopped = true;
        attacked = false;
        nearPlayer = false;
        attackCountdown = _attackGrace;
        if (!YouDied)
        {
            trapHandle.enabled = true;
            trapHandle.hasWorked = false;
            gate.TechForceOpen();
            trapLever.TechDeactivation();
        }
    }

    private void PlayDeath()
    {
        gate.Play();
        playerKill.currentRespawnPosition = GameObject.Find("Player_SpawnPoints/SpawnPoint_Default").transform;
        textTipRoutine = StartCoroutine(TextTip());
    }

    public IEnumerator TextTip()
    {
        uiText.SetText("A hidden room opened in the back of the corridor...");
        treasureRoomDirector.Play();
        yield return new WaitForSeconds(3f);
        uiText.SetText("But watch your step...");
        yield return new WaitForSeconds(3f);
        uiText.SetText("");
        StopCoroutine(textTipRoutine);
    }
}
