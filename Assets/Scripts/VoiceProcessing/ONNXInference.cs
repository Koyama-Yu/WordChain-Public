using System;
//using System.Collections.Generic;
using UnityEngine;
using Unity.Sentis;
//using Unity.Sentis.IWorker;
//using Unity.Barracuda;

public class ONNXInference
{
    private Model encoderModel;
    private Model decoderModel;
    private Worker encoderWorker;
    private Worker decoderWorker;

    // ONNXモデルのパスをコンストラクタで渡す
    public ONNXInference(string encoderModelPath, string decoderModelPath)
    {
        // ONNXモデルをロード
        LoadSentisModels(encoderModelPath, decoderModelPath);
    }

    // Sentisモデルをロードするメソッド
    private void LoadSentisModels(string encoderModelPath, string decoderModelPath)
    {
        try
        {
            // // ModelLoaderでONNXモデルを読み込み
            // encoderModel = ModelLoader.Load(encoderModelPath);
            // decoderModel = ModelLoader.Load(decoderModelPath);

            // モデルを Resources フォルダからロード
            ModelAsset encoderAsset = Resources.Load("onnx/encoder") as ModelAsset;
            ModelAsset decoderAsset = Resources.Load("onnx/decoder") as ModelAsset;

            // モデルをランタイム用に変換
            encoderModel = ModelLoader.Load(encoderAsset);
            decoderModel = ModelLoader.Load(decoderAsset);


            // Workerを作成
            encoderWorker = new Worker(encoderModel, BackendType.GPUCompute);
            decoderWorker = new Worker(decoderModel, BackendType.GPUCompute);

            Debug.Log("Sentisモデルの読み込みが成功しました。");
        }
        catch (Exception ex)
        {
            Debug.LogError("Sentisモデルの読み込み中にエラーが発生しました: " + ex.Message);
        }
    }

    // メルスペクトログラムをもとにSentisで推論を実行する
    public string RunInference(float[][] melSpectrogram)
    {
        if (melSpectrogram == null || melSpectrogram.Length == 0)
        {
            Debug.LogError("メルスペクトログラムが無効です。");
            return null;
        }

        // パディングしてからメルスペクトログラムをSentisが受け取れるTensorに変換
        Tensor melTensor = ConvertToTensor(PadTo3000Frames(melSpectrogram));

        // エンコーダモデルで推論を実行
        Tensor[] encoderOutputs = RunEncoder(melTensor);

        // デコーダモデルで推論を実行
        return RunDecoder(encoderOutputs);
    }

    // メルスペクトログラムをTensorに変換する
    private Tensor ConvertToTensor(float[][] melSpectrogram)
    {
        int numFrames = melSpectrogram.Length;
        int numFeatures = melSpectrogram[0].Length;

        // Tensorの形状は (1, numFeatures, numFrames)
        Tensor<float> melTensor = new Tensor<float>(new TensorShape(1, numFeatures, numFrames), FlattenArray(melSpectrogram));
        for (int i = 0; i < numFrames; i++)
        {
            for (int j = 0; j < numFeatures; j++)
            {
                melTensor[0, j, i] = melSpectrogram[i][j];
            }
        }

        return melTensor;
    }

    // エンコーダモデルで推論を実行
    private Tensor[] RunEncoder(Tensor melTensor)
    {
        // エンコーダに入力データを設定
        encoderWorker.Schedule(melTensor);

        // エンコーダの出力を取得
        var n_layer_cross_k = encoderWorker.PeekOutput("n_layer_cross_k");
        var n_layer_cross_v = encoderWorker.PeekOutput("n_layer_cross_v");

        return new Tensor[] { n_layer_cross_k, n_layer_cross_v };
    }

    // デコーダモデルで推論を実行
    private string RunDecoder(Tensor[] encoderOutputs)
    {
        // デコーダの入力を準備
        Tensor<int> tokens = new Tensor<int>(new TensorShape(1, 1));  // 初期トークン
        tokens[0, 0] = 50256;  // Whisperモデル用の特殊トークン

        // encoderOutputs を Tensor<float> にキャストしてからトリム
        var n_layer_cross_k = encoderOutputs[0] as Tensor<float>;
        var n_layer_cross_v = encoderOutputs[1] as Tensor<float>;

        if (n_layer_cross_k == null || n_layer_cross_v == null)
        {
            throw new InvalidCastException("Encoder outputs could not be cast to Tensor<float>.");
        }

        // エンコーダの出力を期待される形状に合わせてトリム
        Tensor<float> n_layer_cross_k_trimmed = TrimTensor(n_layer_cross_k, 448);  // フレーム数を448に
        Tensor<float> n_layer_cross_v_trimmed = TrimTensor(n_layer_cross_v, 448);


        //デバッグ
        DebugTensorShape(n_layer_cross_k);
        DebugTensorShape(n_layer_cross_v);
        // デコーダに入力データを設定
        decoderWorker.Schedule(tokens, n_layer_cross_k_trimmed, n_layer_cross_v_trimmed);
        //decoderWorker.Schedule(tokens, n_layer_cross_k, n_layer_cross_v);

        // デコーダの出力を取得
        var logits = decoderWorker.PeekOutput();

        // ロジットからトークンを取得
        int maxTokenIndex = GetMaxTokenIndex(logits);

        // トークン列に追加
        tokens[0, 0] = maxTokenIndex;

        // ロジットをテキストに変換（デコーダロジックを実装する必要があります）
        return ConvertLogitsToText(logits);
    }

