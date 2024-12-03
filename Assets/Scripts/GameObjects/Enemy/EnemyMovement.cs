//using MathNet.Numerics.LinearAlgebra;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 敵の移動を管理するクラス
/// TODO 現在はNavMeshによる処理をStateMachineで記述ているがこっちに書くようにする(関数化などする)
/// </summary>
public class EnemyMovement : MonoBehaviour
{
    [Header("移動スピード")]
    [SerializeField, Tooltip("さまよい時のスピード")]
    private float _wanderSpeed = 1.0f;
    [SerializeField, Tooltip("追跡時のスピード")]
    private float _chaseSpeed = 3.0f;
    [Header("目的地に到着したときの停止距離")]
    [SerializeField, Tooltip("さまよい時の目的地に到着したときの停止距離")]
    private float _wanderStoppingDistance = 0f;
    [SerializeField, Tooltip("追跡時の目的地に到着したときの停止距離")]
    private float _chaseStoppingDistance = 2.0f;

    private NavMeshAgent _agent;
    //public NavMeshAgent Agent => _agent;

    private float _currentSpeed = 0.0f;
    public float CurrentSpeed => _currentSpeed;
    private Vector3 _resumeVelocity; // ポーズ中に速度を保持するための変数
    public float WanderStoppingDistance => _wanderStoppingDistance;
    public float ChaseStoppingDistance => _chaseStoppingDistance;

    public float RANDOM_WANDER_DISTANCE = 5.0f; // Wander時のランダム移動距離

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
    //! Unused
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
    /// Wander時の目的地をNavMeshAgentに設定
    /// </summary>
    public void SetWanderDestination()
    {
        // さまよい時の目的地をランダムに設定
        float newX = transform.position.x 
            + Random.Range(-RANDOM_WANDER_DISTANCE, RANDOM_WANDER_DISTANCE);
        float newZ = transform.position.z 
            + Random.Range(-RANDOM_WANDER_DISTANCE, RANDOM_WANDER_DISTANCE);

        Vector3 NextPosition = new Vector3(newX, transform.position.y, newZ);

        _agent.SetDestination(NextPosition);  //目的地の設定
        _agent.stoppingDistance = _wanderStoppingDistance; //目的地に到着したときの停止距離
    }

    /// <summary>
    /// Chase時の目的地をNavMeshAgentに設定 <br/>
    /// targetPositionは, ChaseStateで_enemy.TargetPositionを渡す
    /// </summary>
    public void SetChaseDestination(Vector3 targetPosition)
    {
        _agent.SetDestination(targetPosition);
        _agent.stoppingDistance = _chaseStoppingDistance; //目的地に到着したときの停止距離
    }

    public void SetDestination(Vector3 destination)
    {
        _agent.SetDestination(destination);
    }

    /// <summary>
    /// Pathの存在を確認してからリセット
    /// </summary>
    public void ResetPath()
    {
        if(_agent.hasPath)
        {
            _agent.ResetPath();
        }
    }

    public bool hasPath()
    {
        return _agent.hasPath;
    }

    /// <summary>
    /// ゲーム停止時の処理.
    /// NavMeshAgentの移動を停止.
    /// </summary>
    public void DisableMovement()
    {
        if (_agent != null)
        {
            _agent.isStopped = true;
            _resumeVelocity = _agent.velocity;
            _agent.velocity = Vector3.zero;
        }
    }

    /// <summary>
    /// ゲーム再開時の処理.
    /// NavMeshAgentの移動を再開.
    /// </summary>
    public void EnableMovement()
    {
        if (_agent != null)
        {
            _agent.isStopped = false;
            _agent.velocity = _resumeVelocity;
        }
    }


}
