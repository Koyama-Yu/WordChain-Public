using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [Header("アニメーション関連")]
    [SerializeField, Tooltip("Animator")]
    private Animator _animator;
    public Animator Animator => _animator;
    private EnemyAnimatorControllerParams _animatorParams;

    public int Chase => _animatorParams.Chase;
    public int Wander => _animatorParams.Wander;
    public int Attack => _animatorParams.Attack;
    public int Die => _animatorParams.Die;

    private void Awake()
    {
        //_animator = GetComponent<Animator>();
        _animatorParams = new EnemyAnimatorControllerParams();
    }

    /// <summary>
    /// すべてのアニメーションを停止する
    /// </summary>
    public void StopAllAnimation()
    {
        foreach (var param in _animatorParams.AllBools)
        {
            _animator?.SetBool(param, false);
        }
    }

    /// <summary>
    /// 特定のアニメーションを再生する
    /// </summary>
    public void PlayAnimation(int trueParam)
    {
        StopAllAnimation();
        _animator?.SetBool(trueParam, true);
    }

    /// <summary>
    /// Idle状態にする
    /// </summary>
    public void PlayIdleAnimation()
    {
        StopAllAnimation();
    }

    /// <summary>
    /// 指定したアニメーションが終了するまで待機する
    /// </summary>
    public IEnumerator WaitForAnimationToEnd(int animationHash)
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        // 指定されたアニメーションが終了するまで待機
        while (stateInfo.shortNameHash == animationHash && stateInfo.normalizedTime < 1.0f)
        {
            yield return null;
            stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        }

        Debug.Log($"Animation with hash {animationHash} finished.");
    }

    /// <summary>
    /// ポーズ時などにを停止する
    /// </summary>
    public void DisableAnimation()
    {
        _animator.speed = 0f;
    }

    /// <summary>
    /// ポーズ解除時にアニメーションを再開する
    /// </summary>
    public void EnableAnimation()
    {
        _animator.speed = 1.0f;
    }
}
