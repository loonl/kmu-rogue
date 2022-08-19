using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    private int gold = 0;
    TextMeshProUGUI coinTXT;
    public int Gold { get { return gold; } }
    private void Start()
    {
        coinTXT = GameObject.Find("GoldCnt").GetComponent<TextMeshProUGUI>();
    }

    // -------------------------------------------------------------
    // 골드
    // -------------------------------------------------------------

    public void UpdateGold(int diff)
    {
        gold += diff;
        Debug.Log($"Update gold: {gold}");      // !!! TEMP
        coinTXT.text = gold.ToString();
        // UIManager.Instance.UpdateGoldText(gold);
    }
}
