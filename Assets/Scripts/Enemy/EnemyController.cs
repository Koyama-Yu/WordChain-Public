using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyUtilities;
using NUnit.Framework;
using UnityEngine.UI;
using UniRx;
using System.Runtime.CompilerServices;

public class EnemyController : MonoBehaviour
{

    enum STATE { IDLE, WANDER, CHASE, ATTACK, DEAD };
    STATE state = STATE.IDLE;

    [Header("敵のステータス")]
    [SerializeField, Tooltip("体力")]
    private int _healthPoint = 1;
    public int HealthPoint => _healthPoint;
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

    [Header("追跡対象設定")]
    [SerializeField, Tooltip("視界に入った後の追跡対象")]
    private GameObject _targetForChase; // chaseTargetだと関数名っぽくなるので変更した

    private EnemyStateMachine _stateMachine;
    public EnemyStateMachine StateMachine => _stateMachine;

    private bool _isVisibleTarget;
    public bool IsVisibleTarget => _isVisibleTarget;

    private EnemyMovement _movement;
    private EnemySight _sight;
    private Rigidbody _rigidbody;

    private string _enemyCurrentString = "";
    private HashSet<string> _defeatEnemyWordSet;  // 敵を倒す単語群

    private ResourcesLoader _resourcesLoader;
    [SerializeField, Tooltip("敵を倒す単語群が書かれたファイル名(拡張子なし)")]
    private string _defeatEnemyWordsFileName = "DefeatEnemyWords";

    private Vector3 _resumeVelocity;

    private void Awake()
    {
        Initialize();
        if(_resourcesLoader != null){
            _resourcesLoader.DefeatEnemyWordsLoaded += OnDefeatEnemyWordsLoaded;
        }
    }

    private void Start()
    {
        _stateMachine = new EnemyStateMachine(this, _movement);
        _stateMachine.Initialize(_stateMachine.IdleState);
        _movement.TargetforChase = _targetForChase;
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
        //if (_isPaused) { return; }

        // 視界情報(ターゲットの位置)の取得
        Vector3 targetPosition = _targetForChase.transform.position;
        _isVisibleTarget = _sight.CanSeeTarget(targetPosition);
        //Vector3 vectorToTarget = _sight.CalculateVectorTo(targetPosition);

        // 移動関数の呼び出し
        //_enemyMovement.ChangeSpeed(_isVisibleTarget);
        //_enemyMovement.Move(_isVisibleTarget, vectorToTarget);
        _movement.Move(_isVisibleTarget, targetPosition);

        // ステートマシンの更新
        _stateMachine.Execute();
    }

    public void HasTakenDamage(int damage)
    {
        _healthPoint -= damage;
    }

    public void HasTakenAlphabet(string alphabet)
    {
        _enemyCurrentString += alphabet;
        _enemyBaseStringText.text = _enemyCurrentString;

        // 敵を倒す単語に含まれていたら消滅
        if(_defeatEnemyWordSet.Contains(_enemyCurrentString))
        {
            DestroySelf();
        }
        // 文字列_maxBaseStringLengthを超えたらリセット
        else if(_enemyBaseStringText.text.Length > _maxBaseStringLength)
        {
            ResetEnemyString();
        }
    }

    private void ResetEnemyString()
    {
        _enemyCurrentString = _enemyBaseString;
        _enemyBaseStringText.text = _enemyBaseString;
    }

    public void DestroySelf(){
        StartCoroutine(DestroyAfterDelaySeconds());
    }


    private void Initialize()
    {
        _movement = GetComponent<EnemyMovement>();
        _sight = GetComponent<EnemySight>();
        _rigidbody = GetComponent<Rigidbody>();
        _isVisibleTarget = false;
        _resourcesLoader = FindObjectOfType<ResourcesLoader>();
    }

    private IEnumerator DestroyAfterDelaySeconds()
    {
        yield return new WaitForSeconds(_delaySeconds);
        Destroy(gameObject);
    }

    private void OnDefeatEnemyWordsLoaded(HashSet<string> wordset)
    {
        _defeatEnemyWordSet = wordset;
    }

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
                //Debug.Log(word);
                _defeatEnemyWordSet.Add(trimmedWord);
            }
        }
        catch (System.NullReferenceException e)
        {
            Debug.LogError(e.Message);
            Debug.LogError("ファイル名が間違っているか、ファイルが存在しません");
        }
        
    }

    private void RegisterPauseEvent()
    {
        StageGameTimeManager.OnPaused.Subscribe( _ =>
        {
            // _movement.DisableMovement();
            _rigidbody.Pause(gameObject);
            if(_movement.Agent != null)
            {
                _movement.Agent.isStopped = true;
                _resumeVelocity = _movement.Agent.velocity;
                _movement.Agent.velocity = Vector3.zero;
            }
            //! アニメーションを追加した場合はここにanimator.speed = 0fを追加する
        }).AddTo(this.gameObject);
    }

    private void RegisterResumeEvent()
    {
        StageGameTimeManager.OnResumed.Subscribe( _ =>
        {
            // _movement.DisableMovement();
            _rigidbody.Resume(gameObject);

            if(_movement.Agent != null)
            {
                _movement.Agent.isStopped = false;
                _movement.Agent.velocity = _resumeVelocity;
            }
            //! アニメーションを追加した場合はここにanimator.speed = 0fを追加する
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