using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTest : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            DungeonSystem.Instance.ClearDungeon();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DungeonSystem.Instance.ClearDungeon();
            DungeonSystem.Instance.CreateDungeon();
        }
    }
}
