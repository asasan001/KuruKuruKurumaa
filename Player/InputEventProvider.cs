using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
//スクリプトの実行順をこれを参照してるクラスより早くする
public class InputEventProvider :MonoBehaviour, IInputEventProvider
{
    public FloatReactiveProperty HorizontalMove { get; } = new FloatReactiveProperty(0);

    void Start() {
        HorizontalMove.AddTo(this);
    }

    // Update is called once per frame
    void Update()
    {
        HorizontalMove.Value = Input.GetAxisRaw("Horizontal");
    }
}
