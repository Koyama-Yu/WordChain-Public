using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class InputBase : MonoBehaviour
{
    protected bool _canGetInput = false;

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

    // スタート時、再開時に入力を受け付けるようにする
    public virtual void EnableInput()
    {
        _canGetInput = true;
    }

    // スタート前、ポーズ時に入力を受け付けないようにする
    public virtual void DisableInput()
    {
        _canGetInput = false;
    }
}
