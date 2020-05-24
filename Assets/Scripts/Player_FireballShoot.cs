using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_FireballShoot : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerFireball;
    [SerializeField]
    private Transform _playerFireballSpawnPosition;
    [SerializeField]
    private float _fireballShootCooldown = 1.0f;

    private List<GameObject> playerFireballs;
    private float shootCooldown;
    private Transform mainCamera;
    void Start()
    {
        if (playerFireballs == null)
        {
            playerFireballs = new List<GameObject>();
        }
        shootCooldown = _fireballShootCooldown;
        mainCamera = GameObject.Find("Main_Camera").transform;
    }

    private void FixedUpdate()
    {
        if (!PauseMenu.GamePaused) 
            if (shootCooldown > 0)
                shootCooldown -= Time.fixedDeltaTime;
    }
    void Update()
    {
        if (!PauseMenu.GamePaused && Input.GetKey(KeyCode.Mouse0) && shootCooldown < 0)
        {
            GameObject fireball = Instantiate(_playerFireball, _playerFireballSpawnPosition.position, mainCamera.localRotation, _playerFireballSpawnPosition);
            fireball.SetActive(true);
            fireball.transform.position = _playerFireballSpawnPosition.position;
            shootCooldown = _fireballShootCooldown;
        }
    }
}
