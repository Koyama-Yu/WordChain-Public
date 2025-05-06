using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyUtilities;
using UnityEngine.AI;

public class EnemyAttackState : EnemyStateBase
{
    private float _lastAttackTime; // 最後に攻撃した時間
    private float _attackCooldown = 2.0f; // 攻撃のクールダウン時間（秒）

    // コンストラクタ
    public EnemyAttackState(EnemyController enemy, EnemyMovement movement) : base(enemy, movement) { }

    public override void Enter()
    {
        
        Debug.Log("AttackState Enter");
    }

    public override void Execute()
    {
        //Attack();

        // HealthPointが0になったら、死亡状態に遷移
        if (_enemy.HealthPoint <= 0)
        {
            _enemy.StateMachine.TransitionTo(_enemy.StateMachine.DieState);
            return;
        }

        // ターゲットが十分に離れたら、追跡状態に遷移
        if(!_movement.IsTargetInAttackRange(_enemy.TargetPosition, _enemy.transform.position))
        {
            _enemy.StateMachine.TransitionTo(_enemy.StateMachine.ChaseState);
            return;
        }

        // 視界にプレイヤーがいないとき、さまよい状態に遷移
        if (!_enemy.IsVisibleTarget)
        {
            _enemy.StateMachine.TransitionTo(_enemy.StateMachine.WanderState);
            return;
        }

        // 攻撃クールダウンが終了している場合に攻撃を実行
        if (Time.time >= _lastAttackTime + _attackCooldown)
        {
            Attack();
            _lastAttackTime = Time.time; // 最後に攻撃した時間を更新
        }
    }

    public override void Exit()
    {
        Debug.Log("AttackState Exit");
    }

    private void Attack()
    {
        //! ターゲットに攻撃を行う処理をここに追加
        // ターゲットの位置を向く
        _movement.LookAtTarget(_enemy.TargetPosition);
        // Attackアニメーションを再生(AnimatorのAttackパラメータをtrueにする)
        _enemy.Animation.PlayAnimation(_enemy.Animation.Attack);
        // 攻撃アニメーションが終了するまで待機
        _enemy.StartCoroutine(_enemy.Animation.WaitForAnimationToEnd(_enemy.Animation.Attack));
        Debug.Log("Attack!");
    }
}
