using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public sealed class PlayerHealth : MonoBehaviour
{
    public Transform currentRespawnPosition;

    private Transform _playerTransform;
    FPS_Controller controller;
    private TextMeshProUGUI _textUI;
    private Coroutine showDeathMessageRoutine;

    void Start()
    {
        _playerTransform = GameObject.Find("PlayerObject").transform;
        controller = _playerTransform.GetComponent<FPS_Controller>();
        _textUI = GameObject.Find("Canvas/UI_Instructions").GetComponent<TextMeshProUGUI>();
    }

    public void PlayerDeath(Action revertGame = null, string deathMessage = "")
    {
        showDeathMessageRoutine = StartCoroutine(ShowDeathMessageRoutine(deathMessage));
        _playerTransform.position = currentRespawnPosition.position;
        revertGame?.Invoke();
    }

    public void SetSpawnPoint(Transform spawnpoint)
    {
        if (spawnpoint != null)
        {
            currentRespawnPosition = spawnpoint;
        }
    }

    private IEnumerator ShowDeathMessageRoutine(string deathMessage)
    {
        _textUI.SetText(deathMessage);
        controller.enabled = false;
        yield return new WaitForSeconds(0.1f);
        controller.enabled = true;
        yield return new WaitForSeconds(4f);
        _textUI.SetText("");
        StopCoroutine(showDeathMessageRoutine);
    }
}
