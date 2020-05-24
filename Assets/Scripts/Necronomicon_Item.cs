using DungeonGame.AIEntities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Necronomicon_Item : Interactable
{
    [SerializeField]
    private PlayableAsset _necroAgression, _necroFloating;
    [SerializeField]
    private GameObject _realNecronomicon;
    [SerializeField]
    private Zombie_AI[] NecroZombies;
    [SerializeField]
    private WallDirector[] _lockers;

    private Coroutine attachedRoutine;

    protected override void Start()
    {
        base.Start();
        _realNecronomicon.SetActive(false);
        foreach (var zombie in NecroZombies)
        {
            zombie.gameObject.SetActive(false);
        }
    }

    public override void Interact()
    {
        Playable.Play(_necroAgression, DirectorWrapMode.Hold);
        attachedRoutine = StartCoroutine(StartBattle());
    }

    private IEnumerator StartBattle()
    {
        yield return new WaitForSeconds(1.5f);
        foreach (var locker in _lockers)
        {
            locker.Play();
        }
        _textUI.SetText("Huh, you? Hunting the Necronomicon?");
        yield return new WaitForSeconds(2f);
        _textUI.SetText("Too bad... Now the Necronomicon is hunting you! Just turn around, fellow.");
        yield return new WaitForSeconds(2f);
        _textUI.SetText("");
        StopCoroutine(StartBattle());
        foreach (var zombie in NecroZombies)
        {
            zombie.gameObject.SetActive(true);
        }
        _realNecronomicon.SetActive(true);
        transform.parent.gameObject.SetActive(false);
    }

    public void ResetAnimation()
    {
        foreach (var locker in _lockers)
        {
            locker.Play();
        }
        Playable.Play(_necroFloating, DirectorWrapMode.Loop);
        _canInteractAgain = true;
    }

}
