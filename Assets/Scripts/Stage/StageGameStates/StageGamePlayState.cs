using UnityEngine;

public class StageGamePlayState : StageGameStateBase
{

    // コンストラクタ
    public StageGamePlayState(StageController stage) : base(stage) { }

    public override void Enter()
    {
        Debug.Log("PlayState Enter");
    }

    public override void Execute()
    {
        // ポーズ入力があればポーズ状態に遷移
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Time.timeScale = 0.0f;
            StageGameTimeManager.Pause();
            _stage.StateMachine.TransitionTo(_stage.StateMachine.PauseState);
        }

        // クリア条件を満たしていればクリア状態に遷移

        // ゲームオーバー条件を満たしていればゲームオーバー状態に遷移
    }

    // public void Exit()
    // {
    //     Debug.Log("JumpState Exit");
    // }
}
