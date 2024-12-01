using UnityEngine;

public class EnemyChaseState : EnemyStateBase
{
    // コンストラクタ
    public EnemyChaseState(EnemyController enemy, EnemyMovement movement) : base(enemy, movement) { }

    public override void Enter()
    {
        // パスをリセット
        if(_movement.Agent.hasPath)
        {
            _movement.Agent.ResetPath();
        }
        Debug.Log("ChaseState Enter");
    }

    public override void Execute()
    {
        // ターゲットの位置を目的地に設定
        _movement.Agent.SetDestination(_movement.TargetforChase.transform.position);
        _movement.Agent.stoppingDistance = _movement.ChaseStoppingDistance; //目的地に到着したときの停止距離

        // HealthPointが0になったら、死亡状態に遷移
        if(_enemy.HealthPoint <= 0)
        {
            _enemy.StateMachine.TransitionTo(_enemy.StateMachine.DieState);
            return;
        }

        // 視界からターゲットが消えたら、さまよい状態に遷移
        if(!_enemy.IsVisibleTarget)
        {
            _enemy.StateMachine.TransitionTo(_enemy.StateMachine.WanderState);
            return;
        }
    }

    public override void Exit()
    {
        //Debug.Log("ChaseState Exit");
        // パスをリセット
        _movement.Agent.ResetPath();
    }
}
