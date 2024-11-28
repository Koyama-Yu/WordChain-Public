using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyUtilities;

public class EnemyWanderState : EnemyStateBase
{
    // コンストラクタ
    public EnemyWanderState(EnemyController enemy, EnemyMovement movement) : base(enemy, movement) { }

    // WanderStateに入ったとき、ランダムな位置へのパスを設定
    public override void Enter()
    {
        float newX = _enemy.transform.position.x + Random.Range(-5, 5);
        float newZ = _enemy.transform.position.z + Random.Range(-5, 5);

        Vector3 NextPosition = new Vector3(newX, _enemy.transform.position.y, newZ);

        _movement.Agent.SetDestination(NextPosition);  //目的地の設定
        _movement.Agent.stoppingDistance = _movement.WanderStoppingDistance; //目的地に到着したときの停止距離

        Debug.Log("WanderState Enter");
    }

    public override void Execute()
    {
        // HealthPointが0になったら、死亡状態に遷移
        if (_enemy.HealthPoint <= 0)
        {
            _enemy.StateMachine.TransitionTo(_enemy.StateMachine.DieState);
            return;
        }

        // 視界にプレイヤーがいる場合は、追跡状態に遷移
        if (_enemy.IsVisibleTarget)
        {
            _enemy.StateMachine.TransitionTo(_enemy.StateMachine.ChaseState);
            return;
        }

        // Enterで設定した目的地に到着したら、待機状態へ遷移
        if (!_movement.Agent.hasPath)
        {
            _enemy.StateMachine.TransitionTo(_enemy.StateMachine.IdleState);
            return;
        }
    }

    public override void Exit()
    {
        //Debug.Log("WanderState Exit");
        _movement.Agent.ResetPath();
    }
}
