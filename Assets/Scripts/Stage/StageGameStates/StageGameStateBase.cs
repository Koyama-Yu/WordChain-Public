using UnityEngine;

public abstract class StageGameStateBase : IState
{
    protected StageController _stage;
    protected string _className;

    // コンストラクタ
    public StageGameStateBase(StageController stage)
    {
        _stage = stage;
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