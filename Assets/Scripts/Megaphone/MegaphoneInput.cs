using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

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

    void Start()
    {

    }

    void Update()
    {
        if (!_canGetInput) { return; }
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
