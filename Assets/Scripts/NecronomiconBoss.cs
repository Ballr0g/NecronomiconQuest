using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NecronomiconBoss : MonoBehaviour
{
    public int bossHealth = 5;
    [SerializeField]
    private GameObject _spawnableBall;
    [SerializeField]
    private Transform _spawnPosition;
    [SerializeField]
    private float _shootCooldown = 10f;
    [SerializeField]
    private List<GameObject> _necroBalls;

    private MeshRenderer necroRenderer;
    private Coroutine damageRoutine, shootRoutine;
    private bool canBeDamaged = true;

    void Start()
    {
        necroRenderer = GetComponent<MeshRenderer>();
    }


    private void OnEnable()
    {
        foreach (GameObject necroball in _necroBalls)
        {
            necroball.SetActive(false);
        }
        shootRoutine = StartCoroutine(ShootFireballs(_shootCooldown));
    }

    void Update()
    {
        if (bossHealth == 0)
        {
            SceneManager.LoadScene(2);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("PlayerFireBall") && canBeDamaged)
        {
            --bossHealth;
            canBeDamaged = false;
            damageRoutine = StartCoroutine(TakeDamage());
        }
    }

    private IEnumerator TakeDamage()
    {
        for (int i = 0; i < 5; ++i) {
            necroRenderer.material.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            necroRenderer.material.color = Color.white;
            yield return new WaitForSeconds(0.2f);
        }
        canBeDamaged = true;
        StopCoroutine(damageRoutine);
    }

    private IEnumerator ShootFireballs(float shootCooldown)
    {
        while (true)
        {
            for (int i = 0; i < 3; i++)
            {
                bool foundBall = false;
                for (int j = 0; j < _necroBalls.Count; j++)
                {
                    if (!foundBall && !_necroBalls[j].activeSelf)
                    {
                        foundBall = true;
                        _necroBalls[j].SetActive(true);
                        _necroBalls[j].transform.position = _spawnPosition.position;
                        j = _necroBalls.Count;
                    }
                }
                if (!foundBall)
                {
                    _necroBalls.Add(Instantiate<GameObject>(_spawnableBall, _spawnPosition));
                    _necroBalls[_necroBalls.Count - 1].SetActive(true);
                }
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(shootCooldown);
        }
    }


    public void ResetTheBoss()
    {
        necroRenderer.material.color = Color.white;
        if (shootRoutine != null)
        {
            StopCoroutine(shootRoutine);
        }
        bossHealth = 5;
        canBeDamaged = true;
        transform.parent.parent.gameObject.SetActive(false);
    }
}
