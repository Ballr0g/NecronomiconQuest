using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_FireBall : MonoBehaviour
{
    public Vector3 _movementDirection = Vector3.forward;

    [SerializeField]
    private float _movementSpeed = 3.0f;
    [SerializeField]
    private float _lifeSpan = 5.0f;
    [SerializeField]
    private GameObject _explosion;

    private Rigidbody rb;
    private Transform mainCamera;
    private float remainingTime = 0.0f;
    private bool isSelected = false;

    void Start()
    {
        mainCamera = GameObject.Find("Player/PlayerObject/Main_Camera").transform;
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        remainingTime = _lifeSpan;
    }

    void FixedUpdate()
    {
        if (!isSelected)
        {
            _movementDirection = mainCamera.forward;
            isSelected = true;
            transform.parent = null;
        }
        if (remainingTime > 0)
        {
            rb.AddForce(_movementDirection * _movementSpeed * Time.fixedDeltaTime, ForceMode.Impulse);
            remainingTime -= Time.fixedDeltaTime;
        }
        else
        {
            Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(_explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
