using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerMover : MonoBehaviour
{
    private float targetSpeed = 0;
    [SerializeField] private float speedCoef = 5f;
    [SerializeField] private float accelation = 5f;
    private float currentSpeed = 0;

    private float movableRangeMin = -6;
    private float movableRangeMax = 6;
    private bool canMove = false;
    // Start is called before the first frame update
    void Start()
    {
        var inputEventProvider = GetComponent<IInputEventProvider>();
        inputEventProvider.HorizontalMove.Subscribe(f => targetSpeed = f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove) return;
        if (targetSpeed > currentSpeed) {
            currentSpeed += accelation * Time.deltaTime;
            if (targetSpeed < currentSpeed) currentSpeed = targetSpeed;
        } else if (targetSpeed < currentSpeed) {
            currentSpeed -= accelation * Time.deltaTime;
            if (targetSpeed > currentSpeed) currentSpeed = targetSpeed;
        }
        Vector3 pos = transform.position;
        pos.x += -currentSpeed * speedCoef * Time.deltaTime;
        if (pos.x < movableRangeMin)
        {
            pos.x = movableRangeMin;
            currentSpeed = 0;
        }
        else if (pos.x > movableRangeMax)
        {
            pos.x = movableRangeMax;
            currentSpeed = 0;
        }
    }
    public void SetCanMove(bool canMove) {
        this.canMove = canMove;
    }
}
