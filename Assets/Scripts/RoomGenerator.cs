using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    DefaultGround = 0,
    DefaultWall = 1,
    DefaultEdge = 2,
    WaterGround = 3,
    WaterWall = 4,
    WaterEdge = 5,
    MossGround = 6,
    MossWall = 7,
    MossEdge = 8,
    MossWaterGround = 9,
    MossWaterWall = 10,
    MossWaterEdge = 11,
    VineGround = 12,
    VineWall = 13,
    VineEdge = 14,
    VineMossGround = 15,
    VineMossWall = 16,
    VineMossEdge = 17,
    VineMossWaterGround = 18,
    VineMossWaterWall = 19,
    VineMossWaterEdge = 20,
    DefaultOpenDoor = 21,
    DefaultCloseDoor = 22,
    MossOpenDoor = 23,
    MossCloseDoor = 24,
    VineOpenDoor = 25,
    VineCloseDoor = 26,
    VineMossOpenDoor = 27,
    VineMossCloseDoor = 28
}

public enum TileDirect
{
    Default = 0,
    Right = 1,
    Bottom = 2,
    Left = 3
}

public enum RoomDirect
{
    Top = 0,
    Right = 1,
    Down = 2,
    Left = 3
}

public class RoomGenerator : MonoBehaviour
{
    private float roomWidth;
    private int maxRoomCount;
    private int roomCount;

    [Header ("Room")]
    [SerializeField]
    private GameObject roomParent;

    [Header ("Door Tiles")]
    [SerializeField]
    private List<Tile> allDoors;   // Default, Moss, Vine, VineMoss

    [Header ("Tiles")]
    [SerializeField]
    private List<Tile> defaultGrounds;
    [SerializeField]
    private List<Tile> defaultWalls;
    [SerializeField]
    private List<Tile> defaultEdges;
    [SerializeField]
    private List<Tile> mossGrounds;
    [SerializeField]
    private List<Tile> mossWalls;
    [SerializeField]
    private List<Tile> mossEdges;
    [SerializeField]
    private List<Tile> vineWalls;
    [SerializeField]
    private List<Tile> vineEdges;
    [SerializeField]
    private List<Tile> vineMossWalls;
    [SerializeField]
    private List<Tile> vineMossEdges;

    [SerializeField]
    private GameObject emptyRoomPref;
    
    private List<Room> rooms;            // 모든 방 리스트
    private Stack<Room> visitedRooms;
    private List<DungeonRoom> dungeonRooms;

    public List<Room> Rooms { get { return rooms; } }
    
    private void Awake()
    {
        // default
        roomCount = 0;
        roomWidth = 20;
        rooms = new List<Room>();
        visitedRooms = new Stack<Room>();
        dungeonRooms = new List<DungeonRoom>();
    }

    // -------------------------------------------------------------
    // 던전 생성, 초기화
    // -------------------------------------------------------------
    public void Generate(int maxCount)
    {
        // 빈 방 생성
        CreateEmptyRoom(maxCount);

        // Temp
        List<TileType> types = new List<TileType>() { 
            TileType.DefaultGround,
            TileType.MossGround,
            TileType.VineGround,
            TileType.VineMossGround
        };

        // Room 타일 그리기
        DungeonRoom[] rooms = roomParent.GetComponentsInChildren<DungeonRoom>();
        foreach (DungeonRoom room in rooms)
        {
            // DrawRoom(room, TileType.DefaultGround, Random.Range(3, 20), Random.Range(3, 20));
            DrawRoom(room, types[Random.Range(0, types.Count)], Random.Range(3, 20), Random.Range(3, 20));
            // DrawRoom(room, types[Random.Range(0, types.Count)], 20, 20);
        }

        // 문 생성
        DrawDoors(rooms);
    }

    public void Clear()
    {
        // 생성된 모든 room object 삭제
        foreach (DungeonRoom room in dungeonRooms)
        {
            Destroy(room.gameObject);
        }

        // 초기화
        dungeonRooms.Clear();
        roomCount = 0;
        rooms.Clear();
        visitedRooms.Clear();
    }

