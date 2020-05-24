using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformSmart : WalkableMovingElement
{
    [SerializeField] // Точки перемещения платформы
    private List<Transform> _wayPoints;
    [SerializeField]
    [Range(0.001f, 100f)]// Глобальная скорость движения
    private float _globalMovementSpeed = 1.0f;
    [SerializeField] // Происходит ли движение в обратную сторону?
    private bool _ascendingDirection = true;
    [SerializeField] // Используется ли разная скорость для разных точек?
    private bool _customSpeedForeachWaypoint = false;
    [SerializeField] // Список скоростей для каждой точки (для customSpeedForeachWaypoint = true)
    private List<float> _localMovementSpeed;

    private Transform _currentWaypoint; // Текущая цель перемещения
    private int _currentWaypointIndex = 0; // Текущий индекс цели
    protected virtual void Start()
    {
        if (_wayPoints.Count < 2)
            throw new ArgumentException("There must be at least 2 waypoints.");

        if (_customSpeedForeachWaypoint && _wayPoints.Count != _localMovementSpeed.Count)
            throw new ArgumentException($"The amount of waypoints ({_wayPoints.Count}) does not match with the amount of speed parameters ({_localMovementSpeed})!");

        _currentWaypoint = _wayPoints[0];
    }

    protected virtual void FixedUpdate()
    {
        if (_customSpeedForeachWaypoint)
        {
            if (MoveTowardsTarget(_currentWaypoint, _localMovementSpeed[_currentWaypointIndex]))
                GetNextWaypoint();
        }
        else
        {
            if (MoveTowardsTarget(_currentWaypoint, _globalMovementSpeed))
                GetNextWaypoint();
        }
    }

    public virtual bool MoveTowardsTarget(Transform waypoint, float moveSpeed = 1.0f)
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoint.position, moveSpeed * Time.fixedDeltaTime);

        if (transform.position == waypoint.position)
            return true;

        return false;
    }

    private void GetNextWaypoint()
    {
        if (_ascendingDirection)
        {
            if (_currentWaypointIndex == _wayPoints.Count - 1)
            {
                --_currentWaypointIndex;
                _currentWaypoint = _wayPoints[_currentWaypointIndex];
                _ascendingDirection = false;
                return;
            }
            ++_currentWaypointIndex;
            _currentWaypoint = _wayPoints[_currentWaypointIndex];
            return;
        }

        if (_currentWaypointIndex == 0)
        {
            ++_currentWaypointIndex;
            _currentWaypoint = _wayPoints[_currentWaypointIndex];
            _ascendingDirection = true;
            return;
        }
        --_currentWaypointIndex;
        _currentWaypoint = _wayPoints[_currentWaypointIndex];
    }
}
