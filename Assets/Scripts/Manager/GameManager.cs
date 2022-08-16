using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    public static GameManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        ItemManager.Instance.TempGet(0);
        ItemManager.Instance.TempGet(1);
    }

    // -------------------------------------------------------------
    // Player 아이템 착용 / 해제
    // -------------------------------------------------------------
    public void Equip()
    {
        // 플레이어 스탯 수정
        // 플레이어 외형 수정

        // 장착 슬롯에 아이템 추가
    }

    public void UnEquip()
    {
        // ...
    }
}
