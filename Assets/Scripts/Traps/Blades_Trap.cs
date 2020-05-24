using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.AI;

public class Blades_Trap : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector _trapDirector;
    [SerializeField]
    private PlayableAsset _trapCloseAnim, _trapOpenAnim;
    [SerializeField]
    private float _deathCooldown = 0.2f;
    [SerializeField]
    private Skeleton_AI _dungeonSkeleton;
    [SerializeField]
    private float _activationCooldown = 2.0f;

    private Transform skeletonCollider;
    private PlayerHealth playerHit;
    private Coroutine cr1, cr2;
    private float countdown;
    void Start()
    {
        skeletonCollider = _dungeonSkeleton.transform.Find("Skeleton_Hitbox");
        countdown = _activationCooldown;
        playerHit = GameObject.Find("Player/PlayerObject").GetComponent<PlayerHealth>();
    }

    private void FixedUpdate()
    {
        if (countdown > 0)
        {
            countdown -= Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (countdown <= 0)
        {
            _trapDirector.Play(_trapCloseAnim);
            if (other.name == "Skeleton_Hitbox")
            {
                cr1 = StartCoroutine(InitiateSkeletonDeath());
            }
            if (other.name == "PlayerObject")
            {
                cr2 = StartCoroutine(InitiatePlayerDeath());
            }
            countdown = _activationCooldown;
        }
    }

    private IEnumerator InitiateSkeletonDeath()
    {
        yield return new WaitForSeconds(_deathCooldown);
        _dungeonSkeleton.YouDied = true;
        skeletonCollider.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        _trapDirector.Play(_trapOpenAnim);
        StopCoroutine(cr1);
    }

    private IEnumerator InitiatePlayerDeath()
    {
        yield return new WaitForSeconds(_deathCooldown);
        playerHit.PlayerDeath(_dungeonSkeleton.OnPlayerDeath, "You have been sliced in a half.");
        yield return new WaitForSeconds(2f);
        _trapDirector.Play(_trapOpenAnim);
        StopCoroutine(cr2);
    }
}
