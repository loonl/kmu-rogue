using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSystem : MonoBehaviour
{
    private static DungeonSystem instance = null;
    public static DungeonSystem Instance { get { return instance; } }

    public int Floor { get; private set; } = 1;

    [SerializeField]
    private RoomGenerator generator;
    [SerializeField]
    private int tempRoomCount;

    public Transform DroppedItems;        // 떨어진 아이템 parent transform


    public List<DungeonRoom> Rooms { get { return generator.Rooms; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Load()
    {
        // 현재 Floor 기준으로 레벨 디자인
        CreateDungeon();
    }

    // -------------------------------------------------------------
    // 던전 생성
    // -------------------------------------------------------------
    public void CreateDungeon()
    {
        List<TileType> tileSeqence = new List<TileType>()
        {
            TileType.DefaultGround,
            TileType.MossGround,
            TileType.VineGround,
            TileType.VineMossGround
        };

        // 맵 생성
        generator.Generate(tempRoomCount, tileSeqence[(Floor - 1) % 4]);
        CreateSpawners(Floor);   // 스포너 생성
        CreateShop();       // 상점 생성
    }

    public void ClearDungeon()
    {
        // 맵 삭제
        generator.Clear();
    }

    // -------------------------------------------------------------
    // 스포너 생성
    // -------------------------------------------------------------
    private void CreateSpawners(int floor)
    {
        List<Dictionary<string, object>> monsterSpawnerData = CSVReader.Read("Datas/MonsterSpawner");
        List<Dictionary<string, object>> monsterData = CSVReader.Read("Datas/Monster");

        List<float> monsterProbList = new List<float>();
        for (int i = 0; i < monsterSpawnerData.Count; i++)
        {
            if(floor == int.Parse(monsterSpawnerData[i]["Floor"].ToString()))
            {
                monsterProbList.Add(float.Parse(monsterSpawnerData[i]["Prob"].ToString()));
            }
        }

        for (int roomIndex = 1; roomIndex < generator.Rooms.Count; roomIndex++)
        // foreach (DungeonRoom room in generator.Rooms)
        {
            // 0번 방에는 스포너를 안만듬
            int MonsterSpawnerId = RandomSelect(monsterProbList);
            string MonsterSpawnerPath = monsterSpawnerData[MonsterSpawnerId]["Path"].ToString();

            GameObject goSpawner = GameManager.Instance.CreateGO(MonsterSpawnerPath, generator.Rooms[roomIndex].transform);
            MonsterSpawner spawner = goSpawner.GetComponent<MonsterSpawner>();
            generator.Rooms[roomIndex].SetSpawner(spawner, roomIndex, monsterData);
        }
    }

    // csv파일에 기재된 확률에 의거해 무작위로 몬스터 선택
    int RandomSelect(List<float> list)
    {
        float total = 0;

        foreach (float elem in list)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < list.Count; i++)
        {
            if (randomPoint < list[i])
            {
                return i;
            }
            else
            {
                randomPoint -= list[i];
            }
        }
        return list.Count - 1;
    }

    // -------------------------------------------------------------
    // 상점 생성
    // -------------------------------------------------------------
    private void CreateShop()
    {
        DungeonRoom shop = generator.Shop;
        
        // DroppedItem 생성
        // GameManager.Instance.CreateGo("Prefabs/Dungeon/DroppedItem", shop.transform);
    }
}