using UnityEngine;

public class AttackColliderHandler : MonoBehaviour
{
    private EnemyCombat _enemyCombat;

    /// <summary>
    /// 初期化メソッド
    /// </summary>
    public void Initialize(EnemyCombat enemyCombat)
    {
        _enemyCombat = enemyCombat;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_enemyCombat == null) return;

        var damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            // EnemyCombatのダメージ処理を呼び出す
            damageable.TakeDamage(_enemyCombat.Damage);
            _enemyCombat.ApplyKnockback(other);
            Debug.Log($"[AttackColliderHandler] {other.name} took {_enemyCombat.Damage} damage.");
        }
    }
}