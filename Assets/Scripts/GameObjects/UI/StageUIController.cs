using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! unused
/// <summary>
/// ステージ全体のUIを管理するクラス
/// </summary>
//! SystemUI, DisplayUIをメンバとし, 適宜UIの表示を切り替えるようにする?
public class StageUIController : MonoBehaviour
{
    [SerializeField] private GameObject _systemUI;
    [SerializeField] private GameObject _displayUI;

    /// <summary>
    /// システムUIの表示 <br>
    /// スタート, ポーズ, クリア, ゲームオーバー時に表示するUI(システムUI)のみ表示する
    /// </summary>
    //TODO : 詳細は後で決める
    public void ShowSystemUI()
    {
        _systemUI.SetActive(true);
        _displayUI.SetActive(false);
    }

    /// <summary>
    /// プレイ中のUIの表示 <br>
    /// 体力等のプレイ中に表示するUI(ディスプレイUI)のみ表示する
    /// </summary>
    //TODO : 詳細は後で決める
    public void ShowDisplayUI()
    {
        _systemUI.SetActive(false);
        _displayUI.SetActive(true);
    }
}
