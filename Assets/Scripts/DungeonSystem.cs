using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSystem : MonoBehaviour
{
    private static DungeonSystem instance = null;
    public static DungeonSystem Instance { get { return instance; } }

    private int floor;

    [SerializeField]
    private RoomGenerator generator;
    [SerializeField]
    private int tempRoomCount;

    // [Header ("Dungeon Size")]
    // // [SerializeField]
    // private int row;
    // [SerializeField]
    // private int col;

    // [Header ("Room Size")]
    // [SerializeField]
    // private float roomWidth = 20f;

    // [SerializeField]
    // private GameObject spawnerParent;
    
    // // Spawner prefab
    // [SerializeField]
    // private GameObject spawnerPref;

    public int Floor { get { return floor; } }

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
        // !!! 레벨 디자인 클래스 만들기
        CreateDungeon();

        // CreatePortal(0, false);
        // CreatePortal(1, true);

        // if (floor % 2 == 1)
        // {
        //     // 홀수 층은 스폰 율이 높음
        //     CreateSpawners(0.8f, 2);
        // }
        // else
        // {
        //     CreateSpawners(1f);
        // }
    }

    // -------------------------------------------------------------
    // 던전 생성
    // -------------------------------------------------------------
    public void CreateDungeon()
    {
        // 맵 생성
        generator.Generate(tempRoomCount);
        // generator.GenerateWalls();
    }

    public void ClearDungeon()
    {
        // 맵 삭제
        generator.Clear();
        // generator.ClearWalls();
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
    // private void CreateSpawners(float percent, int maxCount = 1)
    // {
    //     // percent: Enemy가 나오는 방 비율
    //     for (int i = 2; i < generator.Rooms.Count; i++)
    //     {
    //         // !!! 임시로 2번방부터 마지막 방까지 스포너 생성
    //         if (Random.value < percent)
    //         {
    //             GameObject spawnerObject = Instantiate(
    //                 spawnerPref,
    //                 new Vector3(
    //                     generator.Rooms[i].X * roomWidth,
    //                     0f,
    //                     generator.Rooms[i].Y * roomWidth
    //                 ),
    //                 Quaternion.identity
    //             );
    //             spawnerObject.name = i.ToString();
    //             spawnerObject.transform.parent = spawnerParent.transform;
    //             // spawnerObject.GetComponent<Spawner>().Set(gameManager, roomWidth, Random.Range(1, maxCount + 1), 30f);
    //         }
    //     }
    // }
}