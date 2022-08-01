using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public Monster monsterPrefab; // 생성할 몬스터 Prefab

    public Transform[] spawnPoints; // 스폰 위치

    private List<Monster> monsters = new List<Monster>(); // 생성된 몬스터들을 담는 리스트

    private void OnEnable()
    {
        // 몬스터 스폰
        CreateEnemy();
    }

    // 몬스터를 생성하고 추적할 대상 할당
    private void CreateEnemy()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Transform spawnPoint = spawnPoints[i];
            Monster monster = Instantiate(monsterPrefab, spawnPoint.position, spawnPoint.rotation);

            monsters.Add(monster);
            monster.onDeath += () => monsters.Remove(monster);
            monster.onEliminate += () => Destroy(monster.gameObject);
            monster.onRevive += () => monsters.Add(monster);
        }
    }
}
