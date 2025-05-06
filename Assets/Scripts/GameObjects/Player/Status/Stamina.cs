/// <summary>
/// スタミナを管理するクラス
/// </summary>
public class Stamina : GaugeBase
{
    private void Start()
    {
        Initialize();
        SetMaxValue(Status.MaxStamina);
    }

    public override void UpdateGauge()
    {
        SetValue(Status.Stamina);
    }

    private void Update()
    {
        UpdateGauge();
    }

}
