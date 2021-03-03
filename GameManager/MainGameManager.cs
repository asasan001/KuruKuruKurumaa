using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.UI;
public class MainGameManager : SingletonMonoBehaviour<MainGameManager>
{
    [SerializeField] int gemPoint = 100;
    [SerializeField] int bombPoint = -100;
    [SerializeField] private Text pointText = null;
    [SerializeField] private Text countDownText = null;
    PlayerCore player;
    ItemProvider itemProvider;
    StageManager stageManager;
    TimeManager timeManager;
    SoundManager soundManager;
    ResultManager resultManager;
    private int _totalPoint = 0;
    private int TotalPoint {
        set
        {
            _totalPoint = value > 0 ? value : 0;
        }
        get {
            return _totalPoint;
        }
     }
    private int speedLevel = 0;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerCore>();
        player.SendTouchedItemType.Subscribe(itemType => OnGetItem(itemType));
        stageManager = StageManager.Instance;
        itemProvider = ItemProvider.Instance;
        stageManager.RotateSpeed.Subscribe(
            rotateSpeed=> itemProvider.SetRotateSpeed(rotateSpeed));
        timeManager = TimeManager.Instance;
        timeManager.TimeOut.Subscribe(_=>GameEnd());
        soundManager = SoundManager.Instance;
        resultManager = ResultManager.Instance;

        int count = 4;
        countDownText.gameObject.SetActive(false);
        var timer = Observable.Timer(dueTime: TimeSpan.FromSeconds(1), period: TimeSpan.FromSeconds(1)).Publish();
        var timerDisposable = timer.Connect();
        timer.Subscribe(x =>
        {
            --count;
            countDownText.text = count.ToString();
            if (count == 3)
            {
                countDownText.gameObject.SetActive(true);
            }

            if (count != 0) {

                soundManager.PlayCountSound();
            }
            else{
                timerDisposable.Dispose();
                soundManager.PlayCountFinishSound();
                countDownText.gameObject.SetActive(false);
                GameStart();
            }
        });

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGetItem(ItemType itemType) {
        switch(itemType){
            case ItemType.Gem:
                soundManager.PlayGemSound();
                TotalPoint += gemPoint;
                pointText.text = TotalPoint.ToString();
                break;
            case ItemType.Bomb:
                soundManager.PlayBombSound();
                TotalPoint += bombPoint;
                pointText.text = TotalPoint.ToString();
                break;
            case ItemType.SpeedClock:
                soundManager.PlayClockSound();
                ++speedLevel;
                stageManager.SetSpeedLevel(speedLevel);
                break;
        }
    }
    private void GameStart(){
        timeManager.TimerStart();
        itemProvider.SetCanStartProvideItem(true);
        stageManager.SetCanRotate(true);
        player.SetCanMove(true);
    }
    private void GameEnd() {
        soundManager.PlayCarStopSound();
        itemProvider.SetCanStartProvideItem(false);
        stageManager.SetCanRotate(false);
        player.SetCanMove(false);
        Observable.Timer(dueTime: TimeSpan.FromSeconds(1f))
            .Subscribe(_=> resultManager.DisplayResult(TotalPoint));
    }
}
