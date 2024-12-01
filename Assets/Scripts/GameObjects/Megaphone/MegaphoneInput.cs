using UnityEngine;

/// <summary>
/// メガホンの入力を管理するクラス
/// </summary>
public class MegaphoneInput : InputBase
{
    [Header("マウスから射撃系統の入力")]
    [SerializeField, Tooltip("デフォルトではマウス左ボタンのみ")] private KeyCode[] _shotButtons = { KeyCode.Mouse0 };
    [SerializeField, Tooltip("デフォルトではマウス右ボタンのみ")] private KeyCode[] _scopeButtons = { KeyCode.Mouse1 };
    private float _changeAlphabetWheelScroll;
    public float ChangeAlphabetMouseScroll => _changeAlphabetWheelScroll;

    private bool _isShooting;
    public bool IsShooting => _isShooting;
    private bool _isScoping;
    public bool IsScoping => _isScoping;

    void Update()
    {
        // 入力受付不可の場合は何もしない
        if (!_canGetInput) { return; }

        // 入力情報の取得
        _changeAlphabetWheelScroll = Input.GetAxis("Mouse ScrollWheel");
        _isShooting = HasPressedMouseButtons(_shotButtons);
        _isScoping = HasPressedMouseButtons(_scopeButtons);
    }

    private bool HasPressedMouseButtons(KeyCode[] keys)
    {
        return base.HasPressedKeysDown(keys);
    }

    public override void DisableInput()
    {
        base.DisableInput();
    }
}
