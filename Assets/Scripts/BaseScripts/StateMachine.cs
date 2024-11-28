using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StateMachine
{
    public IState CurrentState { get; protected set; }

    // 他のオブジェクトに状態変化を通知するためのイベント
    public event Action<IState> StateChanged;

    public void Initialize(IState state)
    {
        CurrentState = state;
        state.Enter();

        // 状態変化を通知
        StateChanged?.Invoke(state);
    }

    /// <summary>
    /// 状態遷移
    /// </summary>
    /// <param name="nextState"></param>
    public void TransitionTo(IState nextState)
    {
        CurrentState.Exit();
        CurrentState = nextState;
        nextState.Enter();

        // 状態変化を通知
        StateChanged?.Invoke(nextState);
    }

    /// <summary>
    /// 現在の状態を実行
    /// </summary>
    public void Execute()
    {
        if (CurrentState != null)
        {
            CurrentState.Execute();
        }
    }
}
