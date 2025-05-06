using UnityEngine;

// 自動生成された Animator パラメータクラス
public class EnemyAnimatorControllerParams
{
    public readonly int Attack;
    public readonly int Die;
    public readonly int Wander;
    public readonly int Chase;

    public readonly int[] AllBools;

    public EnemyAnimatorControllerParams()
    {
        Attack = Animator.StringToHash("Attack");
        Die = Animator.StringToHash("Die");
        Wander = Animator.StringToHash("Wander");
        Chase = Animator.StringToHash("Chase");
        AllBools = new int[]
        {
            Attack,
            Die,
            Wander,
            Chase
        };

    }
}
