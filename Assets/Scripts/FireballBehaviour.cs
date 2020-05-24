using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FireballBehaviour : MonoBehaviour
{
    // Поведение огненной сферы, используется паттерн Object Pooling -
    // в момент окончания использования объект деактивируется, а не уничтожается
    [SerializeField] // Время жизни объекта
    private float _lifespan = 5.0f;
    [SerializeField] // скорость передвижения
    private float _movementSpeed = 1.0f;
    [SerializeField] // Направление передвижения
    private Vector3 _movementDirection = Vector3.back;

    protected PlayerHealth player;
    protected Rigidbody rb;
    protected float _remainingTime = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player/PlayerObject").GetComponent<PlayerHealth>();
    }

    // Метод необходим для паттерна Object Pooling, устанавливает срок
    // жизни объекта при каждой его активации
    private void OnEnable()
    {
        _remainingTime = _lifespan;
    }

    protected virtual void FixedUpdate()
    {
        if (_remainingTime > 0)
        {
            rb.angularVelocity = Vector3.one;
            rb.AddForce(_movementDirection * _movementSpeed * Time.fixedDeltaTime, ForceMode.Impulse);
            // transform.Translate(_movementDirection * _movementSpeed * Time.fixedDeltaTime);
            _remainingTime -= Time.fixedDeltaTime;
        }
        else
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.one;
            gameObject.SetActive(false);
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "PlayerObject")
        {
            player.PlayerDeath(deathMessage: "You tried to ride a fireball.");
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.one;
            gameObject.SetActive(false);
        }
    }
}
