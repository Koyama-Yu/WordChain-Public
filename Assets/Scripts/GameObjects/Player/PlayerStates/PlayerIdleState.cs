using UnityEngine;

public class PlayerIdleState : PlayerStateBase
{
    // コンストラクタ
    public PlayerIdleState(Player player) : base(player) { }

    // public void Enter()
    // {
    //     Debug.Log("IdleState Enter");
    // }

    public override void Execute()
    {
        // 地に足がついていないときは、ジャンプ状態に遷移
        if (!_player.IsGrounded)
        {
            _player.StateMachine.TransitionTo(_player.StateMachine.JumpState);
            return;
        }

        // 移動しているときは、歩行状態に遷移
        if (_player.Movement.MoveDirection != Vector3.zero)
        {
            _player.StateMachine.TransitionTo(_player.StateMachine.WalkState);
            return;
        }

        // HealthPointが0になったら、死亡状態に遷移
        if (_player.HealthPoint <= 0)
        {
            _player.StateMachine.TransitionTo(_player.StateMachine.DieState);
            return;
        }
    }

    // public void Exit()
    // {
    //     Debug.Log("IdleState Exit");
    // }
}
