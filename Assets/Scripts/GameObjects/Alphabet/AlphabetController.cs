//using System.Numerics;
using UnityEngine;
using UniRx;

public class AlphabetController : MonoBehaviour
{
    [Header("弾(アルファベットの)基本ステータス")]
    [SerializeField, Tooltip("弾の速度")] private float _shootingSpeed = 5.0f;  //! Unused
    [SerializeField, Tooltip("弾の生存時間")] private float _lifeTime = 3.0f;

    private Rigidbody _rigidbody;
    private float _remainingLifeTime;   // アルファベットの残り生存時間
    private bool _isPaused = false;
    private bool _canUseCollision = true;

    private void Start()
    {
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
        // 停止時は処理を行わない
        if (_isPaused) { return; }

        // 残り生存時間を減らす
        _remainingLifeTime -= Time.deltaTime;

        // 生存時間が0以下になったら削除
        if (_remainingLifeTime < 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // 衝突を無効化している場合は処理を行わない
        if (!_canUseCollision) { return; }
        
        // 敵に当たったらその敵にアルファベットを付与
        if (other.gameObject.tag == "Enemy")
        {
            // 衝突を無効化
            //! フラグを使用しないとおそらく処理時間がかかり、衝突判定が続いてしまうのでこの方法で対処
            //? 他に良い方法ある?
            _canUseCollision = false;
            GetComponent<Collider>().enabled = false;
            
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
            enemy.HasTakenAlphabet(gameObject.name);
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// プレハブ生成時にクローンの名前から(Clone)を削除
    /// </summary>
    private void RemoveCloneFromName()
    {
        gameObject.name = gameObject.name.Replace("(Clone)", "");
    }

    /// <summary>
    /// ポーズ時の動作を登録
    /// </summary>
    private void RegisterPauseEvent()
    {
        StageGameTimeManager.OnPaused.Subscribe(_ =>
        {
            _rigidbody.Pause(gameObject);
            _isPaused = true;
        }).AddTo(this.gameObject);

    }

    /// <summary>
    /// 再開時の動作を登録
    /// </summary>
    private void RegisterResumeEvent()
    {
        StageGameTimeManager.OnResumed.Subscribe(_ =>
        {
            _rigidbody.Resume(gameObject);
            _isPaused = false;
        }).AddTo(this.gameObject);
    }
}