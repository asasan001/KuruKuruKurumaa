using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

public class ItemProvider : SingletonMonoBehaviour<ItemProvider>
{
    [SerializeField] private int providedGemNum = 5;
    [SerializeField] private int providedBombNum = 5;
    private bool canProvideClock = false;
    [SerializeField] private int timeSlices = 10;
    [SerializeField] private int horizontalSlices = 10;
    [SerializeField] private float timeIntervalCoef = 500f;
    [SerializeField] private float provideClockInterval = 10;
    [SerializeField] private Gem gemPrefab = null;
    [SerializeField] private Bomb bombPrefab = null;
    [SerializeField] private SpeedClock clockPrefab = null;
    private ItemObjectPool<Gem> gemObjectPool;
    private ItemObjectPool<Bomb> bombObjectPool;
    private ItemObjectPool<SpeedClock> clockObjectPool;
    private const float placeableRangeMin = -6;
    private const float placeableRangeMax = 6;
    readonly Vector3 defaultPosition = new Vector3(0,0,-3); 

    private float rotateSpeed;
    Coroutine coroutineForStop;
    Transform rotateStage;

    bool canDoProvideItem;

    // Start is called before the first frame update
    void Start()
    {
        rotateStage = StageManager.Instance.transform;
        gemObjectPool = new ItemObjectPool<Gem>(gemPrefab, "Gems");
        bombObjectPool = new ItemObjectPool<Bomb>(bombPrefab, "Bombs");
        clockObjectPool = new ItemObjectPool<SpeedClock>(clockPrefab, "SpeedClocks");

        gemObjectPool.PreloadAsync(preloadCount:15,threshold:2).Subscribe();
        bombObjectPool.PreloadAsync(preloadCount: 10, threshold: 2).Subscribe();
        clockObjectPool.PreloadAsync(preloadCount: 3, threshold: 2).Subscribe();
        Observable.Timer(dueTime: TimeSpan.FromSeconds(provideClockInterval), period: TimeSpan.FromSeconds(provideClockInterval))
            .Subscribe(_ => {
                canProvideClock = true;
            }    
            ).AddTo(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (canDoProvideItem) ProvideItem();
    }

    public void SetCanStartProvideItem(bool canDoProvideItem)
    {
        this.canDoProvideItem = canDoProvideItem;
        if (!canDoProvideItem && coroutineForStop != null) StopCoroutine(coroutineForStop);
    }
    public void SetRotateSpeed(float rotateSpeed) {
        this.rotateSpeed = rotateSpeed;
        if(canDoProvideItem) ProvideItem();
    }
    private void ProvideItem() {
        if(coroutineForStop != null) StopCoroutine(coroutineForStop);
        canDoProvideItem = false;
        coroutineForStop = StartCoroutine(ProvideItemCoroutine());
    }
    private IEnumerator ProvideItemCoroutine() {
        int providedClockNum = canProvideClock?1:0;
        canProvideClock = false;

        List<int> randomChoices = RandomChoices(0, timeSlices * horizontalSlices, providedGemNum + providedBombNum + providedClockNum);
        List<int> quotients = randomChoices.Select(x =>(int)( x/timeSlices)).ToList();
        List<int> remainders = randomChoices.Select(x => x % timeSlices).ToList();
        List<ItemType> types = Enumerable.Repeat(ItemType.Gem,providedGemNum)
            .Concat(Enumerable.Repeat(ItemType.Bomb, providedBombNum))
            .Concat(Enumerable.Repeat(ItemType.SpeedClock, providedClockNum)).ToList();
       
        float timeInterval = timeIntervalCoef / (timeSlices * rotateSpeed);
        for (int time = 0; time < timeSlices; time++) {
            yield return new WaitForSeconds(timeInterval);
            for (int index = 0; index < quotients.Count(); index++) {
                if (quotients[index] == time) {
                    PlaceItem(types[index],remainders[index]);
                }
            }

        }
        canDoProvideItem = true;
        providedClockNum = 0;
        
    }

    private List<int> RandomChoices(int start, int end, int choicesNum)//start〜endからchoicesNum選ぶ
    {
        List<int> Results = new List<int>();
        List<int> numbers = new List<int>();
        for (int i = start; i <= end; i++)
        {
            numbers.Add(i);
        }

        while (choicesNum-- > 0)
        {

            int index = UnityEngine.Random.Range(0, numbers.Count);
           
            int x = numbers[index];
            Results.Add(x);

            numbers.RemoveAt(index);
        }
        return Results;
    }


    private void PlaceItem(ItemType type, int place) {
        IItem item=null;
        switch (type) {
            case ItemType.Gem:
                item = gemObjectPool.Rent();
                item.ReturnObservable.Take(1).Subscribe(x => {
                    gemObjectPool.ResetParent((Gem)x);
                    gemObjectPool.Return((Gem)x);
                });
                break;
            case ItemType.Bomb:
                item = bombObjectPool.Rent();
                item.ReturnObservable.Take(1).Subscribe(x => {
                    bombObjectPool.ResetParent((Bomb)x);
                    bombObjectPool.Return((Bomb)x);
                });
                break;
            case ItemType.SpeedClock:
                item = clockObjectPool.Rent();
                item.ReturnObservable.Take(1).Subscribe(x => {
                    clockObjectPool.ResetParent((SpeedClock)x);
                    clockObjectPool.Return((SpeedClock)x);
                });
                break;
        }

        float space = (placeableRangeMax - placeableRangeMin) / (horizontalSlices - 1);

        var posX = placeableRangeMin + space * place;

        item.Initialize(posX, rotateStage);
    }
}
