using UnityEngine;

/// <summary>
/// 敵のUIをカメラに追従させるクラス
/// </summary>
public class EnemyUICameraFollower : MonoBehaviour
{
    void LateUpdate() {
        // カメラ方向を向くように設定
        transform.rotation = Camera.main.transform.rotation;
    }
}
