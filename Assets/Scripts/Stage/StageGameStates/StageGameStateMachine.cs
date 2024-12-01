using System;

/// <summary>
/// ステージにおけるゲームの状態を管理するステートマシン
/// </summary>
[Serializable]
public class StageGameStateMachine : StateMachine
{

    /// <summary>
    /// ゲーム中(各ステージ)での状況を示すステート
    /// ・StartState: ステージ開始時
    /// ・PlayingState: ゲーム中
    /// ・PauseState: ポーズ時
    /// ・ClearState: クリア時
    /// ・OverState: ゲームオーバー時
    /// </summary>
    
    public StageGameStartState StartState;
    public StageGamePlayState PlayState;
    public StageGamePauseState PauseState;

    // コンストラクタ
    public StageGameStateMachine(StageController stage)
    {
        // 各ステートを生成
        StartState = new StageGameStartState(stage);
        PlayState = new StageGamePlayState(stage);
        PauseState = new StageGamePauseState(stage);
    }

}
