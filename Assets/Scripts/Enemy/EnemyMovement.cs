using System.Collections;
using System.Collections.Generic;
//using MathNet.Numerics.LinearAlgebra;

//using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;


public class EnemyMovement : MonoBehaviour
{
    [Header("移動")]
    [SerializeField, Tooltip("さまよい時のスピード")] private float _wanderSpeed = 1.0f;
    [SerializeField, Tooltip("追跡時のスピード")] private float _chaseSpeed = 3.0f;
    [SerializeField, Tooltip("さまよい時の目的地に到着したときの停止距離")] private float _wanderStoppingDistance = 0f;
    [SerializeField, Tooltip("追跡時の目的地に到着したときの停止距離")] private float _chaseStoppingDistance = 2.0f;

    private NavMeshAgent _agent;
    public NavMeshAgent Agent => _agent;
    public GameObject TargetforChase{get; set;}

    private float _currentSpeed = 0.0f;
    public float CurrentSpeed => _currentSpeed;
    public float WanderStoppingDistance => _wanderStoppingDistance;
    public float ChaseStoppingDistance => _chaseStoppingDistance;

    private void Awake()
    {
        Initialize();
    }
    private void Update()
    {
        //Debug.Log(_currentSpeed);
    }

    //水平方向(前後左右)の移動
    public void Move(bool isVisibleTarget, Vector3 targetPosition)
    {
        //SetDestination(isVisibleTarget, targetPosition);
        //Debug.Log(_currentSpeed);
    }


    //TODO 後からここはこれは変更する予定
    private void ChangeSpeed(bool isVisibleTarget)
    {
        if (isVisibleTarget)
        {
            _currentSpeed = _chaseSpeed;
        }
        else
        {
            _currentSpeed = _wanderSpeed;
        }
    }

    private void Initialize()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void SetDestination(bool isVisibleTarget, Vector3 targetPosition)
    {
        if (isVisibleTarget)
        {
            _agent.SetDestination(targetPosition);
            _agent.stoppingDistance = _chaseStoppingDistance; //目的地に到着したときの停止距離
        }
        else if (!_agent.hasPath)
        {
            float newX = transform.position.x + Random.Range(-5, 5);
            float newZ = transform.position.z + Random.Range(-5, 5);

            Vector3 NextPosition = new Vector3(newX, transform.position.y, newZ);

            _agent.SetDestination(NextPosition);  //目的地の設定
            _agent.stoppingDistance = _wanderStoppingDistance; //目的地に到着したときの停止距離

            // _agent.ResetPath();
            // if (!_agent.hasPath)
            // {
            //     float newX = transform.position.x + Random.Range(-5, 5);
            //     float newZ = transform.position.z + Random.Range(-5, 5);

            //     Vector3 NextPosition = new Vector3(newX, transform.position.y, newZ);

            //     _agent.SetDestination(NextPosition);  //目的地の設定
            //     _agent.stoppingDistance = _wanderStoppingDistance; //目的地に到着したときの停止距離
            // }
        }

        ChangeSpeed(isVisibleTarget);
    }
}
