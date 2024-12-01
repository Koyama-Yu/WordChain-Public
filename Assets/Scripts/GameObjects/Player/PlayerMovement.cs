using System.Collections;
using System.Collections.Generic;
//using UnityEditor.EditorTools;
using UnityEngine;

/// <summary>
/// プレイヤーの移動を制御するクラス
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("移動")]
    [SerializeField, Tooltip("通常スピード")] private float _walkSpeed;
    [SerializeField, Tooltip("ダッシュスピード")] private float _dashSpeed;
    [SerializeField, Tooltip("スタミナ切れスピード")] private float _tiredSpeed;

    [Header("ジャンプ")]
    [SerializeField, Tooltip("ジャンプ力")] private const float _jumpPower = 6.0f;
    [SerializeField, Tooltip("接地判定用の距離")] private float _groundCheckOffset = 0.25f;
    private Vector3 _jumpForce = new Vector3(0, _jumpPower, 0);

    private Vector3 _moveDirection;   //進む方向格納
    public Vector3 MoveDirection => _moveDirection;

    private float _currentSpeed = 0.0f;
    public float CurrentSpeed => _currentSpeed;

    private bool _canMove = false;

    private void Update()
    {
        //Debug.Log(_currentSpeed);
    }

    /// <summary>
    /// 水平方向(前後左右)の移動
    /// </summary>
    /// <param name="inputVector"></param>
    public void Move(Vector3 inputVector)
    {
        if (!_canMove) { return; }
        
        //移動方向の計算
        _moveDirection = ((transform.forward * inputVector.z) + (transform.right * inputVector.x)).normalized;
        //デバッグ用
        Debug.DrawRay(transform.position, _moveDirection, Color.green);

        //現在位置に反映
        transform.position += _moveDirection * _currentSpeed * Time.deltaTime;
    }

    /// <summary>
    /// rigidbodyに力を加えてジャンプ
    /// </summary>
    /// <param name="rigidbody"></param>
    public void Jump(Rigidbody rigidbody)
    {
        rigidbody.AddForce(_jumpForce, ForceMode.Impulse);
    }

    /// <summary>
    /// 接地しているかどうかをRaycast判定
    /// </summary>
    /// <param name="groundCheckPoint"></param>
    /// <param name="groundLayer"></param>
    public bool IsGrounded(Transform groundCheckPoint, LayerMask groundLayer)
    {
        return Physics.Raycast(groundCheckPoint.position, Vector3.down, _groundCheckOffset, groundLayer);
    }

    //TODO 後からここはこれは変更する予定(引数含め)
    public void ChangeSpeed(bool isDashing, bool isTired)
    {
        if (isTired)
        {
            _currentSpeed = _tiredSpeed;
        }
        else if (isDashing)
        {
            _currentSpeed = _dashSpeed;
        }
        else
        {
            _currentSpeed = _walkSpeed;
        }
    }

    /// <summary>
    /// 移動を可能にする
    /// </summary>
    public void EnableMovement()
    {
        _canMove = true;
    }

    /// <summary>
    /// 移動を不可能にする
    /// </summary>
    public void DisableMovement()
    {
        _canMove = false;
    }
}
