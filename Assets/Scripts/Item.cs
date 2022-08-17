using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public new string name { get; set; } // TO-DO 바꿔야 할 수도?
    public int itemType { get; set; }
    public Stat stat { get; set; }
    public string path { get; set; }

    public Item(Dictionary<string, object> data)
    {
        name = (string) data["name"];
        stat = new Stat((int)data["hp"], (int)data["dmg"], (int)data["range"], 
                        (int)data["skilldmg"], (int)data["cooltime"], (int)data["speed"]);
        itemType = (int) data["type"];
        path = (string) data["path"];
    } 
}
