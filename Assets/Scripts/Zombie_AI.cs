using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace DungeonGame.AIEntities
{
    public enum AIStates
    {
        Idling,
        Alert,
        Chasing,
        Attacking,
        Dead
    }

    [RequireComponent(typeof(NavMeshAgent))]
    public class Zombie_AI : MonoBehaviour
    {
        public AIStates currentState;

        [SerializeField]
        private Collider _attachedCollider;
        [SerializeField]
        private Transform _target;
        [SerializeField]
        private float _alertNoticeRange = 6.0f;
        [SerializeField]
        private float _playerChasingRange = 6.0f;
        [SerializeField]
        private float _playerAttackRange = 2.0f;
        [SerializeField]
        private float _moveSpeed = 1.5f;
        [SerializeField]
        private float _alertTimeout = 2.0f;
        [SerializeField]
        private float _attackGrace = 1.7f;
        [SerializeField]
        private float _enemyProximityCheck = 1.5f;
        [SerializeField]
        private bool _isStandingGuard = true;
        [SerializeField]
        private List<Transform> _followUpWaypoints;
        [SerializeField]
        private UnityEvent _onPlayerDeath;
        [SerializeField]
        private float _otherAICountdown = 10f;

        private NavMeshAgent attachedAgent;
        private Zombie_AI otherEnemy = null;
        private PlayerHealth player;

        private bool walkBackwards = false;
        private int currentWaypointIndex = 0;
        private float distanceToWaypoint = 100f;
        private float distanceToTarget = 100f;
        private float alertCountdown = 0f;
        private float attackCooldown = 0f;
        private Animator attachedAnimator;
        private float aiCountdown = 0f;

        void Start()
        {
            player = GameObject.Find("Player/PlayerObject").GetComponent<PlayerHealth>();
            attachedAgent = GetComponent<NavMeshAgent>();
            attachedAnimator = GetComponent<Animator>();
            attachedAgent.speed = _moveSpeed;
            if (_target == null)
            {
                _target = GameObject.Find("Player/PlayerObject").transform;
            }
            attachedAgent.SetDestination(_followUpWaypoints[currentWaypointIndex].position);
            if (!_isStandingGuard)
                attachedAnimator.SetBool("Walk", true);
        }

        void Update()
        {
            switch (currentState)
            {
                case AIStates.Idling:
                    if (_isStandingGuard)
                        InIdleState();
                    else
                        InPatrolState();
                    break;
                case AIStates.Alert:
                    InAlertState();
                    break;
                case AIStates.Chasing:
                    InChasingState();
                    break;
                case AIStates.Attacking:
                    InAttackingState();
                    break;
                case AIStates.Dead:
                    break;
            }

            // Вызов проверки на наличие врагов по соседству происходит только каждый 10 кадр.
            if (aiCountdown <= 0)
            {
                    CheckForNearbyEnemies();
            }
            else
            {
                aiCountdown -= Time.deltaTime;
            }
        }

        public void OnIdleStateEnter()
        {
            if (!_isStandingGuard)
                attachedAnimator.SetBool("Walk", true);
            attachedAnimator.SetBool("Attack", false);
            attachedAnimator.SetBool("Alert", false);
            attachedAgent.SetDestination(_followUpWaypoints[currentWaypointIndex].position);
            attachedAgent.isStopped = false;
        }

        private void InIdleState()
        {
            attachedAgent.SetDestination(_followUpWaypoints[currentWaypointIndex].position);
            distanceToWaypoint = Vector3.Distance(transform.position, _followUpWaypoints[currentWaypointIndex].position);
            distanceToTarget = Vector3.Distance(transform.position, _target.position);

            if (distanceToWaypoint < 0.2f)
            {
                attachedAnimator.SetBool("Walk", false);
            }
            else
            {
                attachedAnimator.SetBool("Walk", true);
            }
            // Меняем состояние, если игрок рядом
            if (distanceToTarget <= _alertNoticeRange)
            {
                currentState = AIStates.Alert;
                OnAlertStateEnter();
                return;
            }
        }

        private void InPatrolState()
        {
            // Проверяем расстояние до следующей точки перемещения и игрока
            distanceToWaypoint = Vector3.Distance(transform.position, _followUpWaypoints[currentWaypointIndex].position);
            distanceToTarget = Vector3.Distance(transform.position, _target.position);


            if (!attachedAnimator.GetBool("Walk") && !_isStandingGuard)
                attachedAnimator.SetBool("Walk", true);

                // Меняем состояние, если игрок рядом
                if (distanceToTarget <= _alertNoticeRange)
            {
                currentState = AIStates.Alert;
                OnAlertStateEnter();
                return;
            }

            // Циклическое переключение между точками перемещения по логике вперёд-назад
            if (distanceToWaypoint < 0.5f)
            {
                if (!walkBackwards)
                {
                    if (currentWaypointIndex < _followUpWaypoints.Count - 1)
                    {
                        ++currentWaypointIndex;
                    }
                    else
                    {
                        walkBackwards = true;
                        --currentWaypointIndex;
                    }
                    attachedAgent.SetDestination(_followUpWaypoints[currentWaypointIndex].position);
                }
                else
                {
                    if (currentWaypointIndex > 0)
                    {
                        --currentWaypointIndex;
                    }
                    else
                    {
                        walkBackwards = false;
                        ++currentWaypointIndex;
                    }
                    attachedAgent.SetDestination(_followUpWaypoints[currentWaypointIndex].position);
                }
            }
        }

        private void OnAlertStateEnter()
        {
            attachedAnimator.SetBool("Walk", false);
            attachedAnimator.SetBool("Attack", false);
            attachedAnimator.SetBool("Alert", true);
            attachedAgent.isStopped = true;
            alertCountdown = _alertTimeout;
        }

        private void InAlertState()
        {
            distanceToTarget = Vector3.Distance(transform.position, _target.position);
            alertCountdown -= Time.deltaTime;

            if (distanceToTarget > _alertNoticeRange && alertCountdown < 0)
            {
                currentState = AIStates.Idling;
                OnIdleStateEnter();
                return;
            }

            if (alertCountdown < 0)
            {
                currentState = AIStates.Chasing;
                OnChacingStateEnter();
                return;
            }
        }

        private void OnChacingStateEnter()
        {
            attachedAgent.isStopped = false;
            attachedAnimator.SetBool("Walk", true);
            attachedAnimator.SetBool("Attack", false);
            attachedAnimator.SetBool("Alert", false);
        }

        private void InChasingState()
        {
            distanceToTarget = Vector3.Distance(transform.position, _target.position);
            attachedAgent.SetDestination(_target.position);
            if (distanceToTarget > _playerChasingRange)
            {
                currentState = AIStates.Alert;
                OnAlertStateEnter();
                return;
            }

            if (distanceToTarget <= _playerAttackRange)
            {
                currentState = AIStates.Attacking;
                OnAttackingStateEnter();
                return;
            }
        }

        private void OnAttackingStateEnter()
        {
            attachedAgent.isStopped = true;
            attackCooldown = _attackGrace;
            attachedAnimator.SetBool("Walk", false);
            attachedAnimator.SetBool("Attack", true);
            attachedAnimator.SetBool("Alert", false);
        }

        private void InAttackingState()
        {
            distanceToTarget = Vector3.Distance(transform.position, _target.position);
            if (distanceToTarget <= _playerAttackRange)
            {
                attackCooldown -= Time.deltaTime;
                if (attackCooldown <= 0)
                {
                    player.PlayerDeath(deathMessage: "Your brains were taken by the dungeon keeper...");
                    _onPlayerDeath.Invoke();
                }
            }
            else
            {
                currentState = AIStates.Chasing;
                OnChacingStateEnter();
            }
        }

        public void OnDeadStateEnter()
        {
            attachedAgent.speed = 0;
            attachedAgent.isStopped = true;
            _attachedCollider.enabled = false;
            attachedAnimator.SetTrigger("Dead");
            transform.position = new Vector3(transform.position.x, transform.position.y - 3f, transform.position.z);
        }

        private void CheckForNearbyEnemies()
        {
            if (Time.frameCount % 10 == 0)
            {
                // Хранит данные о взаимодействии с невидимыми лучами после вызова Physics.SphereCastNonAlloc.
                RaycastHit[] hitData = new RaycastHit[2];
                /*  Памятка по методу Physics.SphereCastNonAlloc:
                 *  
                 *  Physics.SphereCastNonAlloc - метод, генерирующий невидимую сферу указанного радиуса в заданной точке. Создаёт невидимые лучи до объектов в радиусе,
                 *  сохраняя их в указанном массиве, причём в отличие от Physics.SphereCast количество лучей равно размеру указанного массива + не генерируется мусор.
                 * 
                 * 1) Источник генерации невидимой сферы
                 * 2) Радиус генерации сферы
                 * 3) Направление генерируемых лучей
                 * 4) Массив, в котором соханится информация о столкновении с невидимыми лучами
                 * 5) Максимальное расстояние взаимодействия.
                 * 6) Слой, на котором происходит взаимодействие (9 - Enemy Detect, не менять!)
                 * 
                 */
                int enemiesCloseCount = Physics.SphereCastNonAlloc(transform.position, _enemyProximityCheck, transform.forward, hitData, _enemyProximityCheck, 1 << 9);
                if (enemiesCloseCount > 1)
                {
                    otherEnemy = hitData[0].collider.GetComponent<Zombie_AI>();
                    if (otherEnemy != null && currentState == AIStates.Chasing)
                    {
                        otherEnemy.currentState = currentState;
                        aiCountdown = _otherAICountdown;
                        otherEnemy.aiCountdown = otherEnemy._otherAICountdown;
                    }
                }
            }
        }

        public void ResetPosition()
        {
            currentWaypointIndex = 0;
            currentState = AIStates.Idling;
            OnIdleStateEnter();
            transform.position = _followUpWaypoints[currentWaypointIndex].position;
        }

        // Для отображения радиуса взаимодействия с врагами в Scene View.
        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.green;
        //    Gizmos.DrawSphere(transform.position, _enemyProximityCheck);
        //}
    }
}
