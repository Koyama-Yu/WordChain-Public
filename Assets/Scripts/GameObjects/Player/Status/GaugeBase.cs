using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲージ系UIの基底クラス
/// </summary>
public abstract class GaugeBase : MonoBehaviour, IGauge
{
    private PlayerStatus _status;
    protected PlayerStatus Status => _status;
    private Slider _slider;

    public virtual void Initialize()
    {
        _slider = GetComponent<Slider>();
        _status = FindObjectOfType<Player>()?.Status;

        // OnValueChangedイベントを発火させずに値を設定
        _slider?.SetValueWithoutNotify(_slider.maxValue);
    }

    public abstract void UpdateGauge();

    public void SetMaxValue(float maxValue)
    {
        _slider.maxValue = maxValue;
    }

    public void SetValue(float value)
    {
        _slider.value = value;
    }

}
