using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;

public class AudioResampler
{
    // 元の音声データ(float[])を16kHzにリサンプリング
    public static float[] ResampleTo16kHz(float[] audioData, int originalSampleRate)
    {
        int targetSampleRate = 16000;

        // オリジナルのサンプルレートがすでに16kHzの場合、リサンプリング不要
        if (originalSampleRate == targetSampleRate)
        {
            return audioData;
        }

        // NAudioでリサンプリングを行うために、WaveFormatを作成
        var inFormat = new WaveFormat(originalSampleRate, 1); // Mono音声として扱う
        var outFormat = new WaveFormat(targetSampleRate, 1);  // 16kHzのMono形式に変換

        // バッファに音声データをセットアップ
        var waveBuffer = new BufferedWaveProvider(inFormat);
        waveBuffer.AddSamples(FloatArrayToByteArray(audioData), 0, audioData.Length * 4);

        // リサンプリングプロセスを開始
        // using (var resampler = new WdlResamplingSampleProvider(waveBuffer.ToSampleProvider(), targetSampleRate))
        // {
        //     float[] resampledAudio = new float[audioData.Length * targetSampleRate / originalSampleRate];
        //     int samplesRead = resampler.Read(resampledAudio, 0, resampledAudio.Length);
        //     return resampledAudio;
        // }
        var resampler = new WdlResamplingSampleProvider(waveBuffer.ToSampleProvider(), targetSampleRate); // IDisposableでないのでusing不要

        float[] resampledAudio = new float[audioData.Length * targetSampleRate / originalSampleRate];
        int samplesRead = resampler.Read(resampledAudio, 0, resampledAudio.Length);

        return resampledAudio;
    }

    // float配列をバイト配列に変換
    private static byte[] FloatArrayToByteArray(float[] floatArray)
    {
        var byteArray = new byte[floatArray.Length * 4];
        for (int i = 0; i < floatArray.Length; i++)
        {
            BitConverter.GetBytes(floatArray[i]).CopyTo(byteArray, i * 4);
        }
        return byteArray;
    }
}


// using NAudio.Wave;
// using System.IO;
// using System;

// public class AudioResampler
// {
//     public static float[] ResampleTo16kHz(string inputFilePath)
//     {
//         using (var reader = new AudioFileReader(inputFilePath))
//         {
//             var outFormat = new WaveFormat(16000, reader.WaveFormat.Channels);
//             using (var resampler = new MediaFoundationResampler(reader, outFormat))
//             {
//                 resampler.ResamplerQuality = 60;  // 高品質リサンプリング
//                 using (var memoryStream = new MemoryStream())
//                 {
//                     WaveFileWriter.WriteWavFileToStream(memoryStream, resampler);
//                     return ConvertWaveToFloatArray(memoryStream.ToArray());
//                 }
//             }
//         }
//     }

//     // バイト配列を浮動小数点数の配列に変換する
//     private static float[] ConvertWaveToFloatArray(byte[] waveData)
//     {
//         var floatArray = new float[waveData.Length / 4];
//         for (int i = 0; i < floatArray.Length; i++)
//         {
//             floatArray[i] = BitConverter.ToSingle(waveData, i * 4);
//         }
//         return floatArray;
//     }
// }
