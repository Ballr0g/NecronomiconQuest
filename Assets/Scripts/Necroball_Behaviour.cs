using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Necroball_Behaviour : FireballBehaviour
{
    [SerializeField]
    private UnityEvent _onPlayerDeath;
    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "PlayerObject")
        {
            player.PlayerDeath(deathMessage: "You were withered by dark magic.");
            _onPlayerDeath.Invoke();
            gameObject.SetActive(false);
        }
    }
}
