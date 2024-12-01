// Unity公式のデザインパターンのサンプルを参考に作成
// https://github.com/Unity-Technologies/game-programming-patterns-demo

/// <summary>
/// ステートマシンのインターフェース
/// </summary>
public interface IState
{
    /// <summary>
    /// その状態に入った時に最初に実行される処理
    /// </summary>
    void Enter();

    /// <summary>
    /// フレームごとの処理, ステート遷移の条件もここで判定
    /// </summary>
    void Execute();

    /// <summary>
    /// その状態から出る時に最後に実行される処理
    /// </summary>
    void Exit();
}