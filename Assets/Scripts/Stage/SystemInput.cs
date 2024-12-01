using UnityEngine;

/// <summary>
/// システム系(ポーズ等)の入力を管理するクラス
/// </summary>
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
