using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boxes : MonoBehaviour
{
    public void Set(int number, RoomDirect direct)
    {
        switch (number)
        {
            case 1:
                CreateObject(direct).transform.localPosition = new Vector3(-.25f, .25f, -.1f);
                break;
            case 2:
                CreateObject(direct).transform.localPosition = new Vector3(-.25f, .25f, -.1f);
                if (Random.value < 0.5)
                {
                    CreateObject(direct).transform.localPosition = new Vector3(.25f, .25f, -.1f);
                }
                else
                {
                    CreateObject(direct).transform.localPosition = new Vector3(-.25f, -.25f, -.1f);
                }
                break;
            case 3:
                CreateObject(direct).transform.localPosition = new Vector3(-.25f, .25f, -.1f);
                CreateObject(direct).transform.localPosition = new Vector3(.25f, .25f, -.1f);
                CreateObject(direct).transform.localPosition = new Vector3(-.25f, -.25f, -.1f);
                break;
        }

    }

    private GameObject CreateObject(RoomDirect direct)
    {
        Object obj = Random.value < 0.5
            ? Resources.Load("Prefabs/Dungeon/barrel")
            : Resources.Load("Prefabs/Dungeon/box");
        // Object obj = Resources.Load("Prefabs/Dungeon/box");

        GameObject box = Instantiate(obj) as GameObject;
        box.transform.SetParent(this.transform);

        switch (direct)
        {
            case RoomDirect.Right:
                box.transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case RoomDirect.Down:
                box.transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case RoomDirect.Left:
                box.transform.rotation = Quaternion.Euler(0, 0, 270);
                break;
        }

        return box;
    }
}
