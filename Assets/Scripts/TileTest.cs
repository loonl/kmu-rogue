using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileTest : MonoBehaviour
{
    public Tile tile;
    public Tilemap tileMap;

    private Vector3Int previous;

    public int offset = 0;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Vector3Int currentCell = tileMap.WorldToCell(transform.position);

            currentCell.x += offset;

            if (currentCell != previous)
            {
                tileMap.SetTile(currentCell, tile);
                // tileMap.GetTransformMatrix(currentCell).rotation = Quaternion.Euler(0f, 0f, -90f);
                Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, offset * -90f), Vector3.one);
                tileMap.SetTransformMatrix(currentCell, matrix);
                // tileMap.SetTile(previous, null);

                // previous = currentCell;
                offset += 1;
            }
        }
    }
}