    // ロジットからテキストに変換する (デコーダロジックを実装)
    private string ConvertLogitsToText(Tensor logits)
    {
        // Whisperモデルなどでは、ロジットからテキストトークンを生成し、テキストに変換するロジックが必要
        // ロジックは具体的なモデルに依存します。ここでは簡易な実装例を示します。
        string generatedText = "推論結果のテキスト";  // ここで実際のロジットをテキストに変換する処理を実装
        return generatedText;
    }

    // メモリの解放
    public void Dispose()
    {
        encoderWorker?.Dispose();
        decoderWorker?.Dispose();
    }

    private float[] FlattenArray(float[][] array2D)
    {
        int numRows = array2D.Length;
        int numCols = array2D[0].Length;
        float[] flattened = new float[numRows * numCols];

        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                flattened[i * numCols + j] = array2D[i][j];
            }
        }

        return flattened;
    }

    private float[][] PadTo3000Frames(float[][] melSpectrogram)
    {
        int targetFrames = 3000;
        int numFrames = melSpectrogram.Length;
        int numFeatures = melSpectrogram[0].Length;

        // フレーム数が3000以上ならそのまま返す
        if (numFrames >= targetFrames)
        {
            return melSpectrogram;
        }

        // 3000フレームになるようにパディング
        float[][] paddedMelSpectrogram = new float[targetFrames][];
        for (int i = 0; i < targetFrames; i++)
        {
            paddedMelSpectrogram[i] = new float[numFeatures];
            if (i < numFrames)
            {
                // 元のデータをコピー
                Array.Copy(melSpectrogram[i], paddedMelSpectrogram[i], numFeatures);
            }
            else
            {
                // 足りない部分をゼロで埋める
                for (int j = 0; j < numFeatures; j++)
                {
                    paddedMelSpectrogram[i][j] = 0f;
                }
            }
        }
        return paddedMelSpectrogram;
    }

    // 指定範囲のテンソルを取得するメソッド
    private Tensor<float> TrimTensor(Tensor<float> originalTensor, int targetLength)
{
    // オリジナルテンソルをReadbackAndCloneしてCPUでアクセス可能にする
    Tensor<float> readableTensor = originalTensor.ReadbackAndClone();

    if (readableTensor == null)
    {
        Debug.LogError("readableTensor is null");
        return null;
    }

    // トリムされたテンソルを新しく作成 (shape: 6, d0, targetLength, 512)
    Tensor<float> trimmedTensor = new Tensor<float>(new TensorShape(originalTensor.shape[0], originalTensor.shape[1], targetLength, originalTensor.shape[3]));

    if (trimmedTensor == null)
    {
        Debug.LogError("trimmedTensor is null");
        return null;
    }

    // オリジナルテンソルから範囲をコピー
    for (int i = 0; i < originalTensor.shape[0]; i++)  // ブロック数
    {
        for (int j = 0; j < originalTensor.shape[1]; j++)  // バッチサイズ
        {
            for (int k = 0; k < targetLength; k++)  // トリムされたコンテキスト長
            {
                for (int m = 0; m < originalTensor.shape[3]; m++)  // アテンション次元
                {
                    trimmedTensor[i, j, k, m] = readableTensor[i, j, k, m];
                }
            }
        }
    }

    return trimmedTensor;
}


    // 最大のトークンIDを取得する処理
    private int GetMaxTokenIndex(Tensor logits)
    {
        // logitsをTensor<float>にキャストしてインデックスアクセスを可能にする
        var logitsTensor = logits as Tensor<float>;

        int maxIndex = 0;
        float maxVal = float.MinValue;

        // logitsの3次元目のサイズを取得
        int numTokens = logitsTensor.shape[2];  // 3次元目のトークン数を取得

        // logitsの各トークンIDに対する確率を確認して、最大のものを取得
        for (int i = 0; i < numTokens; i++)
        {
            float logitValue = logitsTensor[0, 0, i];  // 0, 0, iの要素にアクセス

            if (logitValue > maxVal)
            {
                maxVal = logitValue;
                maxIndex = i;
            }
        }

        return maxIndex;
    }

    private void DebugTensorShape(Tensor tensor)
{
    var shape = tensor.shape;
    string shapeString = "Tensor Shape: (";
    for (int i = 0; i < shape.rank; i++)  // rank はテンソルの次元数
    {
        shapeString += shape[i];
        if (i < shape.rank - 1)
        {
            shapeString += ", ";
        }
    }
    shapeString += ")";
    Debug.Log(shapeString);
}

}



