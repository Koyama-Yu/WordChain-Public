using System;
using MathNet.Numerics;
using MathNet.Numerics.IntegralTransforms;

public class LogMelSpectrogram
{
    public int SampleRate { get; private set; }
    public int NumMelBands { get; private set; }
    public int NFFT { get; private set; }

    public LogMelSpectrogram(int sampleRate = 16000, int numMelBands = 80, int nfft = 400)
    {
        SampleRate = sampleRate;
        NumMelBands = numMelBands;
        NFFT = nfft;
    }

    // メル・スペクトログラムを計算する関数
    public double[][] ComputeMelSpectrogram(float[] audioSamples)
    {
        //int hopLength = NFFT / 2;  // ハンニング窓を使用
        int hopLength = 160;  // ハンニング窓を使用
        int numFrames = (audioSamples.Length - NFFT) / hopLength + 1;

        // パワースペクトログラムの計算
        double[][] powerSpectrogram = new double[numFrames][];
        for (int i = 0; i < numFrames; i++)
        {
            float[] frame = new float[NFFT];
            Array.Copy(audioSamples, i * hopLength, frame, 0, NFFT);

            // 窓関数（ハンニング窓）をかける
            var window = Window.Hann(NFFT);
            for (int j = 0; j < NFFT; j++)
            {
                frame[j] *= (float)window[j];
            }

            // FFTを適用
            Complex32[] fftResult = new Complex32[NFFT];
            for (int j = 0; j < NFFT; j++)
            {
                fftResult[j] = new Complex32(frame[j], 0);
            }
            Fourier.Forward(fftResult, FourierOptions.Matlab);

            // パワースペクトログラムを計算
            powerSpectrogram[i] = new double[NFFT / 2 + 1];
            for (int j = 0; j <= NFFT / 2; j++)
            {
                powerSpectrogram[i][j] = fftResult[j].MagnitudeSquared();
            }
        }

        // メルフィルタバンクを適用してメルスペクトログラムを生成
        double[][] melSpectrogram = ApplyMelFilterBank(powerSpectrogram);

        // 対数変換
        for (int i = 0; i < melSpectrogram.Length; i++)
        {
            for (int j = 0; j < melSpectrogram[i].Length; j++)
            {
                melSpectrogram[i][j] = Math.Log(melSpectrogram[i][j] + 1e-10);  // 対数スケール
            }
        }

        return melSpectrogram;
    }

    // メルフィルタバンクを適用する関数
    private double[][] ApplyMelFilterBank(double[][] powerSpectrogram)
    {
        // メルフィルタバンクを作成
        double[][] melFilterBank = CreateMelFilterBank();
        double[][] melSpectrogram = new double[powerSpectrogram.Length][];
        for (int i = 0; i < powerSpectrogram.Length; i++)
        {
            melSpectrogram[i] = new double[NumMelBands];
            for (int j = 0; j < NumMelBands; j++)
            {
                melSpectrogram[i][j] = 0;
                for (int k = 0; k < powerSpectrogram[i].Length; k++)
                {
                    melSpectrogram[i][j] += powerSpectrogram[i][k] * melFilterBank[j][k];
                }
            }
        }
        return melSpectrogram;
    }

    // メルフィルタバンクを作成する関数
    private double[][] CreateMelFilterBank()
    {
        double[][] filterBank = new double[NumMelBands][];
        double[] melFrequencies = GetMelFrequencies();
        double melMin = MelScale(0);
        double melMax = MelScale(SampleRate / 2);

        for (int i = 0; i < NumMelBands; i++)
        {
            filterBank[i] = new double[NFFT / 2 + 1];
            double melStart = melFrequencies[i];
            double melCenter = melFrequencies[i + 1];
            double melEnd = melFrequencies[i + 2];

            for (int j = 0; j <= NFFT / 2; j++)
            {
                double freq = (double)j / NFFT * SampleRate;
                double melFreq = MelScale(freq);

                if (melFreq >= melStart && melFreq <= melEnd)
                {
                    if (melFreq <= melCenter)
                    {
                        filterBank[i][j] = (melFreq - melStart) / (melCenter - melStart);
                    }
                    else
                    {
                        filterBank[i][j] = (melEnd - melFreq) / (melEnd - melCenter);
                    }
                }
                else
                {
                    filterBank[i][j] = 0;
                }
            }
        }

        return filterBank;
    }

    // メル周波数のスケールを計算する関数
    private double[] GetMelFrequencies()
    {
        double melMin = MelScale(0);
        double melMax = MelScale(SampleRate / 2);
        double[] melFrequencies = new double[NumMelBands + 2];

        for (int i = 0; i < melFrequencies.Length; i++)
        {
            melFrequencies[i] = InverseMelScale(melMin + (melMax - melMin) * i / (NumMelBands + 1));
        }

        return melFrequencies;
    }

    // メル尺度に変換
    private double MelScale(double frequency)
    {
        return 2595 * Math.Log10(1 + frequency / 700.0);
    }

    // メル尺度から逆変換
    private double InverseMelScale(double melFrequency)
    {
        return 700 * (Math.Pow(10, melFrequency / 2595) - 1);
    }
}
