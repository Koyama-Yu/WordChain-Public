using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerStateBase
{
    // コンストラクタ
    public PlayerWalkState(Player player) : base(player) { }

    // public void Enter()
    // {
    //     Debug.Log("WalkState Enter");
    // }

    public override void Execute()
    {
        // 地に足がついていないときは、ジャンプ状態に遷移
        if (!_player.IsGrounded)
        {
            _player.StateMachine.TransitionTo(_player.StateMachine.JumpState);
            return;
        }

        // 移動していないときは、待機状態に遷移
        if (_player.Movement.MoveDirection == Vector3.zero)
        {
            _player.StateMachine.TransitionTo(_player.StateMachine.IdleState);
            return;
        }

        // ダッシュ中は、ダッシュ状態に遷移
        if(_player.IsDashing)
        {
            _player.StateMachine.TransitionTo(_player.StateMachine.DashState);
            return;
        }

        // スタミナが尽きたら、疲れ状態に遷移
        if (_player.IsTired)
        {
            _player.StateMachine.TransitionTo(_player.StateMachine.TiredState);
            return;
        }

        // HelthPointが0になったら、死亡状態に遷移
        if(_player.HealthPoint <= 0)
        {
            _player.StateMachine.TransitionTo(_player.StateMachine.DieState);
            return;
        }
    }

    // public void Exit()
    // {
    //     Debug.Log("WalkState Exit");
    // }
}
