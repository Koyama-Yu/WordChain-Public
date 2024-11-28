using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGamePauseState : StageGameStateBase
{

    // コンストラクタ
    public StageGamePauseState(StageController stage) : base(stage) { }

    public override void Enter()
    {
        // Pasue UIを表示
    }

    public override void Execute()
    {
        // 再びポーズ入力があればプレイ状態に遷移
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //Time.timeScale = 1.0f;
            StageGameTimeManager.Resume();
            _stage.StateMachine.TransitionTo(_stage.StateMachine.PlayState);
        }

    }

    public override void Exit()
    {
        // Pasue UIを非表示
    }
}