    private void CreateEmptyRoom(int maxCount = 10)
    {
        roomCount = 0;
        maxRoomCount = maxCount;

        Room selectRoom = new Room();       // 시작 방 생성
        rooms.Add(selectRoom);
        visitedRooms.Push(selectRoom);
        GameObject initRoomObj = Instantiate(emptyRoomPref, this.transform.position, Quaternion.identity);
        dungeonRooms.Add(initRoomObj.GetComponent<DungeonRoom>());
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
                GameObject newRoomObj = Instantiate(emptyRoomPref, roomPos, Quaternion.identity);
                dungeonRooms.Add(newRoomObj.GetComponent<DungeonRoom>());
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

    
    // 타일 그리기
    private void DrawRoom(DungeonRoom room, TileType type, int rows, int cols)
    {
        ushort indexTileType = (ushort)type;
        // 타입은 Ground 타입을 지정하면 됨
        // row x col 크기의 방 생성
        // 모서리 생성
        Vector3Int currentTile = new Vector3Int(0, cols - 1, 0);
        DrawTile(room.WallLayer, (TileType)(indexTileType + 2), TileDirect.Default, currentTile);
        currentTile = new Vector3Int(rows - 1, cols - 1, 0);
        DrawTile(room.WallLayer, (TileType)(indexTileType + 2), TileDirect.Right, currentTile);
        currentTile = new Vector3Int(rows - 1, 0, 0);
        DrawTile(room.WallLayer, (TileType)(indexTileType + 2), TileDirect.Bottom, currentTile);
        currentTile = new Vector3Int(0, 0, 0);
        DrawTile(room.WallLayer, (TileType)(indexTileType + 2), TileDirect.Left, currentTile);

        // 벽 생성
        for (int row = 1; row < rows - 1; row++)
        {
            currentTile = new Vector3Int(row, cols - 1, 0);
            DrawTile(room.WallLayer, (TileType)(indexTileType + 1), TileDirect.Default, currentTile);
            currentTile = new Vector3Int(row, 0, 0);
            DrawTile(room.WallLayer, (TileType)(indexTileType + 1), TileDirect.Bottom, currentTile);
        }

        for (int col = 1; col < cols - 1; col++)
        {
            currentTile = new Vector3Int(0, col, 0);
            DrawTile(room.WallLayer, (TileType)(indexTileType + 1), TileDirect.Left, currentTile);
            currentTile = new Vector3Int(rows - 1, col, 0);
            DrawTile(room.WallLayer, (TileType)(indexTileType + 1), TileDirect.Right, currentTile);
        }

        // 바닥 생성
        for (int col = 1; col < cols - 1; col++)
        {
            for (int row = 1; row < rows - 1; row++)
            {
                currentTile = new Vector3Int(row, col, 0);
                DrawTile(room.GroundLayer, (TileType)indexTileType, TileDirect.Default, currentTile);
            }
        }

        // 방 크기에 맞추어 정 가운데로 정렬
        Vector3 tileMapSize = new Vector3(room.WallLayer.size.x, room.WallLayer.size.y, 0f);
        room.TileMapParent.transform.localPosition -= tileMapSize * room.WallLayer.cellSize.x * 0.5f;
    }

    private void DrawDoors(DungeonRoom[] dungeonRooms)
    {
        for (int roomIndex = 0; roomIndex < rooms.Count; roomIndex++)
        {
            foreach (RoomDirect direct in rooms[roomIndex].ExistDirects)
            {
                Vector3Int roomSize = dungeonRooms[roomIndex].WallLayer.size;
                Vector3Int centerDoorPos ;
                // 중앙 배치
                switch ((ushort)direct)
                {
                    case 0:
                        centerDoorPos = new Vector3Int((int)(roomSize.x / 2), roomSize.y - 1);
                        DrawTile(dungeonRooms[roomIndex].ObjectLayer, TileType.DefaultOpenDoor, (TileDirect)(ushort)direct, centerDoorPos);
                        break;
                    case 2:
                        centerDoorPos = new Vector3Int((int)(roomSize.x / 2), 0);
                        DrawTile(dungeonRooms[roomIndex].ObjectLayer, TileType.DefaultOpenDoor, (TileDirect)(ushort)direct, centerDoorPos);
                        break;
                    case 1:
                        centerDoorPos = new Vector3Int(roomSize.x - 1, (int)(roomSize.y / 2));
                        DrawTile(dungeonRooms[roomIndex].ObjectLayer, TileType.DefaultOpenDoor, (TileDirect)(ushort)direct, centerDoorPos);
                        break;
                    case 3:
                        centerDoorPos = new Vector3Int(0, (int)(roomSize.y / 2));
                        DrawTile(dungeonRooms[roomIndex].ObjectLayer, TileType.DefaultOpenDoor, (TileDirect)(ushort)direct, centerDoorPos);
                        break;
                }
            }
        }
    }

    private void DrawTile(Tilemap tileMap, TileType type, TileDirect direct, Vector3Int pos)
    {
        Tile tile = null;
        switch ((ushort)type)
        {
            case 0:
            case 12:
                // DefaultGrond or VineGround
                tile = RoomGenerator.SelectRandomTile(defaultGrounds);
                break;
            case 1:
                // DefaultWall
                tile = RoomGenerator.SelectRandomTile(defaultWalls);
                break;
            case 2:
                // DefaultEdge
                tile = RoomGenerator.SelectRandomTile(defaultEdges);
                break;
                // WaterWall,
                // WaterEdge,
            case 6:
            case 15:
                // MossGround or VindMossGround
                tile = RoomGenerator.SelectRandomTile(mossGrounds);
                break;
            case 7:
                // MossWall
                tile = RoomGenerator.SelectRandomTile(mossWalls);
                break;
            case 8:
                // MossEdge
                tile = RoomGenerator.SelectRandomTile(mossEdges);
                break;
            case 13:
                // VineWall
                tile = RoomGenerator.SelectRandomTile(vineWalls);
                break;
            case 14:
                // VineEdge
                tile = RoomGenerator.SelectRandomTile(vineEdges);
                break;
            case 16:
                // VineMossWall
                tile = RoomGenerator.SelectRandomTile(vineMossWalls);
                break;
            case 17:
                // VineMossEdge
                tile = RoomGenerator.SelectRandomTile(vineMossEdges);
                break;
                // MossWaterWall,
                // MossWaterEdge,
                // VineMossWaterWall,
                // VineMossWaterEdge,
            case 21:
            case 22:
            case 23:
            case 24:
            case 25:
            case 26:
            case 27:
            case 28:
                // All doors
                tile = allDoors[(ushort)type % 21];
                break;
        }

        if (tile != null)
        {
            tileMap.SetTile(pos, tile);
            
            if (direct != TileDirect.Default)
            {
                // 정방향 타일이 아닌 경우 타일 회전
                Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, (ushort)direct * -90f), Vector3.one);
                tileMap.SetTransformMatrix(pos, matrix);
            }
        }
        else
        {
            Debug.LogError("Invalid tile");
        }
    }

    private static Tile SelectRandomTile(List<Tile> tiles)
    {
        return tiles[Random.Range(0, tiles.Count)];
    }
}