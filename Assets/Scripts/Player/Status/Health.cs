using System.Collections;
using System.Collections.Generic;
//using Microsoft.Unity.VisualStudio.Editor;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField, Header("HPアイコン")]
    private GameObject _healthPointIcon;

    private Player _player;
    private int _preHealthPoint;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _preHealthPoint = _player.GetHealthPoint();
        CreateHealthPointIcon();
    }

    private void CreateHealthPointIcon(){
        for(int i = 0; i< _player.GetHealthPoint(); i++){
            GameObject hpIconObject = Instantiate(_healthPointIcon);
            hpIconObject.transform.parent = transform;
        }
    }

    void Update()
    {
        ShowHPIcon();
    }

    private void ShowHPIcon(){
        int nowHealthPoint = _player.GetHealthPoint();

        if(_preHealthPoint == nowHealthPoint) return;

        //Image型を持っている子オブジェクトを全て取得
        UnityEngine.UI.Image[] healthPointIcons = transform.GetComponentsInChildren<UnityEngine.UI.Image>();
        for(int i = 0; i < healthPointIcons.Length; i++){
            healthPointIcons[i].gameObject.SetActive(i < nowHealthPoint);
        }
        _preHealthPoint = nowHealthPoint;
    }
}
