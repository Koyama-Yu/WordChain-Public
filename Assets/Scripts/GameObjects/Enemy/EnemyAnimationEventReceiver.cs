using UnityEngine;

/// <summary>
/// アニメーションイベントを EnemyController に中継するコンポーネント
/// </summary>
public class EnemyAnimationEventReceiver : MonoBehaviour
{
    private EnemyController _enemy;

    private void Awake()
    {
        _enemy = GetComponentInParent<EnemyController>();
        if (_enemy == null)
        {
            Debug.LogError("EnemyController が親階層に見つかりません！");
        }
    }

    // アニメーションイベントで呼ばれる
    public void OnEnableAttackCollider()
    {
        _enemy?.Combat.EnableAttackCollider();
    }

    public void OnDisableAttackCollider()
    {
        _enemy?.Combat.DisableAttackCollider();
    }
}
