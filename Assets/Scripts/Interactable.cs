using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

public abstract class Interactable : MonoBehaviour
{
    [Header("GameObject with PlayableDirector")]
    public PlayableDirector Playable;
    [Header("UI Message")]
    public string text;
    [Header("Allow Multiple Interaction")]
    public bool InteractOnce;
    [Header("Execute an extra method")]
    public bool extraMethodCall = false;

    protected bool _canInteractAgain = true;
    protected TextMeshProUGUI _textUI; // reference to GUI Text
    protected bool _isPlayerClose;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _textUI = GameObject.Find("Canvas/UI_Instructions").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && _isPlayerClose)
        {
            // Проверяем, может ли игрок многократно взаимодействовать с предметом или нет
            if (InteractOnce && _canInteractAgain)
            {
                Interact();
                if (extraMethodCall)
                    ExecuteExtraAction();
                _textUI.SetText(string.Empty);
                _canInteractAgain = false;
            }

            if (!InteractOnce)
            {
                Interact();
                if (extraMethodCall)
                    ExecuteExtraAction();
            }
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        // Находится ли игрок рядом с объектом?
        // Выводим сообщение, если взаимодействие возможно
        if (other.name == "PlayerObject")
        {
            if (_canInteractAgain)
            {
                _textUI.SetText(text);
            }
            _isPlayerClose = true;
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        // Очищаем пользовательский интерфейс при выходе из активной зоны
        if (other.name == "PlayerObject")
        {
            _textUI.SetText(string.Empty);
            _isPlayerClose = false;
        }
    }

    // Взаимодействие устроено таким образом, что
    // вызывается через Update() даже без переопределения
    public abstract void Interact();

    public virtual void ExecuteExtraAction() => Debug.Log("Replace this with an action");
}
