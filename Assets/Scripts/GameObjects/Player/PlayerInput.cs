using UnityEngine;

public class PlayerInput : InputBase
{
    /// <summary>
    /// プレイヤーへの入力
    /// WASD(or 矢印)で移動, spaceでジャンプ, 左クリックで攻撃
    /// マウスで視点移動
    /// </summary>
    [Header("移動入力"), Tooltip("WASDキーまたは矢印キー等で移動")]
    [SerializeField] private KeyCode[] _forwardKeys = new KeyCode[] {KeyCode.W, KeyCode.UpArrow};
    [SerializeField] private KeyCode[] _backKeys = new KeyCode[] {KeyCode.S, KeyCode.DownArrow};
    [SerializeField] private KeyCode[] _rightKeys = new KeyCode[] {KeyCode.D, KeyCode.RightArrow};
    [SerializeField] private KeyCode[] _leftKeys = new KeyCode[] {KeyCode.A, KeyCode.LeftArrow};

    [Header("ジャンプ入力"), Tooltip("スペースキー等でジャンプ")]
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;

    [Header("ダッシュ入力"), Tooltip("左Shiftキー等でダッシュ")]
    [SerializeField] private KeyCode _dashKey = KeyCode.LeftShift;

    private Vector2 _mouseInputVector; //視点移動に用いるマウスの入力値
    [SerializeField, Tooltip("視点移動の感度")] private float _mouseSensitivity = 1.0f;
    [SerializeField, Tooltip("カメラの回転制限(Y方向)")] private float _cameraRotateLimit = 60.0f;
    private float _verticalMouseInput;   //y軸の回転格納

    //プロパティ
    private Vector3 _keyInputVector;
    private bool _isJumping;
    private bool _isDashing;
    public Vector3 KeyInputVector => _keyInputVector;
    public bool IsJumping {get => _isJumping; set => _isJumping = value;}
    public bool IsDashing {get => _isDashing; set => _isDashing = value;}
    public Vector2 MouseInputVector => _mouseInputVector;
    public float VerticalMouseInput => _verticalMouseInput;

    //入力値
    private float _xInput;
    private float _yInput;
    private float _zInput;


    private void Update()
    {
        if (!_canGetInput) { return; }
        HandleInput();
    }

    /// <summary>
    /// 入力情報の取得
    /// </summary>
    private void HandleInput()
    {
        //inputの初期化
        _xInput = 0;
        _yInput = 0;
        _zInput = 0;

        if(HasPressedMoveKeys(_forwardKeys))
        {
            _zInput++;
        }

        if(HasPressedMoveKeys(_backKeys))
        {
            _zInput--;
        }

        if(HasPressedMoveKeys(_rightKeys))
        {
            _xInput++;
        }

        if(HasPressedMoveKeys(_leftKeys))
        {
            _xInput--;
        }

        _keyInputVector = new Vector3(_xInput, _yInput, _zInput);

        _isJumping = Input.GetKeyDown(_jumpKey);
        _isDashing = Input.GetKey(_dashKey) && _keyInputVector != Vector3.zero;

        GetMouseInputs();
    }

    private bool HasPressedMoveKeys(KeyCode[] keys)
    {
        return base.HasPressedKeys(keys);
    }

    private void GetMouseInputs()
    {
        _mouseInputVector = new Vector2(Input.GetAxisRaw("Mouse X") * _mouseSensitivity,
            Input.GetAxisRaw("Mouse Y") * _mouseSensitivity);

        //y軸の回転制限
        _verticalMouseInput += _mouseInputVector.y;
        _verticalMouseInput = Mathf.Clamp(_verticalMouseInput, -_cameraRotateLimit, _cameraRotateLimit);
    }
    
    public override void DisableInput()
    {
        // 移動(ダッシュ)に関わるinputの初期化
        _xInput = 0;
        _yInput = 0;
        _zInput = 0;

        _isDashing = false;
        _mouseInputVector = Vector2.zero;

        base.DisableInput();
    }
}
