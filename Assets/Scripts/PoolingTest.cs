using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingTest : MonoBehaviour
{
    public List<GameObject> pool;
    public GameObject prefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject obj = GetFromPool();
            if (obj != null)
            {
                obj.transform.position = transform.position;
                Rigidbody rb = obj.GetComponent<Rigidbody>();

                // 총알 속도 초기화
                rb.linearVelocity = Vector3.zero;

                // 총알 발사 속도 셋팅
                rb.AddForce(Vector3.forward * 50f, ForceMode.Impulse);
                
                // 지정 된 시간이후 obj 비활성화
                StartCoroutine(Alive(obj, 3f));
            }
        }
    }

    GameObject GetFromPool()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy) 
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // 최대 개수가 넘어가면 prefab 추가
        GameObject newObj = Instantiate(prefab);
        newObj.SetActive(true);
        pool.Add(newObj);

        return newObj;
    }

    public IEnumerator Alive(GameObject obj, float aliveTime)
    {
        float t = 0f;

        while (t < aliveTime)
        {
            t += Time.deltaTime;

            yield return null;
        }

        obj.SetActive(false);
    }
}
