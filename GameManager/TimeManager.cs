using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.UI;

public class TimeManager : SingletonMonoBehaviour<TimeManager>
{
    [SerializeField]private int _time = 80;
    [SerializeField] private Text timeText = null;
    private Subject<Unit> timeOut = new Subject<Unit>();
    public IObservable<Unit> TimeOut {
        get {
            return timeOut;
        }
    }
    private int Time {
        set
        {
            _time = value > 0 ? value : 0;
        }
        get
        {
            return _time;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        timeText.text = Time.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TimerStart() {
        var timer = Observable.Timer(dueTime: TimeSpan.FromSeconds(1), period: TimeSpan.FromSeconds(1)).Publish();
        var timerDisposable = timer.Connect();
        timer.Subscribe(x=>
        {
            --Time;
            timeText.text = Time.ToString();
            if (Time == 0) {
                timerDisposable.Dispose();
                timeOut.OnNext(Unit.Default);
                timeOut.OnCompleted();
            }
         });

    }
}
