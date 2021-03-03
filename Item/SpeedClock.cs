using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
public class SpeedClock :ItemBase
{
    override public ItemType Type { get; } = ItemType.SpeedClock;
    override public void Touched()
    {
        returnSubject.OnNext(this);
    }
}
