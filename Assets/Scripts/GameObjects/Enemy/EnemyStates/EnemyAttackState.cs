using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyUtilities;

//! Unused
public class EnemyAttackState : EnemyStateBase
{
    // コンストラクタ
    public EnemyAttackState(EnemyController enemy, EnemyMovement movement) : base(enemy, movement) { }

    // public void Enter()
    // {
    //     Debug.Log("DieState Enter");
    // }

    // public override void Execute()
    // {
    //     // HealthPointが0になったら、死亡状態に遷移
    //     if (_enemy.HealthPoint <= 0)
    //     {
    //         _enemy.StateMachine.TransitionTo(_enemy.StateMachine.DieState);
    //         return;
    //     }

    //     // 視界にプレイヤーがいる場合は、追跡状態に遷移
    //     if (_enemy.IsVisibleTarget)
    //     {
    //         _enemy.StateMachine.TransitionTo(_enemy.StateMachine.ChaseState);
    //         return;
    //     }
    //     // 視界にプレイヤーがいないとき、一定確率でさまよい状態に遷移
    //     else if (MyMath.Probability(0.1f))
    //     {
    //         _enemy.StateMachine.TransitionTo(_enemy.StateMachine.WanderState);
    //         return;
    //     }
    // }

    // public void Exit()
    // {
    //     Debug.Log("DieState Exit");
    // }
}
