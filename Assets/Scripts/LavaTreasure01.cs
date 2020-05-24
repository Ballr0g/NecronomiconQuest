using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaTreasure01 : TreasureChest
{
    private GameObject _part1Platforms, _part2Platforms;
    private GameObject _leftStatueTurret, _rightStatueTurret;
    private Coroutine cr;

    public static event Action OnMonumentChestOpened;

    protected override void Start()
    {
        base.Start();
        _part1Platforms = GameObject.Find("Lava_Area_Platforms/Part_01");
        _part2Platforms = GameObject.Find("Lava_Area_Platforms/Part_02");
        _part2Platforms.SetActive(false);
        _leftStatueTurret = GameObject.Find("Moving_Gothic_Statue_01/Fireball_Spawner");
        _rightStatueTurret = GameObject.Find("Moving_Gothic_Statue_02/Fireball_Spawner");
        _leftStatueTurret.SetActive(false);
        _rightStatueTurret.SetActive(false);
    }
    public override void ExecuteExtraAction()
    {
        _part1Platforms.SetActive(false);
        _part2Platforms.SetActive(true);
        _leftStatueTurret.gameObject.SetActive(true);
        _rightStatueTurret.gameObject.SetActive(true);
        OnMonumentChestOpened?.Invoke();
        cr = StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        _textUI.SetText("Something has changed behind you...");
        yield return new WaitForSeconds(3f);
        _textUI.SetText(string.Empty);
        StopCoroutine(cr);
    }
}