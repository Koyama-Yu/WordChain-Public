using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalUIRoatator : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 1.0f;

    void Update()
    {
        transform.Rotate(Vector3.up, _rotateSpeed);
    }
}
