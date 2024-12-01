using UnityEngine;

/// <summary>
/// プレイヤーの状態における処理を記述する基底クラス
/// </summary>
public abstract class PlayerStateBase : IState
{
    protected Player _player;
    protected string _className;

    // コンストラクタ
    public PlayerStateBase(Player player)
    {
        _player = player;
        _className = this.GetType().Name;
    }

    public virtual void Enter()
    {
        // Debug.Log($"{_className}.Enter is executing.");
    }

    public virtual void Execute()
    {
        Debug.Log($"{_className}.Execute is executing.");
    }

    public virtual void Exit()
    {
        //Debug.Log($"{_className}.Exit is executing.");
    }
}