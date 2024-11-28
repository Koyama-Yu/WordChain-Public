// using UnityEngine;

// public class MicrophoneCapture : MonoBehaviour
// {
//     public int sampleRate = 16000;  // Whisperモデルに合わせて16kHzでサンプリング
//     private AudioClip audioClip;
//     private bool isRecording = false;
//     private string currentDeviceName = null;  // 現在使用しているマイクのデバイス名

//     void Start()
//     {
//         StartRecording();
//     }

//     // マイク録音を開始
//     public void StartRecording()
//     {
//         if (Microphone.devices.Length > 0)
//         {
//             currentDeviceName = Microphone.devices[0];  // 最初のデバイスを選択（複数のデバイスがある場合は選択可能）
//             isRecording = true;
//             audioClip = Microphone.Start(currentDeviceName, true, 10, sampleRate);  // 10秒間の録音
//             Debug.Log("録音開始: " + currentDeviceName);
//         }
//         else
//         {
//             Debug.Log("マイクが見つかりません");
//         }
//     }

//     // 録音停止
//     public void StopRecording()
//     {
//         if (isRecording)
//         {
//             Microphone.End(currentDeviceName);
//             isRecording = false;
//             Debug.Log("録音終了: " + currentDeviceName);
//         }
//     }

//     // 録音データが存在するか確認
//     public bool HasAudioData()
//     {
//         return audioClip != null && Microphone.GetPosition(currentDeviceName) > 0;
//     }

//     // 録音データを取得
//     public float[] GetRecordedAudio()
//     {
//         if (audioClip == null) return null;

//         int sampleLength = audioClip.samples * audioClip.channels;
//         float[] audioData = new float[sampleLength];
//         audioClip.GetData(audioData, 0);

//         // 取得後にクリップをリセット
//         audioClip = null;
//         return audioData;
//     }

//     // 現在使用中のマイクデバイス名を取得
//     public string GetCurrentDeviceName()
//     {
//         return currentDeviceName;
//     }

//     // サンプルレートを取得
//     public int GetSampleRate()
//     {
//         return sampleRate;
//     }
// }
