using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public Monster monsterPrefab; // 생성할 몬스터 Prefab

    public Transform[] spawnPoints; // 소환 위치

    private List<Monster> monsters = new List<Monster>(); // 생성된 몬스터들을 담는 리스트

    private void Update()
    {

    }

    // 몬스터를 생성하고 추적할 대상 할당
    private void CreateEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Monster monster = Instantiate(monsterPrefab, spawnPoint.position, spawnPoint.rotation);

        monsters.Add(monster);

        monster.onDeath += () => monsters.Remove(monster);
        monster.onDeath += () => Destroy(monster.gameObject, 10f);
    }
}
