using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLocker : Singleton<CursorLocker>
{
    private bool _cursorLock = true;

    void Update()
    {
        UpdateCrusorLock();
    }

    private void UpdateCrusorLock()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _cursorLock = false;    //表示
        }
        else if (Input.GetMouseButton(0))
        {
            _cursorLock = true; //非表示
        }

        if (_cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
