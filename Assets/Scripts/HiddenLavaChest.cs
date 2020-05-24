using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenLavaChest : TreasureChest
{
    public static event Action OnHiddenChestOpened;

    private Coroutine cr;
    private GameObject spawnedItem;
    Player_FireballShoot playerFireballAbility;

    protected override void Start()
    {
        base.Start();
        playerFireballAbility = GameObject.Find("Player/PlayerObject").GetComponent<Player_FireballShoot>();
        playerFireballAbility.enabled = false;
    }
    public override void SpawnTreasure()
    {
        spawnedItem = Instantiate(Treasure_Item, SpawnLocation.transform.position, new Quaternion(0, 0, 0.7071f, 0.7071f), SpawnLocation.transform);
        OnHiddenChestOpened?.Invoke();
    }

    public override void ExecuteExtraAction()
    {
        cr = StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        yield return new WaitForSeconds(2f);
        _textUI.SetText("You feel the flow of fire magic in the wand... Left Click to Shoot Fireballs!");
        playerFireballAbility.enabled = true;
        yield return new WaitForSeconds(3f);
        Destroy(spawnedItem);
        _textUI.SetText(string.Empty);
        StopCoroutine(cr);
    }
}
