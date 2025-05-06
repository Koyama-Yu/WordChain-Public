using System.Collections;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class StageGameStartState : StageGameStateBase
{
    private GameObject _startUI;
    private Text _startText;
    private IEnumerator _enumerator = null; // カウントダウン用のコルーチン
    public int COUNTDOWN_DURATION = 5;  //TODO : ソースコード外からも変更できるようにしたい

    // コンストラクタ
    public StageGameStartState(StageController stage, GameObject startUI) : base(stage) {
        _startUI = startUI;
        _startText = startUI.GetComponentInChildren<Text>();
    }

    public override void Enter()
    {
        // シーンのロード
        // SceneLoader.LoadScene("Stage1");

        // シーンロード時の経過時間を取得
        //_timeSinceSceneStart = Time.time;

        // Start UIを表示
        _startUI.SetActive(true);

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

    public override void Exit()
    {
        // Start UIを非表示
        _startUI.SetActive(false);
    }


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

            // UIにカウントダウンを表示
            _startText.text = i.ToString();

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
