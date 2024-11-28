using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesLoader : MonoBehaviour
{
    private GameObject[] _alphabetPrefabs;
    public  GameObject[] AlphabetPrefabs => _alphabetPrefabs;
    [SerializeField, Tooltip("アルファベットのプレハブが入っている「フォルダ」名")]
    private string _alphabetPrefabFolderName = "AlphabetPrefabs";
    [SerializeField, Tooltip("敵を倒す単語群が書かれたファイル名(拡張子なし)")]
    private string _defeatEnemyWordsFileName = "DefeatEnemyWords";
    private HashSet<string> _defeatEnemyWordSet;
    public HashSet<string> DefeatEnemyWordSet => _defeatEnemyWordSet;

    public Action<GameObject[]> PrefabsLoaded;
    public Action<HashSet<string>> DefeatEnemyWordsLoaded;

    private void Awake()
    {
        LoadPrefabs();
        LoadWards();
        PrefabsLoaded?.Invoke(_alphabetPrefabs);
        DefeatEnemyWordsLoaded?.Invoke(_defeatEnemyWordSet);
    }

    private void LoadPrefabs()
    {
        _alphabetPrefabs = Resources.LoadAll<GameObject>(_alphabetPrefabFolderName);
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
                _defeatEnemyWordSet.Add(word);
            }
        }
        catch (System.NullReferenceException e)
        {
            Debug.LogError(e.Message);
            Debug.LogError("ファイル名が間違っているか、ファイルが存在しません");
        }
        
    }


}
