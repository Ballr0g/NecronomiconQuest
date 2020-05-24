using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPS_Controller : MonoBehaviour
{
    [Header("Controller Info")]
    [SerializeField ][Tooltip("Player Walking Speed")]
    public float _walkSpeed = 1.0f; 
    [SerializeField][Tooltip("Player Running Speed")]
    private float _runSpeed = 2.0f;
    [SerializeField][Tooltip("Player Gravity")] 
    private float _gravity = 1.0f;
    [SerializeField][Tooltip("Player Jump Height")]
    private float _jumpHeight = 15.0f;

    private CharacterController _controller;
    private Transform parentObj;
    private float _yVelocity = 0.0f; // Для кэширования скорости


    [Header("Headbob Settings")]

    private Animator _anim;
    private float _speedMultForTl; // Множитель скорости для Timeline
    private bool _isGrounded;
    private FootstepSFX _footstepSFX;

    [SerializeField][Tooltip("Walking head bobbing frequency")]
    private float _walkFrequency = 4.8f; // Частота покачивания при ходьбе
    [SerializeField][Tooltip("Running head bobbing frequency")]
    private float _runFrequency = 7.8f; // Частота покачивания при беге
    [SerializeField][Tooltip("Head bobbing strength")][Range(0.0f, 0.2f)]
    private float _heightOffset = 0.05f; // Насколько сильно

    [Header("Camera Settings")]
    [SerializeField][Tooltip("Camera Sensitivity")]
    private float _lookSensitivity = 5.0f; // Чувствительность мыши

    private Camera _fpsCamera;

    private void Start()
    {
        parentObj = transform.parent;
        _controller = GetComponent<CharacterController>();
        _fpsCamera = GetComponentInChildren<Camera>();
        _anim = transform.Find("Main_Camera").GetComponentInChildren<Animator>();
        _footstepSFX = transform.Find("Main_Camera").GetComponentInChildren<FootstepSFX>();
        Cursor.visible = false;
    }

    private void Update()
    {
        if (!PauseMenu.GamePaused)
        {
            if (parentObj?.name == "Player")
                parentObj.position = transform.position;

            FPSController();
            CameraController();
            HeadBobbing();
        }
    }
    void FPSController()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(h, 0, v); // Направление движения
        Vector3 velocity = direction * _walkSpeed; // Скорость

        if (Input.GetKey(KeyCode.LeftShift)) // Бегаем?
        {
            velocity = direction * _runSpeed;
            _isGrounded = true;
        }

        if (_controller.isGrounded == true) // Находится ли игрок на земле
        {
            _footstepSFX.walkVolume = 1.0f;  // Включаем звук шагов
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _yVelocity = _jumpHeight; // Скорость по y
                _footstepSFX.PlayJumpSound();
            }
        }
        else // Не на земле
        {
            _yVelocity -= _gravity; // Вычитаем гравитацию 
            _isGrounded = false;
            _footstepSFX.walkVolume = 0.0f;  // Выключаем шаги

        }

        velocity.y = _yVelocity; // используем кэшированную скорость

        velocity = transform.TransformDirection(velocity);

        _controller.Move(velocity * Time.deltaTime); // Двигаем персонажа методом CharacterController
    }

    void CameraController()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 rot = transform.localEulerAngles; // текущий поворот игрока
        rot.y += mouseX * _lookSensitivity; // добавляем поворот в зависимости от чувствительности (y)
        transform.localRotation = Quaternion.AngleAxis(rot.y, Vector3.up); // Повород на угол вокруг оси

        Vector3 camRot = _fpsCamera.transform.localEulerAngles; // текущее вращение камеры
        camRot.x += -mouseY * _lookSensitivity; // добавляем поворот в зависимости от чувствительности (x)
        _fpsCamera.transform.localRotation = Quaternion.AngleAxis(camRot.x, Vector3.right); // Вращение вокруг оси
    }

    void HeadBobbing()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (h != 0 || v != 0) // Скорость ненулевая
        {
            _anim.SetBool("IsWalking", true); // Анимация ходьбы
            _speedMultForTl = _walkSpeed * 1.1f; // Скорость анимации
            _anim.SetFloat("SpeedFactor", _speedMultForTl); // Задаём значение аниматору

            if (_isGrounded == true)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    _anim.SetBool("IsRunning", true);  // Покачивание головы при беге
                    _speedMultForTl = _walkSpeed * 1.2f;  
                    _anim.SetFloat("SpeedFactor", _speedMultForTl);
                }

                else
                {
                    _anim.SetBool("IsRunning", false); // Мы не бежим   
                    _anim.SetBool("Idle", true);
                }
                    
            }

            if (_isGrounded == false)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    _anim.SetBool("IsRunning", false);
                    _speedMultForTl = _walkSpeed * 1.2f;
                    _anim.SetFloat("SpeedFactor", _speedMultForTl);
                }
            }
        }
        else
        {
            _anim.SetBool("IsWalking", false); // Выключаем ходьбу, мы стоим
            _anim.SetFloat("SpeedFactor", 1.0f); // Сбрасываем скорость анимации
                
        }
    }
}


