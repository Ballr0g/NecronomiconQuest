using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icy_Chest : TreasureChest
{
    DoorInteraction door;
    GameObject _spawnedItem;
    Coroutine cr;
    public override void SpawnTreasure()
    {
        door = GameObject.Find("Animated_Door_To_Lava/Door_Open_Area").GetComponent<DoorInteraction>();
        _spawnedItem = Instantiate(Treasure_Item, SpawnLocation.transform.position, new Quaternion(0, 0, 0.7071f, 0.7071f), SpawnLocation.transform);
    }

    public override void ExecuteExtraAction()
    {
        cr = StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        yield return new WaitForSeconds(2f);
        door.hasKey = true;
        _textUI.SetText("You found a key! You can open the door in the crypt.");
        yield return new WaitForSeconds(3f);
        Destroy(_spawnedItem);
        _textUI.SetText(string.Empty);
        StopCoroutine(cr);
    }
}
