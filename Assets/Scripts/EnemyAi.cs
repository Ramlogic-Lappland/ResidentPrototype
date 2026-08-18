using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAi : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform zombieTransform;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;


    [Header("Layers")]
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private LayerMask playerLayerMask;


    [Header("Patrol Settings")]
    [SerializeField] private float patrolRadius = 10f;
    private Vector3 _currentPatrolPoint;
    private bool _hasPatrolPoint;


    [Header("Combat Settings")]
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float forwardShotForce = 10f;
    [SerializeField] private float verticalShotForce = 5f;
    private bool _isOnAttackCooldown;


    [Header("Detection Ranges")]
    [SerializeField] private float visionRange = 20f;
    [SerializeField] private float engagementRange = 10f;


    private bool _isPlayerVisible;
    private bool _isPlayerInRange;


    private void Awake()
    {
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.Find("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
        }


        if (navAgent == null)
        {
            navAgent = GetComponent<NavMeshAgent>();
        }
    }


    private void Update()
    {
        DetectPlayer();
        UpdateBehaviourState();
    }

    private void UpdateBehaviourState()
    {
        if (!_isPlayerVisible && !_isPlayerInRange)
        {
            PerformPatrol();
        }
        else if (_isPlayerVisible && !_isPlayerInRange)
        {
            PerformChase();
        }
        else if (_isPlayerVisible && _isPlayerInRange)
        {
            PerformAttack();
        }
    }

    #region MovementLogic
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, engagementRange);


        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);
    }
    
    private void FindPatrolPoint()
    {
        float randomX = Random.Range(-patrolRadius, patrolRadius);
        float randomZ = Random.Range(-patrolRadius, patrolRadius);


        Vector3 potentialPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);


        if (Physics.Raycast(potentialPoint, -transform.up, 2f, terrainLayer))
        {
            _currentPatrolPoint = potentialPoint;
            _hasPatrolPoint = true;
        }
    }
    
    private void PerformPatrol()
    {
        if (!_hasPatrolPoint)
            FindPatrolPoint();


        if (_hasPatrolPoint)
            navAgent.SetDestination(_currentPatrolPoint);


        if (Vector3.Distance(zombieTransform.position, _currentPatrolPoint) < 1f)
            _hasPatrolPoint = false;
    }
    
    private void DetectPlayer()
    {
        _isPlayerVisible = Physics.CheckSphere(transform.position, visionRange, playerLayerMask);
        _isPlayerInRange = Physics.CheckSphere(transform.position, engagementRange, playerLayerMask);
    }
    
    private void PerformChase()
    {
        if (playerTransform != null)
        {
            navAgent.SetDestination(playerTransform.position);
        }
    }
    #endregion


    private void FireProjectile()
    {
        if (projectilePrefab == null || firePoint == null) return;


        Rigidbody projectileRb = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        projectileRb.AddForce(transform.forward * forwardShotForce, ForceMode.Impulse);
        projectileRb.AddForce(transform.up * verticalShotForce, ForceMode.Impulse);


        Destroy(projectileRb.gameObject, 3f);
    }


    private IEnumerator AttackCooldownRoutine()
    {
        _isOnAttackCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        _isOnAttackCooldown = false;
    }

    private void PerformAttack()
    {
        navAgent.SetDestination(transform.position);


        if (playerTransform != null)
        {
            transform.LookAt(playerTransform);
        }


        if (!_isOnAttackCooldown)
        {
            //FireProjectile();
            //StartCoroutine(AttackCooldownRoutine());
        }
    }

}
