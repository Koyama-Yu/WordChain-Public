using UnityEngine;

/// <summary>
/// カーソルの表示・非表示を切り替えるクラス
/// ! Unused
/// </summary>
public class CursorLocker : Singleton<CursorLocker>
{
    private bool _cursorLock = true;

    void Update()
    {
        UpdateCrusorLock();
    }

    /// <summary>
    /// カーソルの表示・非表示を切り替える
    /// 左クリックでゲームに入ると非表示、ESCキーで表示
    /// </summary>
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
