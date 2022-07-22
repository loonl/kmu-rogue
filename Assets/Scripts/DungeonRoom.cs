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

    public Tilemap GroundLayer { get { return _groundLayer; } }
    public Tilemap WallLayer { get { return _wallLayer; } }
    public Tilemap ObjectLayer { get { return _objectLayer; } }
}
