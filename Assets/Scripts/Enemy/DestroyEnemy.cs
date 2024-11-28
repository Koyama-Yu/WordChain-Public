using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
