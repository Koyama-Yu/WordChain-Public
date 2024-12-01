using UnityEngine;

/// <summary>
/// 入力受付の基底クラス
/// </summary>
public class InputBase : MonoBehaviour
{
    protected bool _canGetInput = false;

    /// <summary>
    /// キーが押されているか(継続)どうか
    /// </summary>
    /// <param name="keys"></param>
    protected bool HasPressedKeys(KeyCode[] keys)
    {
        foreach(KeyCode key in keys)
        {
            if(Input.GetKey(key))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// キーが押された瞬間かどうか
    /// </summary>
    /// <param name="keys"></param>
    protected bool HasPressedKeysDown(KeyCode[] keys)
    {
        foreach(KeyCode key in keys)
        {
            if(Input.GetKeyDown(key))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// スタート時、再開時に入力を受け付けるようにする
    /// </summary>
    public virtual void EnableInput()
    {
        _canGetInput = true;
    }

    /// <summary>
    /// スタート前、ポーズ時に入力を受け付けないようにする
    /// </summary>
    public virtual void DisableInput()
    {
        _canGetInput = false;
    }
}
