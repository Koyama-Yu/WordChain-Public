using System;
using UnityEngine;
using UniRx;

public class StageGameTimeManager : MonoBehaviour
{
    private static Subject<string> _pauseSubject = new Subject<string>();
    private static Subject<string> _resumeSubject = new Subject<string>();

    private static bool _isPaused = false;
    public static bool IsPaused => _isPaused;

    public static IObservable<string> OnPaused
    {
        get { return _pauseSubject; }
    }

    public static IObservable<string> OnResumed
    {
        get { return _resumeSubject; }
    }

    public static void Pause()
    {
        _isPaused = true;

        _pauseSubject.OnNext("pause");
    }

    public static void Resume()
    {
        _isPaused = false;

        _resumeSubject.OnNext("resume");
    }

}