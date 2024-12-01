using UnityEngine;

/// <summary>
/// 敵の破壊を管理するクラス
//! Unused
/// </summary>
public class DestroyEnemy : MonoBehaviour
{
    private bool _deadJudge;
    
    void Start()
    {
        _deadJudge = true;
    }

    public void DeadEnemy(){
        if(_deadJudge)  _deadJudge = false;
        else    return;

        Invoke("DestroyGameObject", 3.0f);
    }

    private void DestroyGameObject(){
        Destroy(gameObject);
    }
}
