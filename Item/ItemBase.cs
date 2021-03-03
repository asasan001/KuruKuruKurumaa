using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
public abstract class ItemBase : MonoBehaviour, IItem
{
    abstract public ItemType Type { get; }
    protected Subject<IItem> returnSubject = new Subject<IItem>();
    public IObservable<IItem> ReturnObservable
    {
        get
        {
            return returnSubject;
        }
    }
    private bool isFirstInitialize = true;
    private Quaternion initialQuat;
    private Vector3 initalPos;
    private void OnDestroy()
    {
        returnSubject.Dispose();
    }
    abstract public void Touched();
    public void Return()
    {
        returnSubject.OnNext(this);
    }
    virtual public void Initialize(float posX, Transform parent)
    {
        if (isFirstInitialize)
        {
            initalPos = transform.position;
            initialQuat = transform.localRotation;
            isFirstInitialize = false;
        }
        var rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        var pos = initalPos;
        pos.x = posX;
        transform.SetPositionAndRotation(pos, initialQuat);
        SetParent(parent);

    }
    public void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }
}
