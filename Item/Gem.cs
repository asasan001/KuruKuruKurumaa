using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
public class Gem :ItemBase
{
    override public ItemType Type { get; } = ItemType.Gem;
    override public void Touched()
    {
        returnSubject.OnNext(this);
    }
}
