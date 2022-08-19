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
        stat = new Stat(float.Parse(data["hp"].ToString()), float.Parse(data["dmg"].ToString()), float.Parse(data["range"].ToString()), 
                        float.Parse(data["skilldmg"].ToString()), float.Parse(data["cooltime"].ToString()), float.Parse(data["speed"].ToString()));
        itemType = (int) data["type"];
        path = (string) data["path"];
    } 
}
