using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Zombie_Target : MonoBehaviour
{
    public static int targetsHit = 0;


    [SerializeField]
    private float _disableTimeOut = 30f;
    [SerializeField]
    private string _onDisableMessage = "";
    [SerializeField]
    private bool _isActivated = false;

    private TextMeshProUGUI uitext;
    private Coroutine disableRoutine;
    private bool coroutineStarted = false;

    private MeshRenderer targetRenderer;
    void Start()
    {
        targetRenderer = GetComponent<MeshRenderer>();
        uitext = GameObject.Find("Canvas/UI_Instructions").GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isActivated && other.name.Contains("PlayerFireBall"))
        {
            _isActivated = true;
            coroutineStarted = true;
            targetRenderer.material.color = Color.red;
            ++targetsHit;
            disableRoutine = StartCoroutine(TargetDisableRoutine(_onDisableMessage));
        }
    }

    private IEnumerator TargetDisableRoutine(string disablingMsg)
    {
        yield return new WaitForSeconds(_disableTimeOut);
        uitext.SetText(disablingMsg);
        yield return new WaitForSeconds(0.5f);
        targetRenderer.material.color = Color.white;
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(0.5f);
            targetRenderer.material.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            targetRenderer.material.color = Color.white;
        }
        uitext.SetText("");
        _isActivated = false;
        --targetsHit;
        coroutineStarted = false;
        StopCoroutine(disableRoutine);
    }

    public void ForceDisable()
    {
        if (coroutineStarted)
        {
            StopCoroutine(disableRoutine);
        }
        _isActivated = false;
        coroutineStarted = false;
        targetRenderer.material.color = Color.white;
        targetsHit = 0;
        uitext.SetText("");
    }
}
