using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab; // ������ ���� Prefab

    public Transform[] spawnPoints; // ��ȯ ��ġ

    private List<Monster> monsters = new List<Monster>(); // ������ ���͵��� ��� ����Ʈ

    private void Start()
    {
        CreateEnemy();
    }

    private void Update()
    {

    }

    // ���͸� �����ϰ� ������ ��� �Ҵ�
    private void CreateEnemy()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Transform spawnPoint = spawnPoints[i];
            Monster monster = Instantiate(monsterPrefab.GetComponentInChildren<Monster>(), spawnPoint.position, spawnPoint.rotation);

            monsters.Add(monster);

            monster.onDeath += () => monsters.Remove(monster);
            monster.onDeath += () => Debug.Log("dead");
            monster.onDeath += () => Destroy(monster.gameObject, 5f);
        }
    }
}
