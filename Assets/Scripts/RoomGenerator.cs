using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    Default = 1,
    Moss = 2,
    Vine = 3,
    MossVine = 4
}

public enum RoomDirect
{
    Top,
    Right,
    Down,
    Left
}

public class RoomGenerator : MonoBehaviour
{
    private float roomWidth;
    private int maxRoomCount;
    private int roomCount;

    [SerializeField]
    private GameObject roomParent;

    [SerializeField]
    private GameObject tempRoomPref;
    [SerializeField]
    private GameObject wallPref;
    [SerializeField]
    private GameObject doorPref;
    private List<Room> rooms;            // 모든 방 리스트
    private Stack<Room> visitedRooms;
    private List<GameObject> walls;
    private List<GameObject> doors;

    public List<Room> Rooms { get { return rooms; } }
    
    private void Awake()
    {
        // default
        maxRoomCount = 25;
        roomCount = 0;
        roomWidth = 20f;
        rooms = new List<Room>();
        visitedRooms = new Stack<Room>();
        walls = new List<GameObject>();
        doors = new List<GameObject>();
    }

    // -------------------------------------------------------------
    // 던전 생성, 초기화
    // -------------------------------------------------------------
    public void Generate(int maxCount = 10)
    {
        roomCount = 0;
        maxRoomCount = maxCount;

        Room selectRoom = new Room();       // 시작 방 생성
        rooms.Add(selectRoom);
        visitedRooms.Push(selectRoom);
        GameObject initRoomObj = Instantiate(tempRoomPref, this.transform.position, Quaternion.identity);
        initRoomObj.transform.parent = roomParent.transform;
        initRoomObj.name = roomCount.ToString();
        selectRoom.SetRoomObject(initRoomObj);
        roomCount += 1;

        while (roomCount < maxRoomCount)
        {
            if (selectRoom.EmptyDirects.Count == 0)
            {
                // 인접한 방이 없는 경우, 이전 room 중에서 인접 방을 찾음
                visitedRooms.Pop();
                selectRoom = visitedRooms.Peek();        // 이전 room으로 다시 인접 빈 방 검색
            }
            else
            {
                // before room index로 새 room 생성
                Room newRoom = new Room(selectRoom.X, selectRoom.Y);
                // selectRoom에 인접한 빈 room 중 랜덤하게 선택하여 selectRoom과 상호 연결
                RoomDirect selected = selectRoom.EmptyDirects[Random.Range(0, selectRoom.EmptyDirects.Count)];
                selectRoom.InterconnectRoom(newRoom, selected);
                newRoom.UpdateCoorinate(selected);  // newRoom 좌표 재설정

                // newRoom에 인접한 기존 room이 있으면 서로 연결
                ConnectNearRoom(newRoom);

                // Room 좌표 위치에 groundPref 생성
                Vector3 roomPos = new Vector3(newRoom.X, newRoom.Y, 0f) * roomWidth;
                // newRoom.SetRoomObject(Instantiate(groundPref, roomPos, Quaternion.identity));
                GameObject newRoomObj = Instantiate(tempRoomPref, roomPos, Quaternion.identity);
                newRoomObj.transform.parent = roomParent.transform;
                newRoomObj.name = roomCount.ToString();
                newRoom.SetRoomObject(newRoomObj);

                selectRoom = newRoom;
                rooms.Add(selectRoom);
                visitedRooms.Push(selectRoom);
                roomCount += 1;
            }
        }
    }

    public void Clear()
    {
        // 생성된 모든 room object 삭제
        foreach (Room room in rooms)
        {
            Destroy(room.RoomObject);
        }

        // 초기화
        roomCount = 0;
        rooms.Clear();
        visitedRooms.Clear();
    }

