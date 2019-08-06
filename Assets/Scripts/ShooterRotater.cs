using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterRotater : MonoBehaviour
{
    private enum RotateState
    {

        Idle,Vertical,Horizontal,Ready
    }

    //처음 상태: Idle
    private RotateState state = RotateState.Idle;

    public float verticalRotateSpeed = 360f;
    public float horizontalRotateSpeed = 360f;

    public BallShooter ballShooter;

    void Update()
    {
        switch(state)
        {
            case RotateState.Idle:
                if (Input.GetButtonDown("Fire1"))//버튼을 누른 그 순간
                {
                    state = RotateState.Horizontal;
                }
                break;
            case RotateState.Horizontal:
                if (Input.GetButton("Fire1"))//버튼이 눌려있는 동안
                {
                    transform.Rotate(new Vector3(0, horizontalRotateSpeed * Time.deltaTime, 0));
                }
                else if (Input.GetButtonUp("Fire1"))//버튼을 떼는 그 순간
                {
                    state = RotateState.Vertical;
                }
                break;
            case RotateState.Vertical:
                if (Input.GetButton("Fire1"))//버튼이 눌려있는 동안
                {
                    transform.Rotate(new Vector3(-verticalRotateSpeed * Time.deltaTime, 0, 0));
                }
                else if (Input.GetButtonUp("Fire1"))//버튼을 떼는 그 순간
                {
                    state = RotateState.Ready;
                    ballShooter.enabled = true;
                }
                break;
            case RotateState.Ready:
                break;
        }
    }

    private void OnEnable()
    {
        transform.rotation = Quaternion.identity;//0,0,0도
        state = RotateState.Idle;
        ballShooter.enabled = false;
    }
}