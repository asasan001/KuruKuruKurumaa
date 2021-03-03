using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
public class StageManager : SingletonMonoBehaviour<StageManager>
{
    private FloatReactiveProperty rotateSpeed = new FloatReactiveProperty(0);
    public IObservable<float> RotateSpeed { get { return rotateSpeed; } } 
    private const float speedCoef = 20; 
    private const float speedIntc = 50;
    private bool canRotate = false;


    override protected void Awake()
    {
        base.Awake();
        SetSpeedLevel(0);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!canRotate) return;
        transform.Rotate(new Vector3(rotateSpeed.Value * Time.deltaTime, 0, 0));
    }

    public void SetSpeedLevel(int speedLevel) {
        rotateSpeed.Value = speedCoef * speedLevel + speedIntc;
    }
    public void SetCanRotate(bool canRotate) {
        this.canRotate = canRotate;
    }
}