    private void ConnectNearRoom(Room newRoom)
    {
        // newRoom은 좌표 설정 및 이전 room과 연결되어 있어야 함
        List<RoomDirect> directs = newRoom.EmptyDirects.ConvertAll(d => d);
        // !!! 안좋은 search
        foreach (RoomDirect direct in directs)
        {
            int checkX = 0;
            int checkY = 0;
            // newRoom은 emptyDirects.Count는 무조건 3
            switch (direct)
            {
                case RoomDirect.Top:
                    checkX = newRoom.X + 0;
                    checkY = newRoom.Y + 1;
                    break;
                case RoomDirect.Right:
                    checkX = newRoom.X + 1;
                    checkY = newRoom.Y + 0;
                    break;
                case RoomDirect.Down:
                    checkX = newRoom.X + 0;
                    checkY = newRoom.Y - 1;
                    break;
                case RoomDirect.Left:
                    checkX = newRoom.X - 1;
                    checkY = newRoom.Y + 0;
                    break;
            }

            foreach(Room existRoom in rooms)
            {
                if (existRoom.X == checkX && existRoom.Y == checkY)
                {
                    // newRoom 인접에 기존 room이 있는 경우
                    newRoom.InterconnectRoom(existRoom, direct);
                    break;
                }
            }
        }
    }

    // 벽 생성
    public void GenerateWalls()
    {
        float roomRadius = roomWidth / 2f;
        foreach(Room room in rooms)
        {
            foreach(RoomDirect direct in room.EmptyDirects)
            {
                // 인접한 방이 없는 곳에 벽 생성
                GameObject wall = Instantiate(wallPref);
                wall.transform.parent = roomParent.transform;
                Vector3 offset = Vector3.zero;

                switch (direct)
                {
                    case RoomDirect.Top:
                        offset += new Vector3(0f, 0f, roomRadius);
                        break;
                    case RoomDirect.Right:
                        offset += new Vector3(roomRadius, 0f, 0f);
                        wall.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
                        break;
                    case RoomDirect.Down:
                        offset += new Vector3(0f, 0f, -1 * roomRadius);
                        wall.transform.rotation = Quaternion.Euler(new Vector3(0f, -90f, 0f));
                        break;
                    case RoomDirect.Left:
                        offset += new Vector3(-1 * roomRadius, 0f, 0f);
                        wall.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                        break;
                }

                wall.transform.position = new Vector3(
                    room.X * roomWidth,
                    wall.transform.position.y,
                    room.Y * roomWidth
                ) + offset;

                walls.Add(wall);
            }

            // foreach(RoomDirect direct in room.ExistDirects)
            // {
            //     // 인접한 방이 있는 곳에 문 생성
            //     GameObject door = Instantiate(doorPref);
            //     Vector3 offset = Vector3.zero;

            //     switch (direct)
            //     {
            //         case RoomDirect.Top:
            //             offset += new Vector3(0f, 0f, roomRadius);
            //             room.Top.ExistDirects.Remove(RoomDirect.Down);
            //             break;
            //         case RoomDirect.Right:
            //             offset += new Vector3(roomRadius, 0f, 0f);
            //             // door.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
            //             room.Right.ExistDirects.Remove(RoomDirect.Left);
            //             break;
            //         case RoomDirect.Down:
            //             offset += new Vector3(0f, 0f, -1 * roomRadius);
            //             room.Down.ExistDirects.Remove(RoomDirect.Top);
            //             break;
            //         case RoomDirect.Left:
            //             offset += new Vector3(-1 * roomRadius, 0f, 0f);
            //             // door.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
            //             room.Left.ExistDirects.Remove(RoomDirect.Right);
            //             break;
            //     }

            //     door.transform.position = new Vector3(
            //         room.X * roomWidth,
            //         door.transform.position.y,
            //         room.Y * roomWidth
            //     ) + offset;

            //     doors.Add(door);
            // }
        }
    }

    // 모든 벽 제거
    public void ClearWalls()
    {
        for (int i = 0; i < walls.Count; i++)
        {
            Destroy(walls[i]);
        }
        walls.Clear();

        for (int i = 0; i < doors.Count; i++)
        {
            Destroy(doors[i]);
        }
        doors.Clear();
    }
}