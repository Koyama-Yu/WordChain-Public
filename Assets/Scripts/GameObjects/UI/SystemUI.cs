using System.Collections;
using System.Collections.Generic;
//using UnityEditor.EditorTools;
using UnityEngine;

public class SystemUI : MonoBehaviour
{
    //TODO? : 今のところ, 結局SystemUIオブジェクトの配下においているので, SerializeFieldではない方法で取得するかも
    [SerializeField, Tooltip("ゲーム開始時のUIパネル")]
    private GameObject _startUI;
    public GameObject StartUI => _startUI;
    
    [SerializeField, Tooltip("ゲームポーズ時のUIパネル")]
    private GameObject _pauseUI;
    public GameObject PauseUI => _pauseUI;

    [SerializeField, Tooltip("ゲームクリア時のUIパネル")]
    private GameObject _clearUI;
    public GameObject ClearUI => _clearUI;
    
    [SerializeField, Tooltip("ゲームオーバー時のUIパネル")]
    private GameObject _overUI;
    public GameObject OverUI => _overUI;

}
