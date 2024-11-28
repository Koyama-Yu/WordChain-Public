using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[ExecuteAlways]
public class AspectKeeper : MonoBehaviour
{
    [SerializeField] private Camera _targetCamera;
    [SerializeField] private Vector2 _aspectVector;
    
    void Start()
    {
        float magnificationRatio = CalculateMagnificationRatio();
        SetCameraViewportRect(magnificationRatio);
    }

    // 目的のアスペクトに対する倍率を算出
    private float CalculateMagnificationRatio()
    {
        float currentScreenAspcet = Screen.width / (float)Screen.height;
        float targetAspect = _aspectVector.x / _aspectVector.y;
        return targetAspect / currentScreenAspcet;
    }

    // 倍率を用いてカメラのViewportRectを設定
    private void SetCameraViewportRect(float ratio)
    {
        var viewportRect = new Rect(0f, 0f, 1.0f, 1.0f);
        if (ratio < 1.0f)   // 使用する横幅を変更
        {
            viewportRect.width = ratio;
            viewportRect.x = CalculateCenteredValue(viewportRect.width);
        }
        else    // 倍率が1.0より大きいときは縦幅の方を狭める
        {
            viewportRect.height = 1.0f / ratio;
            viewportRect.y = CalculateCenteredValue(viewportRect.height);
        }
        
        _targetCamera.rect = viewportRect;  //倍率計算したViewportRectをカメラに設定
    }

    // 中央寄せにする際のサイズ(縦幅, 横幅)を算出
    private float CalculateCenteredValue(float size)
    {
        return 0.5f - size * 0.5f;
    }
}
