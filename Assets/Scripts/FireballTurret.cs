using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballTurret : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _backupSpawanable;

    [SerializeField]
    private List<Rigidbody> _fireballs;
    [SerializeField]
    private Transform _spawnLocation;
    [SerializeField]
    private float _shootCooldown;
    [SerializeField]
    private bool _isActive = true;

    private Coroutine fireballSpawnRoutine;
    void Start()
    {
        HiddenLavaChest.OnHiddenChestOpened += HiddenChestOpenedHandler;
        LavaTreasure01.OnMonumentChestOpened += MonumentChestOpenedHandler;
        _fireballs.AddRange(transform.GetComponentsInChildren<Rigidbody>(true));
    }


    private IEnumerator ShootFireballsRoutine()
    {
        while (true)
        {
            bool inactiveFound = false;
            for (int i = 0; i < _fireballs.Count; i++)
            {
                if (!_fireballs[i].gameObject.activeSelf)
                {
                    _fireballs[i].gameObject.SetActive(true);
                    _fireballs[i].transform.position = _spawnLocation.position;
                    inactiveFound = true;
                    i = _fireballs.Count;
                    yield return new WaitForSeconds(_shootCooldown);
                }
            }
            if (!inactiveFound)
            {
                _fireballs.Add(Instantiate<Rigidbody>(_backupSpawanable, _spawnLocation));
                _fireballs[_fireballs.Count - 1].gameObject.SetActive(true);
                yield return new WaitForSeconds(_shootCooldown);
            }
        }
    }

    private void HiddenChestOpenedHandler()
    {
        StopCoroutine(fireballSpawnRoutine);
    }

    private void MonumentChestOpenedHandler()
    {
        fireballSpawnRoutine = StartCoroutine(ShootFireballsRoutine());
    }
}
