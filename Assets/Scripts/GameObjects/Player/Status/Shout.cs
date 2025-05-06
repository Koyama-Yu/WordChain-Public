/// <summary>
/// 叫びゲージを管理するクラス
/// </summary>
public class Shout : GaugeBase
{
    private void Start()
    {
        Initialize();
        SetMaxValue(Status.MaxShoutPoint);
    }

    public override void UpdateGauge()
    {
        SetValue(Status.ShoutPoint);
    }

    private void Update()
    {
        UpdateGauge();
    }

}
