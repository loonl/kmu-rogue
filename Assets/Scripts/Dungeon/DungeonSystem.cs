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
        List<Dictionary<string, object>> monsterData = CSVReader.Read("Datas/Monster");
        List<float> monsterProbList = new List<float>();
        for (int i = 0; i < monsterData.Count; i++)
        {
            float monsterProb = float.Parse(monsterData[i]["Floor" + floor].ToString());
            monsterProbList.Add(monsterProb);
        }

        for (int i = 1; i < generator.Rooms.Count; i++)
        // foreach (DungeonRoom room in generator.Rooms)
        {
            // 0번 방에는 스포너를 안만듬
            GameObject goSpawner = GameManager.Instance.CreateGO("Prefabs/Dungeon/Spawner", generator.Rooms[i].transform);
            MonsterSpawner spawner = goSpawner.GetComponent<MonsterSpawner>();
            generator.Rooms[i].SetSpawner(spawner, i, floor, monsterData, monsterProbList);
        }
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