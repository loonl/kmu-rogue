using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public Monster monsterPrefab; // ������ ���� Prefab

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
            Monster monster = Instantiate(monsterPrefab, spawnPoint.position, spawnPoint.rotation);

            monsters.Add(monster);
            monster.onDeath += () => monsters.Remove(monster);
            monster.onDeath += () => Destroy(monster.gameObject, 2f);
        }
    }
}
