/// <summary>
/// ゲージ系UIのインターフェース
/// </summary>
public interface IGauge
{
    /// <summary>
    /// ゲージの初期化
    /// </summary>
    void Initialize();

    /// <summary>
    /// ゲージの更新
    /// </summary>
    void UpdateGauge();

    /// <summary>
    /// ゲージの最大値を設定
    /// </summary>
    void SetMaxValue(float value);

    /// <summary>
    /// ゲージの値を設定
    /// </summary>
    void SetValue(float value);
}