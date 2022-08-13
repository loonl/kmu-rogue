using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonRoom : MonoBehaviour
{
    public GameObject TileMapParent;

    [SerializeField]
    private Tilemap _groundLayer;
    [SerializeField]
    private Tilemap _wallLayer;
    [SerializeField]
    private Tilemap _objectLayer;
    [SerializeField]
    private Tilemap _closeDoorLayer;
    [SerializeField]
    private Tilemap _openDoorLayer;

    public Tilemap GroundLayer { get { return _groundLayer; } }
    public Tilemap WallLayer { get { return _wallLayer; } }
    public Tilemap ObjectLayer { get { return _objectLayer; } }
    public Tilemap CloseDoorLayer { get { return _closeDoorLayer; } }
    public Tilemap OpenDoorLayer { get { return _openDoorLayer; } }

    public Portal[] Portals = new Portal[] { null, null, null, null };

    private MonsterSpawner _spawner = null;

    public void Enter()
    {
        // 플레이어 입장
    }

    public void ClearDungeonRoom()
    {
        // 방 클리어
    }

    public void SetSpawner(MonsterSpawner spawner)
    {
        _spawner = spawner;
        List<Vector3> spots = new List<Vector3>();
        float horizontalRange = (float)(this.WallLayer.size.x - 2) * 0.5f;
        float verticalRange = (float)(this.WallLayer.size.y - 2) * 0.5f;

        for (int i = 0; i < 5; i++)
        {
            // !!! 5마리만 스폰
            Vector3 diff = new Vector3(
                Random.Range(-1 * horizontalRange, horizontalRange),
                Random.Range(-1 * verticalRange, verticalRange),
                0f
            );

            spots.Add(this.transform.position + diff);
        }

        _spawner.Set(spots);
        //_spawner.CreateEnemy
    }
}
