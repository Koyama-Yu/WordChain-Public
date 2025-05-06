using System;

[Serializable]
public class EnemyStateMachine : StateMachine
{

    /// <summary>
    /// 敵のステート
    /// ・EnemyIdleState: 待機時
    /// ・EnemyWanderState: さまよい時
    /// ・EnemyChaseState: 追跡時
    /// ・EnemyAttackState: 攻撃時
    /// ・EnemyDieState: 死亡時
    /// </summary>
    
    public EnemyIdleState IdleState;
    public EnemyWanderState WanderState;
    public EnemyChaseState ChaseState;
    public EnemyAttackState AttackState;
    public EnemyDieState DieState;


    // コンストラクタ
    public EnemyStateMachine(EnemyController enemy, EnemyMovement movement)
    {
        // 各ステートを生成
        IdleState = new EnemyIdleState(enemy, movement);
        WanderState = new EnemyWanderState(enemy, movement);
        ChaseState = new EnemyChaseState(enemy, movement);
        AttackState = new EnemyAttackState(enemy, movement);
        DieState = new EnemyDieState(enemy, movement);
    }

}
