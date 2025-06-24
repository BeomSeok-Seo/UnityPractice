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

                // �Ѿ� �ӵ� �ʱ�ȭ
                rb.linearVelocity = Vector3.zero;

                // �Ѿ� �߻� �ӵ� ����
                rb.AddForce(Vector3.forward * 50f, ForceMode.Impulse);
                
                // ���� �� �ð����� obj ��Ȱ��ȭ
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

        // �ִ� ������ �Ѿ�� prefab �߰�
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
