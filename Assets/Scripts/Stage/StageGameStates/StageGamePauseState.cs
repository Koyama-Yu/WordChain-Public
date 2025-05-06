using UnityEngine;
using MyUtilities;

public class StageGamePauseState : StageGameStateBase
{
    private GameObject _pauseUI;

    // コンストラクタ
    public StageGamePauseState(StageController stage, GameObject pauseUI) : base(stage) {
        _pauseUI = pauseUI;
    }

    public override void Enter()
    {
        // Pasue UIを表示
        _pauseUI.SetActive(true);

        // カーソルを表示
        CursorManager.UnlockCursor();
        
    }

    public override void Execute()
    {
        // 再びポーズ入力されたらプレイ状態に遷移
        // TODO : UIのボタンを押した際にも遷移するようにする
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
        _pauseUI.SetActive(false);
    }
}
