using UnityEngine;

public class PlayerJumpState : PlayerStateBase
{
    // コンストラクタ
    public PlayerJumpState(Player player) : base(player) { }

    // public void Enter()
    // {
    //     Debug.Log("JumpState Enter");
    // }

    public override void Execute()
    {
        // 地に足がついているとき
        if (_player.IsGrounded)
        {
            // 移動していないときは、待機状態に遷移
            if (_player.Movement.MoveDirection == Vector3.zero)
            {
                _player.StateMachine.TransitionTo(_player.StateMachine.IdleState);
                return;
            }
            // 移動しているときは、歩行状態に遷移
            else{
                _player.StateMachine.TransitionTo(_player.StateMachine.WalkState);
                return;
            }
        }

        // スタミナが尽きたら、疲れ状態に遷移
        if (_player.IsTired)
        {
            _player.StateMachine.TransitionTo(_player.StateMachine.TiredState);
            return;
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
    //     Debug.Log("JumpState Exit");
    // }
}
