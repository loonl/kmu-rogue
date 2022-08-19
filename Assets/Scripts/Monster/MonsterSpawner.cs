using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    private Monster monsterPrefab; // 생성할 몬스터 Prefab

    public List<Vector3> spawnPoints; // 스폰 위치

    private List<Monster> monsters = new List<Monster>(); // 생성된 몬스터들을 담는 리스트

    //private void OnEnable()
    //{
    //    // 몬스터 스폰
    //    CreateEnemy();
    //}

    // 몬스터를 생성하고 추적할 대상 할당
    private void CreateEnemy()
    {
        foreach (Vector3 spawnPoint in spawnPoints)
        {
            // !!! 좀비만 스폰되게 설정
            Object monsterObj = Resources.Load("Prefabs/Dungeon/Zombie");
            GameObject goMonster = Instantiate(monsterObj, spawnPoint, Quaternion.identity) as GameObject;
            Monster monster = goMonster.GetComponent<Zombie>() as Monster;

            monsters.Add(monster);
            monster.onDeath += () => monsters.Remove(monster);
            monster.onEliminate += () => Destroy(monster.gameObject);
            monster.onRevive += () => monsters.Add(monster);
        }
    }

    public void Set(List<Vector3> spawnPositions)
    {
        spawnPoints = spawnPositions;
    }

    public void Spawn()
    {
        this.CreateEnemy();
    }
}
