using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform01 : MonoBehaviour
{
    [SerializeField] private Transform pointA, pointB;
    [SerializeField] private float _speed = 1.0f;
    private bool isGoingToPointB = true;
    void Update()
    {
        if (isGoingToPointB)
            transform.position = Vector3.MoveTowards(transform.position, pointB.position, _speed * Time.deltaTime);
        else
            transform.position = Vector3.MoveTowards(transform.position, pointA.position, _speed * Time.deltaTime);

        if (transform.position == pointB.position)
            isGoingToPointB = false;
        if (transform.position == pointA.position)
            isGoingToPointB = true;

        // Ругаемся на пользователя, если точки начала и конца перемещения совпали.
        if (pointA.position == pointB.position)
            Debug.LogWarning("pointA is equal to pointB, you should consider moving either of them.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.name == "Player")
        {
            print("test");
            other.transform.parent.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent.name == "Player")
        {
            print("exit test");
            other.transform.parent.parent = null;
        }
    }
}
