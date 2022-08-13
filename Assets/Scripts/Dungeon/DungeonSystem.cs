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
        // 스포너 생성
        CreateSpawners();
        // !!! 오브젝트 추가하기
    }

    public void ClearDungeon()
    {
        // 맵 삭제
        generator.Clear();
    }

    // -------------------------------------------------------------
    // 포탈
    // -------------------------------------------------------------
    public Vector3 GetTargetPortalPos(int dungeonRoomIndex, ushort direct)
    {
        // 연결된 방의 반대 방향 direct 문 위치로 이동
        Vector3 offset;

        if (direct == 0)
        {
            offset = new Vector3(0, 1f, 0);
        }
        else if (direct == 1)
        {
            offset = new Vector3(1f, 0, 0);
        }
        else if (direct == 2)
        {
            offset = new Vector3(0, -1f, 0);
        }
        else
        {
            offset = new Vector3(-1f, 0, 0);
        }

        generator.Rooms[dungeonRoomIndex].Portals[(direct + 2) % 4].DeActivate();   // !!! temp
        return generator.Rooms[dungeonRoomIndex].Portals[(direct + 2) % 4].transform.position + offset * 0.5f;
    }

    // -------------------------------------------------------------
    // 스포너 생성
    // -------------------------------------------------------------
    private void CreateSpawners()
    {
        //for (int i = 1; i < generator.Rooms.Count; i++)
        foreach (DungeonRoom room in generator.Rooms)
        {
            // 0번 방에는 스포너를 안만듬
            Object spawnerObject = Resources.Load("Prefabs/Dungeon/Spawner");
            GameObject goSpawner = Instantiate(spawnerObject) as GameObject;
            goSpawner.transform.SetParent(room.transform);

            MonsterSpawner spawner = goSpawner.GetComponent<MonsterSpawner>();
            room.SetSpawner(spawner);
        }
    }
}