using UnityEngine;

/// <summary>
/// ステージ全体の管理を行うクラス
/// </summary>
public class StageController : MonoBehaviour
{
    private StageGameStateMachine _stateMachine;
    public StageGameStateMachine StateMachine => _stateMachine;

    private void Awake()
    {
        _stateMachine = new StageGameStateMachine(this);
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
