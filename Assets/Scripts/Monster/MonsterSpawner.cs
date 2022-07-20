using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public Monster monsterPrefab; // ������ ���� Prefab

    public Transform[] spawnPoints; // ��ȯ ��ġ

    private List<Monster> monsters = new List<Monster>(); // ������ ���͵��� ��� ����Ʈ

    private void Update()
    {

    }

    // ���͸� �����ϰ� ������ ��� �Ҵ�
    private void CreateEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Monster monster = Instantiate(monsterPrefab, spawnPoint.position, spawnPoint.rotation);

        monsters.Add(monster);

        monster.onDeath += () => monsters.Remove(monster);
        monster.onDeath += () => Destroy(monster.gameObject, 10f);
    }
}
