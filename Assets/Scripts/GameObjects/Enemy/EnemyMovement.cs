//using MathNet.Numerics.LinearAlgebra;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 敵の移動を管理するクラス
/// TODO 現在はNavMeshによる処理をStateMachineで記述ているがこっちに書くようにする(関数化などする)
/// </summary>
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

    private void Initialize()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Move();
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

    /// <summary>
    /// 状態に応じて, NavMeshAgentの目的地を設定する
    //! Unused
    /// </summary>
    /// <param name="isVisibleTarget"></param>
    /// <param name="targetPosition"></param>
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

        }

        ChangeSpeed(isVisibleTarget);
    }
}
