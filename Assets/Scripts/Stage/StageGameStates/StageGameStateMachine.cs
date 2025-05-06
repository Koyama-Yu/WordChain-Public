using System;
using UniRx;
//using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// ステージにおけるゲームの状態を管理するステートマシン
/// </summary>
[Serializable]
public class StageGameStateMachine : StateMachine
{
    //private StageController _stage;

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
    public StageGameClearState ClearState;

    // コンストラクタ
    //TODO! UI系の引数部分はのちに変更予定
    public StageGameStateMachine(StageController stage, SystemUI systemUI)
    {
        //_stage = stage;

        // 各ステートを生成
        StartState = new StageGameStartState(stage, systemUI.StartUI);
        PlayState = new StageGamePlayState(stage);
        PauseState = new StageGamePauseState(stage, systemUI.PauseUI);
        ClearState = new StageGameClearState(stage, systemUI.ClearUI);

        // ステージイベントの登録
        //TODO! 現在はクリアイベントのみ登録, のちにもっとうまいことやる
        RegisterStageEvents(stage, systemUI.ClearUI);
    }

    /// <summary>
    /// ステージイベントの登録
    /// </summary>
    private void RegisterStageEvents(StageController stage, GameObject clearUI)
    {
        // ステージクリアイベントの登録
        stage.EventManager.OnStageCleared
            .Subscribe( _ => TransitionTo(ClearState))
            .AddTo(clearUI);

        // ゲームオーバーイベントの登録
    //     StageEventManager.Instance.OnGameOver
    //         .Subscribe(_ => TransitionTo(OverState))
    //         .AddTo(this);
    }

}
