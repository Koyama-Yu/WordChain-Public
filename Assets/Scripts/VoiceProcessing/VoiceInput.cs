// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.Windows.Speech;


// /*
//  * 注意
//  * ・WebGL形式での出力不可
//  * ・「あーーーーーーーーー」という音声を発音しても、「あー」に省略される。
//  * 　（伸ばし棒は省略される）
//  * 
//  * 設定(windows11)
//  * ・機能を使用するには
//  *   設定→プライバシーとセキュリティ→音声認識→オンライン音声認識　をオンにする
//  * 
//  */

// public class VoiceText : MonoBehaviour
// {
//     public Text text_;
//     public DictationRecognizer m_DictationRecognizer;

//     // オブジェクトが破棄されるとき
//     private void OnDestroy()
//     {
//         // 破棄
//         // 下記を記載しないと処理中断時にエラーになる
//         m_DictationRecognizer.Stop();
//         m_DictationRecognizer.Dispose();
//     }

//     void Start()
//     {
//         text_.text = "";

//         m_DictationRecognizer = new DictationRecognizer();

//         /*
//         // 発音終了時
//         //DictationResultのイベントを登録
//         m_DictationRecognizer.DictationResult += (text, confidence) =>
//         {
//             //音声認識した文章はtextで受け取れます。
//             text_.text = text;
//         };
//         */


//         // 発音中
//         m_DictationRecognizer.DictationHypothesis += DictationRecognizer_DictationHypothesis;

//         // 音声入力停止時に再起動
//         m_DictationRecognizer.DictationComplete += (completionCause) =>
//         {
//             if (completionCause == DictationCompletionCause.TimeoutExceeded)
//             {
//                 //音声認識を起動。
//                 m_DictationRecognizer.Start();
//             }
//             else
//             {
//                 //その他止まった原因に応じてハンドリング
//             }
//         };

//         //Dictationを開始
//         m_DictationRecognizer.Start();
//     }

//     private void DictationRecognizer_DictationHypothesis(string text)
//     {
//         text_.text = text;
//     }

// }