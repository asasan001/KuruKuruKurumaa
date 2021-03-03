using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRetriever : SingletonMonoBehaviour<ItemRetriever>
{
    void OnTriggerEnter(Collider collider)
    {
        var item = collider.gameObject.GetComponent<ItemBase>();
        if (item != null)
        {
            item.Return();
        }
    }
}
