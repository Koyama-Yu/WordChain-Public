using UnityEngine;

namespace MyUtilities
{
    /// <summary>
    /// カーソルの表示・非表示を切り替えるクラス
    /// </summary>
    public class CursorManager
    {
        public static void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public static void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public static void UpdateCursorLock(bool lockInput, bool unlockInput)
        {
            if (lockInput)
            {
                LockCursor();
            }
            else if (unlockInput)
            {
                UnlockCursor();
            }
        }
    }
}