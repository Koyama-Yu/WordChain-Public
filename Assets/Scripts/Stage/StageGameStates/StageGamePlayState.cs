using UnityEngine;
using MyUtilities;

public class StageGamePlayState : StageGameStateBase
{

    // コンストラクタ
    public StageGamePlayState(StageController stage) : base(stage) { }

    public override void Enter()
    {
        Debug.Log("PlayState Enter");

        // カーソルを非表示
        CursorManager.LockCursor();
    }

    public override void Execute()
    {
        // 入力によってカーソルの表示・非表示を切り替える
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CursorManager.UnlockCursor();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            CursorManager.LockCursor();
        }
        
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
