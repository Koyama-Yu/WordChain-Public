using UnityEngine;

public class PlayerTiredState : PlayerStateBase
{
    // コンストラクタ
    public PlayerTiredState(Player player) : base(player) { }

    // public void Enter()
    // {
    //     Debug.Log("TiredState Enter");
    // }

    public override void Execute()
    {
        // 疲れ状態でないとき
        if (!_player.IsTired)
        {
            // ダッシュしていないときは、歩行状態に遷移
            if (_player.IsDashing)
            {
                _player.StateMachine.TransitionTo(_player.StateMachine.WalkState);
                return;
            }

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
        }

        // HelthPointが0になったら、死亡状態に遷移
        if (_player.Status.HealthPoint <= 0)
        {
            _player.StateMachine.TransitionTo(_player.StateMachine.DieState);
            return;
        }

    }

    // public void Exit()
    // {
    //     Debug.Log("TiredState Exit");
    // }
}
