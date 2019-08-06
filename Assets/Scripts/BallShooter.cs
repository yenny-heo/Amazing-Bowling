using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallShooter : MonoBehaviour
{
    public CamFollow cam;
    public Rigidbody ball;
    public Transform firePos;
    public Slider powerSlider;
    public AudioSource shootingAudio;
    public AudioClip fireClip;
    public AudioClip chargingClip;
    public float minForce = 15f;
    public float maxForce = 30f;
    public float chargingTime = 0.75f;//min-> max 까지 충전되는 시간

    private float currentForce;
    private float chargeSpeed;
    private bool fired;
    private bool upDown;

    //컴포넌트가 켜질 때마다 매번 실행됨.
    private void OnEnable()
    {
        //초기화
        currentForce = minForce;
        powerSlider.value = minForce;
        fired = false;
        upDown = false;

    }

    private void Start()
    {
        chargeSpeed = (maxForce - minForce) / chargingTime;//1초동안 충전되는 힘
    }
    private void Update()
    {
        if (fired == true)//이미 발사된 경우 동작 X
            return;

        powerSlider.value = minForce;
        if(currentForce > maxForce && !fired)
        {
            currentForce = maxForce;
            upDown = true;

        }
        else if(currentForce < minForce && !fired)
        {
            currentForce = minForce;
            upDown = false;
        }
        else if(Input.GetButtonDown("Fire1"))
        {
            fired = false;
            currentForce = minForce;
            shootingAudio.clip = chargingClip;
            shootingAudio.Play();
        }
        else if(Input.GetButton("Fire1") && !fired)
        {
            if (upDown == false)
                currentForce += chargeSpeed * Time.deltaTime;
            else if (upDown == true)
                currentForce -= chargeSpeed * Time.deltaTime;
            powerSlider.value = currentForce;
        }
        else if(Input.GetButtonUp("Fire1") && !fired)
        {
            //발사 처리
            Fire();
        }
    }

    private void Fire()
    {
        fired = true;
        Rigidbody ballInstance = Instantiate(ball, firePos.position, firePos.rotation);//공 생성
        ballInstance.velocity = currentForce * firePos.forward;
        shootingAudio.clip = fireClip;
        shootingAudio.Play();

        currentForce = minForce;

        cam.SetTarget(ballInstance.transform, CamFollow.State.Tracking);
    }

}
