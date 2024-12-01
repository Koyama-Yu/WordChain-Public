using UnityEngine;

namespace MyUtilities
{
    /// <summary>
    /// 数学関連の処理をまとめたクラス
    /// </summary>
    public class MyMath
    {
        public static bool Probability(float probability)
        { 

            return Random.Range(0.0f, 100.0f) < Mathf.Clamp(probability, 0.0f, 100.0f);
        }
        
        public static float GetDistance(Vector3 a, Vector3 b)
        {
            return Vector3.Distance(a, b);
        }

        public static bool CheckNear(Vector3 a, Vector3 b, float distance)
        {
            return GetDistance(a, b) < distance;
        }

        public static float Cos(float degree)
        {
            return Mathf.Cos(degree * Mathf.Deg2Rad);
        }
    }
}
