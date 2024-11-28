using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSight : MonoBehaviour
{
    [SerializeField, Tooltip("プレイヤーの始点位置")]
    private Transform _viewPoint;
    public Transform ViewPoint => _viewPoint;

    private Camera _camera; //カメラの情報を格納

    private void Start()
    {
        //カメラ格納
        _camera = Camera.main;
    }

    private void LateUpdate()
    {
        //カメラの位置調整
        _camera.transform.position = _viewPoint.position;
        //回転
        _camera.transform.rotation = _viewPoint.rotation;
    }

    public void RenewViewPoint(Vector2 mouseInputVector, float clampedVerticalInput)
    {
        //マウスのx軸の動きを反映
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x,
            transform.eulerAngles.y + mouseInputVector.x,
            transform.eulerAngles.z);

        _viewPoint.rotation = Quaternion.Euler(-clampedVerticalInput,
            _viewPoint.transform.rotation.eulerAngles.y,
            _viewPoint.transform.rotation.eulerAngles.z);
    }
}
