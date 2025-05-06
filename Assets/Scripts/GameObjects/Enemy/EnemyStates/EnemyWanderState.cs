using UnityEngine;

public class EnemyWanderState : EnemyStateBase
{
    // コンストラクタ
    public EnemyWanderState(EnemyController enemy, EnemyMovement movement) : base(enemy, movement) { }

    // WanderStateに入ったとき、ランダムな位置へのパスを設定
    public override void Enter()
    {
        _movement.SetWanderDestination();
        // Wanderアニメーションを再生(AnimatorのWanderパラメータをtrueにする)
        _enemy.Animation.PlayAnimation(_enemy.Animation.Wander);
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

        // Enterで設定した目的地に到着したら(Pathがなくなったら)、待機状態へ遷移
        if (!_movement.hasPath())
        {
            _enemy.StateMachine.TransitionTo(_enemy.StateMachine.IdleState);
            return;
        }
    }

    public override void Exit()
    {
        //Debug.Log("WanderState Exit");
        _movement.ResetPath();
    }
}
