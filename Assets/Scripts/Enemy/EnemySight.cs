using UnityEngine;
using MyUtilities;
using System.Runtime.CompilerServices;

public class EnemySight : MonoBehaviour
{
    [Header("視界設定")]
    [SerializeField, Tooltip("視野角（degree）")]
    private float _sightAngle = 60.0f;
    [SerializeField, Tooltip("視界の距離制限")]
    private float _viewMaxDistance = 20.0f;
    [SerializeField, Tooltip("障害物レイヤー")]
    private LayerMask _obstacleLayer;       // 障害物のレイヤーマスク

    public bool CanSeeTarget(Vector3 targetPosition)
    {
        // ターゲットの方向を取得
        Vector3 targetDirection = (targetPosition - transform.position).normalized;

        // 視野角内にターゲットがいるか確認
        if (Vector3.Dot(transform.forward, targetDirection.normalized) > MyMath.Cos(_sightAngle)
            && MyMath.CheckNear(transform.position, targetPosition, _viewMaxDistance))
        {
            //Debug.Log("In View");
            // レイキャストでターゲットとの間に障害物があるなら false
            RaycastHit hit;
            if (Physics.Raycast(transform.position, targetDirection, out hit, _viewMaxDistance, _obstacleLayer))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // 視野角内でない、または障害物がある場合は false
        return false;
    }

    public Vector3 CalculateVectorTo(Vector3 targetPosition)
    {
        return (targetPosition - transform.position).normalized;
    }

    #region Debug
    #if UNITY_EDITOR

    void OnDrawGizmos()
    {
        // Gizmos描画をここに書く
        Gizmos.color = Color.green;

        Vector3 leftBoundary = Quaternion.Euler(0, -_sightAngle / 2, 0) * transform.forward * _viewMaxDistance;
        Vector3 rightBoundary = Quaternion.Euler(0, _sightAngle / 2, 0) * transform.forward * _viewMaxDistance;

        // 中央線（視線）
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * _viewMaxDistance);

        // 左右の視野角を描画
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);

        // 視野の円弧を描画
        int segments = 20;
        Vector3 previousPoint = transform.position + leftBoundary;
        for (int i = 1; i <= segments; i++)
        {
            float angle = -_sightAngle / 2 + (_sightAngle / segments) * i;
            Vector3 nextPoint = Quaternion.Euler(0, angle, 0) * transform.forward * _viewMaxDistance;
            Gizmos.DrawLine(previousPoint, transform.position + nextPoint);
            previousPoint = transform.position + nextPoint;
        }
    }
    #endif
    #endregion

}