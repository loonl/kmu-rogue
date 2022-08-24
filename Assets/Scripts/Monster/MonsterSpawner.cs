using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    private int floor; // 층 번호
    private int roomIndex; // 방 번호

    private List<Dictionary<string, object>> monsterData; // 몬스터 데이터
    private List<float> monsterProbList = new List<float>(); // 몬스터 생성 확률을 담은 리스트
    private List<Monster> monsters = new List<Monster>(); // 생성된 몬스터들을 담는 리스트
    private List<Vector3> spawnPoints; // 스폰 위치

    public void Set(List<Vector3> spawnPositions, int roomIndex, int floor, List<Dictionary<string, object>> monsterData, List<float> monsterProbList)
    {
        spawnPoints = spawnPositions;
        this.floor = floor;
        this.roomIndex = roomIndex;
        this.monsterData = monsterData;
        this.monsterProbList = monsterProbList;
    }

<<<<<<< Updated upstream
    private int _roomIndex;
    //private void OnEnable()
    //{
    //    // 몬스터 스폰
    //    CreateEnemy();
    //}

    // 몬스터를 생성하고 추적할 대상 할당
    private void CreateEnemy()
=======

    // 몬스터 스폰
    public void Spawn()
>>>>>>> Stashed changes
    {
        foreach (Vector3 spawnPoint in spawnPoints)
        {
            int monsterId = RandomSelect(monsterProbList);
            
            string monsterPath = monsterData[monsterId]["Path"].ToString();
            GameObject enemy = GameManager.Instance.CreateGO(monsterPath, this.transform);
            enemy.transform.position = spawnPoint;
            Monster monster = enemy.GetComponent<Monster>();
            monster.monsterData = monsterData;
            monsters.Add(monster);
<<<<<<< Updated upstream
            monster.onDeath += () =>
            {
                monsters.Remove(monster);
                CheckRemainEnemy();
            };
            monster.onEliminate += () => Destroy(monster.gameObject);
            monster.onRevive += () => monsters.Add(monster);
=======

            monster.onEliminate += () =>
            {
                Destroy(monster.gameObject);
                monsters.Remove(monster);
                CheckRemainEnemy();
            };
>>>>>>> Stashed changes
        }
    }

    // csv파일에 기재된 확률에 의거해 무작위로 몬스터 선택
    int RandomSelect(List<float> monsterProbList)
    {
        float total = 0;

        foreach (float elem in monsterProbList)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < monsterProbList.Count; i++)
        {
            if (randomPoint < monsterProbList[i])
            {
                return i;
            }
            else
            {
                randomPoint -= monsterProbList[i];
            }
        }
        return monsterProbList.Count - 1;
    }

    // 방에 남은 몬스터가 있는지 확인
    private void CheckRemainEnemy()
    {
        if (monsters.Count < 1)
        {
            DungeonSystem.Instance.Rooms[roomIndex].Clear();
        }
    }
}