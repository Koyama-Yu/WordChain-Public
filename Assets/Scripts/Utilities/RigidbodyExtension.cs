// kan kikuchi 様のブログ記事を参考にしました
//  RigidbodyExtension.cs
//  http://kan-kikuchi.hatenablog.com/entry/Pause_Resume
//
//  Created by kan kikuchi on 2015.11.26.

using UnityEngine;

/// <summary>
/// 一時停止時の速度を保管するクラス
/// </summary>
public class VelocityTmp : MonoBehaviour
{
    //一時停止時の速度
    private Vector3 _angularVelocity;
    private Vector3 _velocity;

    public Vector3 AngularVelocity
    {
        get { return _angularVelocity; }
    }
    public Vector3 Velocity
    {
        get { return _velocity; }
    }

    /// <summary>
    /// Rigidbody2Dを入力して速度を設定する
    /// </summary>
    public void Set(Rigidbody rigidbody)
    {
        _angularVelocity = rigidbody.angularVelocity;
        _velocity = rigidbody.velocity;
    }

}

/// <summary>
/// Rigidbody 型の拡張メソッドを管理するクラス
/// </summary>
public static class RigidbodyExtension
{

    //一時停止時の速度
    private static Vector3 _angularVelocity;
    private static Vector3 _velocity;

    /// <summary>
    /// 一時停止
    /// </summary>
    public static void Pause(this Rigidbody rigidbody, GameObject gameObject)
    {
        gameObject.AddComponent<VelocityTmp>().Set(rigidbody);
        rigidbody.isKinematic = true;
    }

    /// <summary>
    /// 再開
    /// </summary>
    public static void Resume(this Rigidbody rigidbody, GameObject gameObject)
    {
        if (gameObject.GetComponent<VelocityTmp>() == null)
        {
            return;
        }

        // 物理演算を先に再開する
        // 参考元は後にやっていたので適切に動作しなかった
        rigidbody.isKinematic = false;
        rigidbody.velocity = gameObject.GetComponent<VelocityTmp>().Velocity;
        rigidbody.angularVelocity = gameObject.GetComponent<VelocityTmp>().AngularVelocity;

        GameObject.Destroy(gameObject.GetComponent<VelocityTmp>());
    }

}