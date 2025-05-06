using UnityEngine;
using MyUtilities;
using UnityEngine.SceneManagement;

public class StageGameClearState : StageGameStateBase
{
    private GameObject _clearUI;

    // コンストラクタ
    public StageGameClearState(StageController stage, GameObject clearUI) : base(stage) {
        _clearUI = clearUI;
    }

    public override void Enter()
    {
        // Clear UIを表示
        _clearUI.SetActive(true);

        // カーソルを表示
        CursorManager.UnlockCursor();
    }

    public override void Execute()
    {
        //TODO : スペース入力やUIのボタン入力があればゲームタイトルに遷移するようにする
        //! デバッグの都合上タブ入力をすると, Mainシーンを再ロードするようにする
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //SceneManager.LoadScene("Main");
            SceneChanger.StartGame();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            //Time.timeScale = 1.0f;
            // StageGameTimeManager.Resume();
            // _stage.StateMachine.TransitionTo(_stage.StateMachine.PlayState);
            //TODO : のちにインスタンス変数を用いる形に変更
            //SceneManager.LoadScene("Title");
            SceneChanger.ReturnToTitle();
        }

    }

    public override void Exit()
    {
        // Clear UIを非表示
        _clearUI.SetActive(false);
    }
}
