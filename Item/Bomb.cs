using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
public class Bomb : ItemBase
{
    override public ItemType Type { get; } = ItemType.Bomb;
    private GameObject smoke;
    private GameObject model;
    private bool isFirst=true;

    override public void Initialize(float posX, Transform parent)
    {
        base.Initialize(posX, parent);
        if (isFirst) {
            isFirst = false;
            var mon = transform.Find("Mon_00").gameObject;
            smoke = mon.transform.Find("smoke_fx01").gameObject;
            model = mon.transform.Find("Object001").gameObject;
        }
        smoke.SetActive(false);
        model.SetActive(true);
    }
    override public void Touched()
    {
        smoke.SetActive(true);
        model.SetActive(false);
        Observable.Timer(dueTime: TimeSpan.FromSeconds(2f))
            .Subscribe(_ => returnSubject.OnNext(this));
    }
}
