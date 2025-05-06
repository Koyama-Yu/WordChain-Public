//! unused
// using UniRx;
// using UnityEngine;

// public class GameOverUI : MonoBehaviour
// {
//     [SerializeField] private GameObject _gameOverPanel;

//     private void Start()
//     {
//         StageEventManager.Instance.OnGameOver
//             .Subscribe( _ => ShowGameOverUI())
//             .AddTo(this); // 購読解除を自動化
//     }

//     private void ShowGameOverUI()
//     {
//         _gameOverPanel.SetActive(true);
//     }
// }
