using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    public int score = 5;
    public ParticleSystem explosionParticle;
    public float hp = 10f;

    public void TakeDamage(float damage)
    {
        hp -= damage;

        if(hp <= 0)
        {
            ParticleSystem instance = Instantiate(explosionParticle, transform.position, transform.rotation);

            AudioSource explosionAudio = instance.GetComponent<AudioSource>();
            explosionAudio.Play();

            GameManager.instance.AddScore(score);

            Destroy(instance.gameObject, instance.duration);//폭발 이펙트 파괴
            gameObject.SetActive(false);//프롭은 파괴가 되는게 아니라 모양만 없어짐(몇백개가 사라졌다 생겼다 하면 렉이 심할 수 있어서.)
        }
    }
}
