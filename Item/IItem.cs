using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
public interface IItem
{
    ItemType Type { get; }
    IObservable<IItem> ReturnObservable { get; }
    void Touched();
    void Return();
    void Initialize(float posX, Transform parent);
    void SetParent(Transform parent);
}
