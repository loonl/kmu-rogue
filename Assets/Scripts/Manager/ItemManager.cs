using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private static List<Dictionary<string, object>> data;
    private static ItemManager _instance = null;
    public static ItemManager Instance
    { 
        get
        {
            if (_instance == null)
            {
                _instance = new ItemManager();
            }

            return _instance;
        }
    }

    public void TempGet(int itemId)
    {
        if (data == null)
        {
            data = CSVReader.Read("Datas/Item");
        }
        Debug.Log(data[itemId]["tempA"]);
        Debug.Log(data[itemId]["tempB"]);
    }

    //public static Item Get(int itemId)
    //{
    //    return;
    //}
}
