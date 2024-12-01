using System.Collections;
using UnityEngine;

public class StageGameStartState : StageGameStateBase
{
    private IEnumerator _enumerator = null; // カウントダウン用のコルーチン
    public int COUNTDOWN_DURATION = 3;

    // コンストラクタ
    public StageGameStartState(StageController stage) : base(stage) { }

    public override void Enter()
    {
        // シーンのロード
        // SceneLoader.LoadScene("Stage1");

        // シーンロード時の経過時間を取得
        //_timeSinceSceneStart = Time.time;

        // コルーチンの作成
        _enumerator = StartCountdown();

        // ゲーム(ステージのオブジェクト類)を一時停止
        StageGameTimeManager.Pause();
    }

    public override void Execute()
    {
        // 3秒カウントしてプレイ状態に遷移
        if (_enumerator != null && !_enumerator.MoveNext())
        {
            _enumerator = null;
            StageGameTimeManager.Resume();
            _stage.StateMachine.TransitionTo(_stage.StateMachine.PlayState);
        }

    }

    // public void Exit()
    // {
    //     Debug.Log("JumpState Exit");
    // }


    /// <summary>
    /// Start時にカウントダウンを行うコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartCountdown()
    {
        for (int i = COUNTDOWN_DURATION; i > 0; i--)
        {
            Debug.Log("カウントダウン: " + i);
            //yield return new WaitForSeconds(1.0f); 

            // WaitForSecondsは使えないので別で経過時間の確認
            float timeSincePriorCountdown = Time.unscaledTime;
            while (Time.unscaledTime - timeSincePriorCountdown < 1.0f)
            {
                yield return null;
            }
        }

        Debug.Log("スタート！");
    }
}
