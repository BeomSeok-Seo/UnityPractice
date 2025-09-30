using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    private List<Transform> spawnPointList;
    private GameObject enemies;

    private int liveEnemies = 0;

    public GameObject EnemyPrefab;
    public GameObject EnemyPrefab2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnPointList = new List<Transform>();
        int count = transform.childCount;

        for (int i = 0; i < count; i++) 
        {
            spawnPointList.Add(transform.GetChild(i));
        }

        enemies = GameObject.Find("Enemies");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemies(int count)
    {
        liveEnemies = 0;

        for (int i = 0; i < count; i++)
        {
            int target = Random.Range(0, spawnPointList.Count - 1);
            int prefab = Random.Range(0, 2);

            Vector3 targetPosition = Vector3.zero;

            Vector3 monsterSpawnPoint = spawnPointList[target].position;
            Vector3 directionToTarget = targetPosition - monsterSpawnPoint;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            GameObject enemy = Instantiate(prefab == 0 ? EnemyPrefab : EnemyPrefab2, monsterSpawnPoint, targetRotation, enemies.transform);

            EnemyController controller = enemy.GetComponentInChildren<EnemyController>();

            controller.DeadEvent += EnemyDead;

            liveEnemies++;
        }
    }

    private void EnemyDead()
    {
        liveEnemies--;

        if (liveEnemies == 0)
        {
            GameManager.Instance.GoNextStage();
        }
    }
}
