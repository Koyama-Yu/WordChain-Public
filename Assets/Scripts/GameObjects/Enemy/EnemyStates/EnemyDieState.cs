using UnityEngine;

public class EnemyDieState : EnemyStateBase
{
    // コンストラクタ
    public EnemyDieState(EnemyController enemy, EnemyMovement movement) : base(enemy, movement) { }

    public override void Enter()
    {
        // 死亡アニメーションを再生(AnimatorのDieパラメータをtrueにする)
        _enemy.Animation.PlayAnimation(_enemy.Animation.Die);
        Debug.Log("DieState Enter");
    }

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
