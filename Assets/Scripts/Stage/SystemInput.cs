using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class SystemInput : InputBase
{
    [Header("システム系(ポーズ等)の入力")]
    [SerializeField, Tooltip("デフォルトではTabのみ")] private KeyCode[] _pauseKeyss = { KeyCode.Tab };

    private bool _isPausing;
    public bool IsPausing => _isPausing;


    void Update()
    {
        _isPausing = HasPressedMouseButtons(_pauseKeyss);
    }

    private bool HasPressedMouseButtons(KeyCode[] keys)
    {
        return base.HasPressedKeysDown(keys);
    }
}
