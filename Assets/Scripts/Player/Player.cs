using UnityEngine;
// using UnityEngine.UI;
using UniRx;

//! Unused
// using Unity.VisualScripting;
// using Unity.VisualScripting.Antlr3.Runtime.Tree;
// using UnityEditor.Experimental.GraphView;


[RequireComponent(typeof(PlayerInput), typeof(PlayerMovement), typeof(PlayerSight))]
public class Player : MonoBehaviour
{
    [Header("レイヤー")]
    [SerializeField, Tooltip("障害物判定用レイヤー")]
    private LayerMask _obstacleLayer;
    [SerializeField, Tooltip("地面判定用レイヤー")]
    private LayerMask _groundLayer;
    [SerializeField]
    private Transform _groundCheckPoint; //接地判定用のオブジェクトの位置

    [Header("プレイヤーステータス")]
    [SerializeField]
    private int _healthPoint = 5;
    public int HealthPoint => _healthPoint;
    [SerializeField]
    private int _maxHealthPoint = 5;
    [SerializeField]
    private float _stamina;
    [SerializeField]
    private float _maxStamina;
    [SerializeField, Tooltip("スタミナの回復スピード")]
    private float _staminaIncreaseSpeed;
    [SerializeField, Tooltip("スタミナの減少スピード")]
    private float _staminaDecreaseSpeed;

    [Header("メガホン")]
    [SerializeField]
    private GameObject _megaPhone; //メガホンのプレハブ

    //プレイヤー関連のコンポーネント
    private PlayerInput _input;
    public PlayerInput Input => _input;
    private PlayerMovement _movement;
    public PlayerMovement Movement => _movement;
    private PlayerSight _sight;
    public PlayerSight Sight => _sight;

    private PlayerStateMachine _stateMachine;
    public  PlayerStateMachine StateMachine => _stateMachine;

    //その他のコンポーネント
    private Rigidbody _rigidbody;

    private bool _isTired = false;
    public bool IsTired => _isTired;
    private bool _isGrounded = true;
    public bool IsGrounded => _isGrounded;
    private bool _isDashing = false;
    public bool IsDashing => _isDashing;

    private System.IDisposable _staminaSubscription; //TODO (のちに消す) スタミナ更新用のSubscription

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _healthPoint = _maxHealthPoint;
        _stamina = _maxStamina;
        _input = GetComponent<PlayerInput>();
        _movement = GetComponent<PlayerMovement>();
        _sight = GetComponent<PlayerSight>();
        _rigidbody = GetComponent<Rigidbody>();
        _stateMachine = new PlayerStateMachine(this);

    }

    private void Start()
    {
        _stateMachine.Initialize(_stateMachine.IdleState);

        // ポーズ時の動作を登録
        RegisterPauseEvent();

        // 再開時の動作を登録
        RegisterResumeEvent();

        // TODO ここのスタミナ更新の登録は後で変更
        RegisterStaminaUpdate();
    }

    /// <summary>
    /// スタミナの更新を行うイベントの登録
    /// </summary>
    private void RegisterStaminaUpdate()
    {
        _staminaSubscription = Observable.EveryUpdate()
            .Subscribe( _ =>
            {
                ChangeStamina(_isDashing, _isTired);
            })
            .AddTo(this.gameObject);
    }

    /// <summary>
    /// スタミナの更新を行うイベントの解除
    /// </summary>
    private void DisposeStaminaUpdate()
    {
        _staminaSubscription?.Dispose();
    }

    private void Update()
    {
        //入力情報の取得
        Vector3 keyInputVector = _input.KeyInputVector;
        Vector2 mouseInputVector = _input.MouseInputVector;

        //視点移動関数の呼び出し
        _sight.RenewViewPoint(mouseInputVector, _input.VerticalMouseInput);

        //移動関数の呼び出し
        _movement.ChangeSpeed(_isDashing, _isTired);
        _movement.Move(_input.KeyInputVector);

        //スタミナの変更
        // ChangeStamina(_input.IsDashing, _isTired);
        CheckTiredness();
        
        //接地判定
        _isGrounded = _movement.IsGrounded(_groundCheckPoint, _groundLayer);

        //ダッシュ判定
        _isDashing = _input.IsDashing;

        //接地しているときのみジャンプ可能
        if (_movement.IsGrounded(_groundCheckPoint, _groundLayer))
        {
            //ジャンプ関数の呼び出し
            if(_input.IsJumping)
            {
                _movement.Jump(_rigidbody);
            }
        }

        //ステートマシンの更新
        _stateMachine.Execute();

        //デバッグ用
        Debug.DrawRay(transform.position, transform.forward * 10, Color.blue);
    }

    // HPを渡す
    // TODO これを使わないように変更
    public int GetHealthPoint()
    {
        return _healthPoint;
    }

    // スタミナを渡す
    // TODO これを使わないように変更
    public float GetStamina()
    {
        return _stamina;
    }

    /// <summary>
    ///  プレイヤーの状態においてスタミナの変更を行う
    /// </summary>
    /// <param name="isDashing"></param>
    /// <param name="isTired"></param>
    //TODO のちに変更予定
    private void ChangeStamina(bool isDashing, bool isTired)
    {
        //疲れていないあるいはダッシュしていないときスタミナ回復
        if (isTired || !isDashing) 
        {
            _stamina += Time.deltaTime * _staminaIncreaseSpeed;
            _stamina = Mathf.Min(_stamina, _maxStamina);
        }
        //ダッシュ中はスタミナを減らす
        else if (isDashing)
        {
            _stamina -= Time.deltaTime * _staminaDecreaseSpeed;
            _stamina = Mathf.Max(_stamina, 0f);
        }
    }

    /// <summary>
    /// プレイヤーの疲れ具合をチェックし、_isTiredを更新する
    /// </summary>
    private void CheckTiredness()
    {
        //スタミナが0以下になったら疲れる
        if (_stamina <= 0)
        {
            _isTired = true;
        }
        //スタミナが最大値になったら疲れを解消
        if(_isTired && _stamina >= _maxStamina)
        {
            _isTired = false;
        }
    }

    /// <summary>
    /// ゲームストップ時のイベント登録
    /// </summary>
    private void RegisterPauseEvent()
    {
        StageGameTimeManager.OnPaused.Subscribe( _ =>
        {
            _input.DisableInput();
            _movement.DisableMovement();
            _rigidbody.Pause(gameObject);

            DisposeStaminaUpdate();

            //! アニメーションを追加した場合はここにanimator.speed = 0fを追加する
        }).AddTo(this.gameObject);
    }

    /// <summary>
    /// ゲーム再開時のイベント登録
    /// </summary>
    private void RegisterResumeEvent()
    {
        StageGameTimeManager.OnResumed.Subscribe( _ =>
        {
            _input.EnableInput();
            _movement.EnableMovement();
            _rigidbody.Resume(gameObject);

            RegisterStaminaUpdate();

            //! アニメーションを追加した場合はここにanimator.speed = 1.0fを追加する
        }).AddTo(this.gameObject);
    }

}