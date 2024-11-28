using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;
using UniRx;

public class AlphabetController : MonoBehaviour
{
    [Header("弾(アルファベットの)基本ステータス")]
    [SerializeField, Tooltip("敵に与えるダメージ")] private int _damagePoint = 1;
    [SerializeField, Tooltip("弾の速度")] private float _shootingSpeed = 5.0f;
    [SerializeField, Tooltip("弾の生存時間")] private float _lifeTime = 3.0f;

    private Rigidbody _rigidbody;
    private float _remainingLifeTime;
    private bool _isPaused = false;

    private void Start()
    {
        //Destroy(this.gameObject, _lifeTime);
        RemoveCloneFromName();

        _rigidbody = GetComponent<Rigidbody>();
        _remainingLifeTime = _lifeTime;

        // ポーズ時の動作を登録
        RegisterPauseEvent();

        // 再開時の動作を登録
        RegisterResumeEvent();
    }

    private void Update()
    {
        if (_isPaused) { return; }

        _remainingLifeTime -= Time.deltaTime;

        if (_remainingLifeTime < 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void MoveVector(Vector3 direction)
    {
        transform.Translate(direction * _shootingSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
            //enemy.HasTakenDamage(_damagePoint);
            enemy.HasTakenAlphabet(gameObject.name);
            Destroy(this.gameObject);
        }
        //Debug.Log("Hit");
    }

    private void RemoveCloneFromName()
    {
        gameObject.name = gameObject.name.Replace("(Clone)", "");
    }

    private void RegisterPauseEvent()
    {
        StageGameTimeManager.OnPaused.Subscribe(_ =>
        {
            _rigidbody.Pause(gameObject);
            _isPaused = true;
        }).AddTo(this.gameObject);

    }

    private void RegisterResumeEvent()
    {
        StageGameTimeManager.OnResumed.Subscribe(_ =>
        {
            _rigidbody.Resume(gameObject);
            _isPaused = false;
        }).AddTo(this.gameObject);
    }
}