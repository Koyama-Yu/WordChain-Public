using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーのHPを管理するクラス
/// </summary>
//TODO? のちにMVPのModelに変更予定するかも
public class Health : GaugeBase
{
    private void Start()
    {
        Initialize();
        SetMaxValue(Status.MaxHealthPoint);
    }

    public override void UpdateGauge()
    {
        SetValue(Status.HealthPoint);
    }

    private void Update()
    {
        UpdateGauge();
    }

}

