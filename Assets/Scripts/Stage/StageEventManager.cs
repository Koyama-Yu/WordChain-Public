using System;
using UniRx;

public class StageEventManager
{
    private readonly Subject<Unit> _stageClearedSubject = new Subject<Unit>();
    private readonly Subject<Unit> _gameOverSubject = new Subject<Unit>();

    // サブスクライブ用のの公開プロパティ
    public IObservable<Unit> OnStageCleared => _stageClearedSubject;
    public IObservable<Unit> OnGameOver => _gameOverSubject;

    // ステージクリアイベントを発火
    public void OnTriggerStageCleared() => _stageClearedSubject.OnNext(Unit.Default);
    
    // ゲームオーバーイベントを発火
    public void OnTriggerGameOver() => _gameOverSubject.OnNext(Unit.Default);
}
