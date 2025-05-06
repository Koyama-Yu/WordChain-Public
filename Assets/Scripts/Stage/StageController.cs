using UnityEngine;

/// <summary>
/// ステージ全体の管理を行うクラス
/// </summary>
public class StageController : MonoBehaviour
{
    private StageGameStateMachine _stateMachine;
    public StageGameStateMachine StateMachine => _stateMachine;

    private StageEventManager _eventManager;
    public StageEventManager EventManager => _eventManager;

    //TODO : 後にUI専用のクラスを作成し、そのインスタンスを保持するようにする
    // public GameObject PauseUI;
    // public GameObject ClearUI;
    private SystemUI _systemUI;

    private void Awake()
    {
        _systemUI = FindObjectOfType<SystemUI>();   //TODO : 後でうまいこと変更したい
        _eventManager = new StageEventManager();
        _stateMachine = new StageGameStateMachine(this, _systemUI);
    }

    void Start()
    {
        _stateMachine.Initialize(_stateMachine.StartState);
    }

    void Update()
    {
        //ステートマシンの更新
        _stateMachine.Execute();
    }
}
