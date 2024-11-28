using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
