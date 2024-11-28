using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGameStartState : StageGameStateBase
{
    private IEnumerator _enumerator = null;
    // private float _timeSinceSceneStart;
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

        // Time.timeScale = 0.0f;
        StageGameTimeManager.Pause();
    }

    public override void Execute()
    {
        // 3秒カウントしてプレイ状態に遷移
        if (_enumerator != null && !_enumerator.MoveNext())
        {
            _enumerator = null;
            // Time.timeScale = 1.0f;
            StageGameTimeManager.Resume();
            _stage.StateMachine.TransitionTo(_stage.StateMachine.PlayState);
        }

    }

    // public void Exit()
    // {
    //     Debug.Log("JumpState Exit");
    // }


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
