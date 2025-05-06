using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class Megaphone : MonoBehaviour
{
    [Header("メガホンの諸設定")]
    [SerializeField, Tooltip("アルファベットのプレハブが入っているフォルダのパス")]
    private string _alphabetPrefabFolerPath;    //! Unused
    private GameObject[] _alphabetPrefabs;
    [SerializeField, Tooltip("アルファベットのプレハブを発射する位置(走ってるとき以外)")]
    private Transform _nonDashNozzle;
    [SerializeField, Tooltip("アルファベットのプレハブを発射する位置(走ってるとき)")]
    private Transform _DashNozzle;

    

    [SerializeField, Tooltip("アルファベットの発射力(走っていない時)")]
    private float _baseForceStrength = 700.0f;
    [SerializeField, Tooltip("アルファベットの発射力(走っている時)")]
    private float _runningForceStrength = 800.0f;

    private MegaphoneInput _megaphoneInput;
    private Player _player;
    private PlayerInput _playerInput;

    private int _selectedAlphabetIndex = 0;
    private char _currentAlpabetCharactor;

    private ResourcesLoader _resourcesLoader;   // Resourcesにあるオブジェクト類をロードするサブジェクト

    [SerializeField, Tooltip("現在選択中のアルファベットを表示するTextUI")]
    private Text _selectedAlphabetText;

    // TODO? のちにマウスホイールのスクロール制御方法を変えるかも
    [SerializeField] private float _wheelScrollCooldown = 0.2f; // 切り替え間隔（秒）
    private float _lastWheelScrollTime = 0.0f;

    private void Awake()
    {
        // TODO のちにResourcesLoaderを使ってのロードを実装(あるいはAssetBundle)
        // _resourcesLoader = FindObjectOfType<ResourcesLoader>();
        // if (_resourcesLoader != null)
        // {
        //     _resourcesLoader.PrefabsLoaded += OnPrefabsLoaded;
        // }
    }

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _playerInput = _player.Input;
        _megaphoneInput = GetComponent<MegaphoneInput>();
        LoadAlphabetPrefabs();

        // ポーズ時の動作を登録
        RegisterPauseEvent();

        // 再開時の動作を登録
        RegisterResumeEvent();
    }

    void Update()
    {
        //デバッグ用
        Debug.DrawRay(transform.position, transform.forward * 10, Color.red);

        // アルファベットの切り替え
        _currentAlpabetCharactor = (char)('A' + _selectedAlphabetIndex);
        SwitchAlphabet();

        // アルファベットの発射
        if (_megaphoneInput.IsShooting)
        {
            Shot();
        }

        // UIの更新
        RenewUI();
    }

    /// <summary>
    /// アルファベットを発射する関数
    /// </summary>
    private void Shot()
    {
        // TODO _playerInputのメンバはなるべくキャッシュして使いたい

        // プレイヤーがダッシュ中かどうかで発射位置と発射力を変える
        Vector3 spawnPosition = _playerInput.IsDashing ? _DashNozzle.position : _nonDashNozzle.position;
        float forceStrength = _playerInput.IsDashing ? _runningForceStrength : _baseForceStrength;

        // アルファベットを生成し、発射
        Quaternion rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
        GameObject alphabet = Instantiate(_alphabetPrefabs[_selectedAlphabetIndex], spawnPosition, rotation);
        alphabet.GetComponent<Rigidbody>().AddForce(_player.Sight.ViewPoint.transform.forward * forceStrength);
    }

    /// <summary>
    /// マウススクロールでアルファベットを切り替える関数(逆向きにスクロールした場合は逆順に切り替える)
    /// </summary>
    private void SwitchAlphabet()
    {
        if (Time.time - _lastWheelScrollTime < _wheelScrollCooldown)
        {
            return;
        }

        //マウススクロールでアルファベットを切り替え
        if (_megaphoneInput.ChangeAlphabetMouseScroll > 0)
        {
            _selectedAlphabetIndex++;
            if (_selectedAlphabetIndex >= _alphabetPrefabs.Length)
            {
                _selectedAlphabetIndex = 0;
            }

            // 最後のスクロール時間を更新
            _lastWheelScrollTime = Time.time;
        }
        //逆向きにスクロールした場合は逆順に切り替え
        else if (_megaphoneInput.ChangeAlphabetMouseScroll < 0)
        {
            _selectedAlphabetIndex--;
            if (_selectedAlphabetIndex < 0)
            {
                _selectedAlphabetIndex = _alphabetPrefabs.Length - 1;
            }

            // 最後のスクロール時間を更新
            _lastWheelScrollTime = Time.time;
        }

        
    }

    /// <summary>
    /// アルファベットのプレハブをロードする関数
    /// </summary>
    private void LoadAlphabetPrefabs()
    {
        // 指定フォルダ内のすべてのプレハブをロード
        // Resources フォルダ内の指定フォルダからすべてのプレハブをロード
        _alphabetPrefabs = Resources.LoadAll<GameObject>("AlphabetPrefabs");

    }

    /// <summary>
    /// UIの更新
    // TODO のちに残弾ゲージなどを追加した場合もここに書く
    /// </summary>
    private void RenewUI()
    {
        //TODO! 一次的に変更しているので後で直す
        if(_currentAlpabetCharactor.ToString() == "D")
        {
            _selectedAlphabetText.text = "O";
        }
        else{
            _selectedAlphabetText.text = _currentAlpabetCharactor.ToString();
        }
    }

    /// <summary>
    /// プレハブのロードが完了した際のコールバック関数
    /// </summary>
    /// <param name="prefabs"></param>
    private void OnPrefabsLoaded(GameObject[] prefabs)
    {
        _alphabetPrefabs = prefabs;
    }

    /// <summary>
    /// プレハブロードのイベント
    //! Unused
    /// </summary>
    private void OnDestroy()
    {
        if (_resourcesLoader != null)
        {
            _resourcesLoader.PrefabsLoaded -= OnPrefabsLoaded;
        }
    }

    /// <summary>
    /// ポーズ時の動作を登録
    /// </summary>
    private void RegisterPauseEvent()
    {
        StageGameTimeManager.OnPaused.Subscribe( _ =>
        {
            _megaphoneInput.DisableInput();
            
        }).AddTo(this.gameObject);
    }

    /// <summary>
    /// 再開時の動作を登録
    /// </summary>
    private void RegisterResumeEvent()
    {
        StageGameTimeManager.OnResumed.Subscribe( _ =>
        {
            _megaphoneInput.EnableInput();

        }).AddTo(this.gameObject);
    }
}
