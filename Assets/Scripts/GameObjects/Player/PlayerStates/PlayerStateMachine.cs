using System;

/// <summary>
/// プレイヤーのステートマシン
/// </summary>
[Serializable]
public class PlayerStateMachine : StateMachine
{
    /// <summary>
    /// プレイヤーのステート
    /// ・IdleState: 待機時
    /// ・WalkState: 歩行時
    /// ・DashState: ダッシュ時
    /// ・JumpState: ジャンプ時
    /// ・DamagedState: ダメージを受けた時
    /// ・DieState: 死亡時
    /// </summary>
    public PlayerIdleState IdleState;
    public PlayerWalkState WalkState;
    public PlayerDashState DashState;
    public PlayerTiredState TiredState;
    public PlayerJumpState JumpState;
    public PlayerDieState DieState;

    // コンストラクタ
    public PlayerStateMachine(Player player)
    {
        // 各ステートを生成
        IdleState = new PlayerIdleState(player);
        WalkState = new PlayerWalkState(player);
        DashState = new PlayerDashState(player);
        JumpState = new PlayerJumpState(player);
        TiredState = new PlayerTiredState(player);
        DieState = new PlayerDieState(player);
    }

}
