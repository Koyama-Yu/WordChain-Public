using UnityEngine;

/// <summary>
/// ゴールUIを_rotateSpeedに応じて回転させるクラス
/// </summary>
public class GoalUIRoatator : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 1.0f;

    void Update()
    {
        transform.Rotate(Vector3.up, _rotateSpeed);
    }
}
