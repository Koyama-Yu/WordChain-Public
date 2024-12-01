public class PlayerDashState : PlayerStateBase
{
    // コンストラクタ
    public PlayerDashState(Player player) : base(player) { }


    // public void Enter()
    // {
    //     Debug.Log("DashState Enter");
    // }

    public override void Execute()
    {
        // 地に足がついていないときは、ジャンプ状態に遷移
        if (!_player.IsGrounded)
        {
            _player.StateMachine.TransitionTo(_player.StateMachine.JumpState);
            return;
        }

        // ダッシュしていないときは、歩行状態に遷移
        if (!_player.IsDashing)
        {
            _player.StateMachine.TransitionTo(_player.StateMachine.WalkState);
            return;
        }

        // スタミナが尽きたら、疲れ状態に遷移
        if (_player.IsTired)
        {
            _player.StateMachine.TransitionTo(_player.StateMachine.TiredState);
            return;
        }

        // HelthPointが0になったら、死亡状態に遷移
        if (_player.HealthPoint <= 0)
        {
            _player.StateMachine.TransitionTo(_player.StateMachine.DieState);
            return;
        }

    }

    // public void Exit()
    // {
    //     Debug.Log("DashState Exit");
    // }
}
