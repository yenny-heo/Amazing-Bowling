using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public ParticleSystem explosionParticle;
    public AudioSource explosionAudio;
    public LayerMask whatIsProp;

    public float maxDamage = 100f;
    public float explosionForce = 1000f;//폭발 힘
    public float lifeTime = 10f;
    public float explosionRadius = 20f;//폭발 반경

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, whatIsProp);//ball을 중심으로 가상의 구를 그려서 거기에 해당하는 콜라이더들을 배열로 가져옴

        for(int i=0; i < colliders.Length; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

            targetRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);//폭발 힘, 폭발물의 위치, 폭발 반경 -> 물체의 폭발 데미지를 계산하여 물체가 튕겨나가는 효과를 줌.

            Prop targetProp = colliders[i].GetComponent<Prop>();//Prop 스크립트 가져옴

            float damage = CalculateDamage(colliders[i].transform.position);//폭발 데미지 계산
            targetProp.TakeDamage(damage);//데미지 만큼 체력 감소
        }

        explosionParticle.transform.parent = null;//파티클이 ball에서 빠져나오도록
        explosionParticle.Play();
        explosionAudio.Play();

        GameManager.instance.OnBallDestroy();

        Destroy(explosionParticle.gameObject, explosionParticle.duration);//특수효과가 모두 재생되면 파괴되도록.
        Destroy(gameObject);
    }

    //폭발에 가까울 수록 큰 데미지를 받도록 계산
    private float CalculateDamage(Vector3 targetPosition)
    {
        Vector3 explosionToTarget = targetPosition - transform.position;

        float distance = explosionToTarget.magnitude;

        float edgeToTarget = explosionRadius - distance;

        float percent = edgeToTarget / explosionRadius;

        float damage = maxDamage * percent;

        if (damage < 0) damage = 0;

        return damage;
    }
}
