using UnityEngine;

public class EnemyDieState : EnemyStateBase
{
    // コンストラクタ
    public EnemyDieState(EnemyController enemy, EnemyMovement movement) : base(enemy, movement) { }

    // public void Enter()
    // {
    //     Debug.Log("DieState Enter");
    // }

    public override void Execute()
    {
        
        Debug.Log("Enemy is Dead");
        _enemy.DestroySelf();
    }

    // public void Exit()
    // {
    //     Debug.Log("DieState Exit");
    // }
}
