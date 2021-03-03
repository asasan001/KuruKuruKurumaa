using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class PlayerCore : MonoBehaviour
{
    private Subject<ItemType> sendTouchedItemType = new Subject<ItemType>();
    private PlayerMover playerMover;
    public IObservable<ItemType> SendTouchedItemType
    {
        get
        {
            return sendTouchedItemType;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        playerMover = GetComponent<PlayerMover>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider collider)
    {
        var item = collider.gameObject.GetComponent<IItem>();
        if (item != null)
        {
            item.Touched();
            sendTouchedItemType.OnNext(item.Type);
        }
    }
    public void SetCanMove(bool canMove) {
        playerMover.SetCanMove(canMove);
    }
}
