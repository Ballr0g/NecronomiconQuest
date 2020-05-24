using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : Interactable
{
    [Header("Contains a Treasure")]
    public bool ContainsTreasure;
    [Header("Treasure GameObject")]
    public GameObject Treasure_Item;
    [Header("GameObject Spawn Location")]
    public GameObject SpawnLocation;

    public override void Interact()
    {
        Playable.Play();
    }

    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && _isPlayerClose)
        {
            // Проверяем, может ли игрок многократно взаимодействовать с предметом или нет
            if (InteractOnce && _canInteractAgain)
            {
                Interact();
                if (extraMethodCall)
                    ExecuteExtraAction();
                if (ContainsTreasure)
                {
                    SpawnTreasure();
                }
                // _textUI.SetText(string.Empty);
                _canInteractAgain = false;
            }

            if (!InteractOnce)
            {
                Interact();
                if (extraMethodCall)
                    ExecuteExtraAction();
                if (ContainsTreasure)
                {
                    SpawnTreasure();
                }
            }
        }
    }

    // Используется для генерации предмета-сокровища, предназначен для сундуков
    public virtual void SpawnTreasure()
    {
        Instantiate(Treasure_Item, SpawnLocation.transform.position, SpawnLocation.transform.rotation, SpawnLocation.transform);
    }
}
