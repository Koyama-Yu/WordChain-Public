using UnityEngine;

public class EnemyChaseState : EnemyStateBase
{
    // コンストラクタ
    public EnemyChaseState(EnemyController enemy, EnemyMovement movement) : base(enemy, movement) { }

    public override void Enter()
    {
        // パスをリセット
        _movement.ResetPath();
        // Chaseアニメーションを再生(AnimatorのChaseパラメータをtrueにする)
        _enemy.Animation.PlayAnimation(_enemy.Animation.Chase);
        Debug.Log("ChaseState Enter");
    }

    public override void Execute()
    {
        // ターゲットの位置を追跡対象に設定
        _movement.SetChaseDestination(_enemy.TargetPosition);

        // HealthPointが0になったら、死亡状態に遷移
        if(_enemy.HealthPoint <= 0)
        {
            _enemy.StateMachine.TransitionTo(_enemy.StateMachine.DieState);
            return;
        }

        // ターゲットに到達したら、攻撃状態に遷移
        if (_movement.IsTargetInAttackRange(_enemy.TargetPosition, _enemy.transform.position))
        {
            _enemy.StateMachine.TransitionTo(_enemy.StateMachine.AttackState);
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
        _movement.ResetPath();
    }
}
