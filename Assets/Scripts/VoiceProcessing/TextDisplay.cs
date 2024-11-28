// using UnityEngine;
// using UnityEngine.UI;

// public class TextDisplay : MonoBehaviour
// {
//     public Text resultText;  // UnityのUI Textコンポーネント
//     public ONNXInference onnxInference;  // 推論クラス
//     public VoiceToText voiceToText;  // メルスペクトログラム変換クラス

//     void Start()
//     {
//         // 音声データをメルスペクトログラムに変換
//         var melSpectrogram = voiceToText.ConvertAudioToMelSpectrogram();

//         // ONNX推論を実行してテキストを生成
//         string recognizedText = onnxInference.RunInference(ConvertDoubleToFloat(melSpectrogram));

//         // 推論結果をUIに表示
//         resultText.text = recognizedText;
//     }

//     // double[][] を float[][] に変換するメソッド
//     private float[][] ConvertDoubleToFloat(double[][] input)
//     {
//         int numFrames = input.Length;
//         float[][] output = new float[numFrames][];
        
//         for (int i = 0; i < numFrames; i++)
//         {
//             int numFeatures = input[i].Length;
//             output[i] = new float[numFeatures];
//             for (int j = 0; j < numFeatures; j++)
//             {
//                 output[i][j] = (float)input[i][j];  // double から float にキャスト
//             }
//         }

//         return output;
//     }
// }

