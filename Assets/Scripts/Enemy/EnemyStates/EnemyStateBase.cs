using UnityEngine;

public abstract class EnemyStateBase : IState
{
    protected EnemyController _enemy;
    protected EnemyMovement _movement;
    protected string _className;

    // コンストラクタ
    public EnemyStateBase(EnemyController enemy, EnemyMovement movement)
    {
        _enemy = enemy;
        _movement = movement;
        _className = this.GetType().Name;
    }

    public virtual void Enter()
    {
        Debug.Log($"{_className}.Enter is executing.");
    }

    public virtual void Execute()
    {
        Debug.Log($"{_className}.Execute is executing.");
    }

    public virtual void Exit()
    {
        Debug.Log($"{_className}.Exit is executing.");
    }
}