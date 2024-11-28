using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUICameraFollower : MonoBehaviour
{
    void LateUpdate() {
        // カメラ方向を向くように設定
        transform.rotation = Camera.main.transform.rotation;
    }
}