// using System;
// using System.Collections.Generic;
// using Microsoft.ML.OnnxRuntime;
// using Microsoft.ML.OnnxRuntime.Tensors;

// public class ONNXInference
// {
//     private InferenceSession encoderSession;
//     private InferenceSession decoderSession;

//     // ONNXモデルのパスをコンストラクタで渡す
//     public ONNXInference(string encoderModelPath, string decoderModelPath)
//     {
//         // ONNXモデルをロード
//         LoadOnnxModels(encoderModelPath, decoderModelPath);
//     }

//     // ONNXモデルをロードするメソッド
//     private void LoadOnnxModels(string encoderModelPath, string decoderModelPath)
//     {
//         try
//         {
//             encoderSession = new InferenceSession(encoderModelPath);
//             decoderSession = new InferenceSession(decoderModelPath);
//             Console.WriteLine("ONNXモデルの読み込みが成功しました。");
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine("ONNXモデルの読み込み中にエラーが発生しました: " + ex.Message);
//         }
//     }

//     // メルスペクトログラムをもとにONNXモデルで推論を実行する
//     public string RunInference(double[][] melSpectrogram)
//     {
//         if (melSpectrogram == null || melSpectrogram.Length == 0)
//         {
//             Console.WriteLine("メルスペクトログラムが無効です。");
//             return null;
//         }

//         // メルスペクトログラムをONNXが受け取れるTensorに変換
//         var melTensor = ConvertToTensor(melSpectrogram);

//         // エンコーダモデルで推論を実行
//         var encoderOutputs = RunEncoder(melTensor);

//         // デコーダモデルで推論を実行
//         return RunDecoder(encoderOutputs);
//     }

//     // メルスペクトログラムをTensorに変換する
//     private DenseTensor<float> ConvertToTensor(double[][] melSpectrogram)
//     {
//         int numFrames = melSpectrogram.Length;
//         int numFeatures = melSpectrogram[0].Length;
//         var melTensor = new DenseTensor<float>(new[] { 1, numFrames, numFeatures });

//         for (int i = 0; i < numFrames; i++)
//         {
//             for (int j = 0; j < numFeatures; j++)
//             {
//                 melTensor[0, i, j] = (float)melSpectrogram[i][j];
//             }
//         }

//         return melTensor;
//     }

//     // エンコーダモデルで推論を実行
//     private NamedOnnxValue[] RunEncoder(DenseTensor<float> melTensor)
//     {
//         // エンコーダの入力を準備
//         var input = new List<NamedOnnxValue>
//         {
//             NamedOnnxValue.CreateFromTensor("mel", melTensor)
//         };

//         // エンコーダの出力を取得
//         var output = encoderSession.Run(input);
//         var n_layer_cross_k = output[0].AsTensor<float>();
//         var n_layer_cross_v = output[1].AsTensor<float>();

//         return new NamedOnnxValue[]
//         {
//             NamedOnnxValue.CreateFromTensor("n_layer_cross_k", n_layer_cross_k),
//             NamedOnnxValue.CreateFromTensor("n_layer_cross_v", n_layer_cross_v)
//         };
//     }

//     // デコーダモデルで推論を実行
//     private string RunDecoder(NamedOnnxValue[] encoderOutputs)
//     {
//         // デコーダの入力を準備
//         var tokens = new DenseTensor<int>(new[] { 1, 1 });
//         tokens[0, 0] = 50256;  // 初期トークン (Whisperモデル用の特殊トークン)

//         var decoderInputs = new List<NamedOnnxValue>
//         {
//             NamedOnnxValue.CreateFromTensor("tokens", tokens),
//             encoderOutputs[0],
//             encoderOutputs[1],
//         };

//         // デコーダの出力を取得
//         var output = decoderSession.Run(decoderInputs);
//         var logits = output[0].AsTensor<float>();

//         // ロジットをテキストに変換（デコーダロジックを実装する必要があります）
//         return ConvertLogitsToText(logits);
//     }

//     // ロジットからテキストに変換する (デコーダロジックを実装)
//     private string ConvertLogitsToText(Tensor<float> logits)
//     {
//         // Whisperモデルなどでは、ロジットからテキストトークンを生成し、テキストに変換するロジックが必要
//         // ロジックは具体的なモデルに依存します。ここでは簡易な実装例を示します。
//         string generatedText = "推論結果のテキスト";  // ここで実際のロジットをテキストに変換する処理を実装
//         return generatedText;
//     }
// }
