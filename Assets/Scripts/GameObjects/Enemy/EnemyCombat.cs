using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 敵の攻撃判定とコライダー制御を担当するクラス
/// </summary>
public class EnemyCombat : MonoBehaviour
{
    [Header("攻撃判定用コライダー")]
    [SerializeField, Tooltip("攻撃判定用のコライダーリスト")]
    private List<Collider> attackColliders = new List<Collider>();

    [SerializeField, Tooltip("アニメーション名とコライダーのインデックスを紐付ける辞書")]
    private SerializableDictionary<string, int> attackColliderDictionary = null;

    [SerializeField, Tooltip("吹き飛ばしの力")]
    private float _knockbackForce = 5.0f;

    private EnemyController _enemy;
    [SerializeField, Tooltip("Animatorのついた3Dモデル")]
    private GameObject _enemyModel;

    private Animator _animator;
    private float _damage;
    public float Damage => _damage;
    
    // 今動いているAnimatorClipを取得する変数
    private AnimatorClipInfo[] _clipInfo;

    private void Awake()
    {
        _enemy = GetComponent<EnemyController>();
        _animator = _enemyModel.GetComponent<Animator>();
        _clipInfo = _animator.GetCurrentAnimatorClipInfo(0);

        _damage = _enemy.AttackDamage; // EnemyControllerから攻撃力を取得

        // 攻撃コライダーにAttackColliderHandlerをアタッチして初期化
        foreach (var collider in attackColliders)
        {
            var handler = collider.gameObject.AddComponent<AttackColliderHandler>();
            handler.Initialize(this);
        }

        DisableAllColliders(); // 初期状態ですべてのコライダーを無効化
    }

    private void Update()
    {
        _clipInfo = _animator.GetCurrentAnimatorClipInfo(0);
    }

    /// <summary>
    /// 指定したアニメーションに対応する攻撃コライダーを有効化
    /// </summary>
    public void EnableAttackCollider(/*string animationName*/)
    {
        // int index = GetColliderIndex(animationName);
        // if (index != -1)
        // {
        //     attackColliders[index].enabled = true;
        //     Debug.Log($"[EnemyCombat] Enabled collider for animation: {animationName}");
        // }
        string animationName = _clipInfo[0].clip.name;
        Debug.Log($"[EnemyCombat] EnableAttackCollider: {animationName}");
        if(attackColliderDictionary.ContainsKey(animationName))
        {
            attackColliders[attackColliderDictionary[animationName]].enabled = true;
        }
    }

    /// <summary>
    /// 指定したアニメーションに対応する攻撃コライダーを無効化
    /// </summary>
    public void DisableAttackCollider(/*string animationName*/)
    {
        // int index = GetColliderIndex(animationName);
        // if (index != -1)
        // {
        //     attackColliders[index].enabled = false;
        //     Debug.Log($"[EnemyCombat] Disabled collider for animation: {animationName}");
        // }
        string animationName = _clipInfo[0].clip.name;
        Debug.Log($"[EnemyCombat] DisableAttackCollider: {animationName}");
        if(attackColliderDictionary.ContainsKey(animationName))
        {
            attackColliders[attackColliderDictionary[animationName]].enabled = false;
        }
    }

    /// <summary>
    /// アニメーション名からコライダーのインデックスを取得
    /// </summary>
    private int GetColliderIndex(string animationName)
    {
        if (attackColliderDictionary != null && attackColliderDictionary.TryGetValue(animationName, out int index))
        {
            if (index >= 0 && index < attackColliders.Count)
            {
                return index;
            }
        }
        return -1;
    }

    /// <summary>
    /// すべての攻撃コライダーを無効化
    /// </summary>
    public void DisableAllColliders()
    {
        foreach (var collider in attackColliders)
        {
            collider.enabled = false;
        }
    }

    /// <summary>
    /// 攻撃コライダーによる接触処理（ダメージ＋吹き飛ばし）
    /// </summary>
    // private void OnTriggerEnter(Collider other)
    // {
    //     var damageable = other.GetComponent<IDamageable>();
    //     if (damageable != null)
    //     {
    //         damageable.TakeDamage(_enemy.AttackDamage);
    //         ApplyKnockback(other);
    //         Debug.Log($"[EnemyCombat] {other.name} took {_enemy.AttackDamage} damage.");
    //     }
    // }

    /// <summary>
    /// 吹き飛ばし処理を実行
    /// </summary>
    public void ApplyKnockback(Collider targetCollider)
    {
        Rigidbody rigidbody = targetCollider.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            Vector3 direction = (targetCollider.transform.position - transform.position).normalized;
            rigidbody.AddForce(direction * _knockbackForce, ForceMode.Impulse);
            Debug.Log($"[EnemyCombat] {targetCollider.name} was knocked back!");
        }
    }

    /// 攻撃アニメーション用のコライダーを有効化（"Attack" を前提）
    /// </summary>
    // public void EnableAttackCollider()
    // {
    //     EnableColliderForAnimation("Attack");
    // }

    /// <summary>
    /// 攻撃アニメーション用のコライダーを無効化（"Attack" を前提）
    /// </summary>
    // public void DisableAttackCollider()
    // {
    //     DisableAttackCollider("Attack");
    // }
}
