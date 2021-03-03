using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Toolkit;

public class ItemObjectPool<T> : ObjectPool<T> where T : MonoBehaviour, IItem
{
    //ItemのPrefab
    private readonly T _prefab;

    //ヒエラルキーウィンドウ上で親となるTransform
    private readonly Transform _root;

    public ItemObjectPool(T prefab, string name) {
        _prefab = prefab;

        //親になるObject
        _root = new GameObject().transform;
        _root.name = name;
        _root.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    protected override T CreateInstance()
    {
        //インスタンスが新しくなったらInstantiateする
        var newItem = GameObject.Instantiate(_prefab);

        //親となるTransformを変更する
        newItem.transform.SetParent(_root);
        return newItem;
        
    }

    public void ResetParent(T item) {
        item.SetParent(_root);
    }
}
