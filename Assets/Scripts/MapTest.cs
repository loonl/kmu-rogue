using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTest : MonoBehaviour
{
    [SerializeField]
    private DungeonSystem dungeonSystem;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dungeonSystem.ClearDungeon();
            dungeonSystem.CreateDungeon();
        }
    }
}
