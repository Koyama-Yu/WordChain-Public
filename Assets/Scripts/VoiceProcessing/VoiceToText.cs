// using UnityEngine;
// using UnityEngine.UI;

// public class VoiceToText : MonoBehaviour
// {
//     public Text resultText;  // 推論結果を表示するUI Textコンポーネント
//     public MicrophoneCapture micCapture;  // マイクキャプチャをインスペクタから割り当て
//     private ONNXInference onnxInference;   // 推論モデルクラスを保持

//     void Start()
//     {
//         if (micCapture == null)
//         {
//             // micCapture が設定されていない場合、自動でインスタンス化
//             micCapture = gameObject.AddComponent<MicrophoneCapture>();
//         }

//         // ONNXモデルのパスを設定してインスタンスを作成
//         string encoderModelPath = Application.dataPath + "/onnx/encoder.onnx";
//         string decoderModelPath = Application.dataPath + "/onnx/decoder.onnx";
//         //Debug.Log("Application.dataPath:" + Application.dataPath);
//         onnxInference = new ONNXInference(encoderModelPath, decoderModelPath);

//         // 録音開始
//         micCapture.StartRecording();
//     }

//     void Update()
//     {
//         // 録音データがある場合、処理を実行
//         if (micCapture != null && micCapture.HasAudioData())
//         {
//             // 1. 音声データを取得
//             float[] audioData = micCapture.GetRecordedAudio();

//             // 2. リサンプリング
//             int originalSampleRate = micCapture.GetSampleRate();
//             float[] resampledAudio = AudioResampler.ResampleTo16kHz(audioData, originalSampleRate);

//             // 3. メルスペクトログラムを生成
//             LogMelSpectrogram melSpectrogram = new LogMelSpectrogram();
//             double[][] melData = melSpectrogram.ComputeMelSpectrogram(resampledAudio);

//             // 4. 推論を実行してテキストを生成
//             float[][] melSpectrogramFloat = ConvertDoubleToFloat(melData);
//             string recognizedText = onnxInference.RunInference(melSpectrogramFloat);

//             // 5. 推論結果をUIに表示
//             resultText.text = recognizedText;

//             // 録音終了（必要なら繰り返し処理を追加可能）
//             micCapture.StopRecording();
//         }
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
//                 output[i][j] = (float)input[i][j];
//             }
//         }

//         return output;
//     }
// }
