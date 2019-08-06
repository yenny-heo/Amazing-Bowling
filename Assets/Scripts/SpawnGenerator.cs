using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGenerator : MonoBehaviour
{
    public GameObject[] propPrefabs;
    private BoxCollider area;
    public int count = 100;

    //재활용될 프롭들
    private List<GameObject> props = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        area = GetComponent<BoxCollider>();
        for (int i = 0; i < count; i++) 
        {
            Spawn();
        }

        area.enabled = false;//다른 충돌처리에 방해되지 않도록
        
    }

    private void Spawn()
    {
        int selection = Random.Range(0, propPrefabs.Length);//프롭들 중 하나 랜덤하게 선택

        GameObject selectedPrefab = propPrefabs[selection];//선택된 프롭 저장
        Vector3 spawnPos = GetRandomPos();//프롭의 위치 랜덤하게 지정

        GameObject instance = Instantiate(selectedPrefab, spawnPos, Quaternion.identity);//프롭 생성
        props.Add(instance);
    }

   private Vector3 GetRandomPos()
    {
        Vector3 basePosition = transform.position;
        Vector3 size = area.size;

        float posX = basePosition.x + Random.Range(-size.x / 2f, size.x / 2f);
        float posY = basePosition.y + Random.Range(-size.y / 2f, size.y / 2f);
        float posZ = basePosition.z + Random.Range(-size.z / 2f, size.z / 2f);

        Vector3 spawnPos = new Vector3(posX, posY, posZ);
        return spawnPos;
    }

    public void Reset()
    {
        for (int i = 0; i < props.Count; i++) 
        {
            props[i].transform.position = GetRandomPos();
            props[i].SetActive(true);
        }
    }
}
