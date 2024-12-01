using UnityEngine;

/// <summary>
/// カメラのアスペクト比を保つクラス
/// </summary>
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

    /// <summary>
    /// 目的のアスペクト比と現在のアスペクト比から倍率を算出
    /// </summary>
    /// <returns></returns>
    private float CalculateMagnificationRatio()
    {
        float currentScreenAspcet = Screen.width / (float)Screen.height;
        float targetAspect = _aspectVector.x / _aspectVector.y;
        return targetAspect / currentScreenAspcet;
    }

    /// <summary>
    /// 倍率を用いてカメラのViewportRectを設定
    /// </summary>
    /// <param name="ratio"></param>
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

    /// <summary>
    /// 中央寄せにする際のサイズ(縦幅, 横幅)を算出
    /// </summary>
    /// <param name="size"></param>
    /// <returns>中央寄席されたサイズ</returns>
    
    private float CalculateCenteredValue(float size)
    {
        return 0.5f - size * 0.5f;
    }
}
