using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class EnemyController : MonoBehaviour
{
    [Header("敵のステータス")]
    [SerializeField, Tooltip("体力")]
    private int _healthPoint = 1;
    public int HealthPoint => _healthPoint;
    [SerializeField, Tooltip("敵の攻撃力")]
    private float _attackDamage = 10.0f;
    public float AttackDamage => _attackDamage;
    [SerializeField, Tooltip("敵が死亡してから消滅するまでの時間")]
    private float _delaySeconds = 1.0f;
    [SerializeField, Tooltip("敵の固有文字列")]
    private string _enemyBaseString;
    [SerializeField, Tooltip("敵の固有文字列を表示させるTextUI")]
    private Text _enemyBaseStringText;
    [SerializeField, Tooltip("最大固有文字列長(この長さになったらリセット)")]
    private int _maxBaseStringLength = 6;

    [Header("敵の移動")]
    [SerializeField, Tooltip("徘徊時の移動速度")]
    public float _wanderSpeed = 1.0f;
    [SerializeField, Tooltip("追跡時の移動速度")]
    public float _chaseSpeed = 3.0f;

    [Header("追跡・攻撃対象設定")]
    [SerializeField, Tooltip("視界に入った後の追跡・攻撃対象")]
    private GameObject _target;
    private Vector3 _targetPosition;    // _targetForChaseの位置
    public Vector3 TargetPosition => _targetPosition;

    private EnemyStateMachine _stateMachine;
    public EnemyStateMachine StateMachine => _stateMachine;

    private bool _isVisibleTarget;
    public bool IsVisibleTarget => _isVisibleTarget;

    [Header("コンポーネント")]
    private EnemyMovement _movement;
    public EnemyMovement Movement => _movement;
    private EnemyAnimation _animation;
    public EnemyAnimation Animation => _animation;
    private EnemyCombat _combat;
    public EnemyCombat Combat => _combat;
    private EnemySight _sight;
    private Rigidbody _rigidbody;

    private string _enemyCurrentString = "";
    private HashSet<string> _defeatEnemyWordSet;  // 敵を倒す単語群

    private ResourcesLoader _resourcesLoader;   //! Unused
    [SerializeField, Tooltip("敵を倒す単語群が書かれたファイル名(拡張子なし)")]
    private string _defeatEnemyWordsFileName = "DefeatEnemyWords";

    private void Awake()
    {
        Initialize();
        if(_resourcesLoader != null){
            _resourcesLoader.DefeatEnemyWordsLoaded += OnDefeatEnemyWordsLoaded;
        }
    }

    private void Initialize()
    {
        _movement = GetComponent<EnemyMovement>();
        _animation = GetComponent<EnemyAnimation>();
        _sight = GetComponent<EnemySight>();
        _combat = GetComponent<EnemyCombat>();
        _rigidbody = GetComponent<Rigidbody>();
        _isVisibleTarget = false;
        _resourcesLoader = FindObjectOfType<ResourcesLoader>();
    }

    private void Start()
    {
        _stateMachine = new EnemyStateMachine(this, _movement);
        _stateMachine.Initialize(_stateMachine.IdleState);
        _enemyBaseStringText.text = _enemyBaseString;
        _enemyCurrentString = _enemyBaseString;
        LoadWards();

        // ポーズ時の動作を登録
        RegisterPauseEvent();

        // 再開時の動作を登録
        RegisterResumeEvent();
    }

    private void Update()
    {
        // 視界情報(ターゲットの位置)の取得
        _targetPosition = _target.transform.position;
        _isVisibleTarget = _sight.CanSeeTarget(_targetPosition);

        // 移動関数の呼び出し
        //_movement.Move(_isVisibleTarget, targetPosition);

        // ステートマシンの更新
        _stateMachine.Execute();
    }

    /// <summary>
    /// 敵がアルファベットを取得したときの処理
    /// </summary>
    /// <param name="alphabet"></param>
    public void HasTakenAlphabet(string alphabet)
    {
        // 取得したアルファベットを固有文字列に追加
        _enemyCurrentString += alphabet;
        _enemyBaseStringText.text = _enemyCurrentString;

        // 敵を倒す単語に含まれていたらHPを減らす
        if(_defeatEnemyWordSet.Contains(_enemyCurrentString))
        {
            _healthPoint--;
            //DestroySelf();
        }
        // 文字列_maxBaseStringLengthを超えたらリセット
        else if(_enemyBaseStringText.text.Length > _maxBaseStringLength)
        {
            ResetEnemyString();
        }
    }

    /// <summary>
    /// 敵の固有文字列をリセットする
    /// </summary>
    private void ResetEnemyString()
    {
        _enemyCurrentString = _enemyBaseString;
        _enemyBaseStringText.text = _enemyBaseString;
    }

    /// <summary>
    /// コルーチンを使って一定時間後に自信を破棄する
    /// </summary>
    public void DestroySelf(){
        StartCoroutine(DestroyAfterDelaySeconds());
    }

    /// <summary>
    /// 一定時間後に自信を破棄するコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator DestroyAfterDelaySeconds()
    {
        yield return new WaitForSeconds(_delaySeconds);
        Destroy(gameObject);
    }

    /// <summary>
    /// 敵を倒す単語を読み込んだときの処理
    /// </summary>
    /// <param name="wordset"></param>
    private void OnDefeatEnemyWordsLoaded(HashSet<string> wordset)
    {
        _defeatEnemyWordSet = wordset;
    }

    /// <summary>
    /// 敵を倒せる単語群を読み込む
    //! Unused
    /// </summary>
    private void LoadWards()
    {
        TextAsset textAsset = Resources.Load(_defeatEnemyWordsFileName, typeof(TextAsset)) as TextAsset;

        try{
            _defeatEnemyWordSet = new HashSet<string>();
            string[] words = textAsset.text.Split('\n');
            foreach (string word in words)
            {
                string trimmedWord = word.Trim();
                if (string.IsNullOrEmpty(trimmedWord))
                {
                    continue;
                }
                _defeatEnemyWordSet.Add(trimmedWord);
            }
        }
        catch (System.NullReferenceException e)
        {
            Debug.LogError(e.Message);
            Debug.LogError("ファイル名が間違っているか、ファイルが存在しません");
        }
        
    }

    /// <summary>
    /// ポーズ時の動作を登録
    /// </summary>
    private void RegisterPauseEvent()
    {
        StageGameTimeManager.OnPaused.Subscribe( _ =>
        {
            _movement.DisableMovement();
            _rigidbody.Pause(gameObject);
            _animation.DisableAnimation();
        }).AddTo(this.gameObject);
    }

    /// <summary>
    /// 再開時の動作を登録
    /// </summary>
    private void RegisterResumeEvent()
    {
        StageGameTimeManager.OnResumed.Subscribe( _ =>
        {
            _movement.EnableMovement();
            _rigidbody.Resume(gameObject);
            _animation.EnableAnimation();
        }).AddTo(this.gameObject);
    }

    #if DEBUG
    // 視界判定の結果をGUI出力
    private void OnGUI()
    {
        // 結果表示
        GUI.Box(new Rect(20, 20, 150, 23), $"isVisible = {_isVisibleTarget}");
    }
    #endif
}