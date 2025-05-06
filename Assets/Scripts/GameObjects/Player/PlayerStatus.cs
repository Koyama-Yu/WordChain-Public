using UnityEngine;

/// <summary>
/// プレイヤーのステータス変数を管理するクラス
/// </summary>
[System.Serializable]
public class PlayerStatus : MonoBehaviour
{
    [Header("プレイヤーステータス")]
    [SerializeField]
    private float _healthPoint = 100.0f;
    [SerializeField]
    private float _maxHealthPoint = 100.0f;
    [SerializeField]
    private float _stamina;
    [SerializeField]
    private float _maxStamina;
    [SerializeField, Tooltip("スタミナの回復スピード")]
    private float _staminaIncreaseSpeed;
    [SerializeField, Tooltip("スタミナの減少スピード")]
    private float _staminaDecreaseSpeed;
    [SerializeField]
    private float _shoutPoint;
    [SerializeField]
    private float _maxShoutPoint;
    [SerializeField, Tooltip("叫び(弾)発射時の消費ポイント")]
    private float _shoutPointConsumption;
    [SerializeField, Tooltip("叫びゲージの回復スピード")]
    private float _shoutPointIncreaseSpeed;

    public float HealthPoint  {get => _healthPoint; internal set => _healthPoint = value; }
    public float MaxHealthPoint => _maxHealthPoint;
    public float Stamina { get => _stamina; internal set => _stamina = value; }
    public float MaxStamina => _maxStamina;
    public float StaminaIncreaseSpeed => _staminaIncreaseSpeed;
    public float StaminaDecreaseSpeed => _staminaDecreaseSpeed;
    public float ShoutPoint { get => _shoutPoint; internal set => _shoutPoint = value; }
    public float MaxShoutPoint => _maxShoutPoint;
    public float ShoutPointConsumption => _shoutPointConsumption;
    public float ShoutPointIncreaseSpeed => _shoutPointIncreaseSpeed;
    
    public void Initialize()
    {
        _healthPoint = _maxHealthPoint;
        _stamina = _maxStamina;
        _shoutPoint = _maxShoutPoint;
    }
    
}