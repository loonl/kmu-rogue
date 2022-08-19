using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    public static GameManager Instance { get { return _instance; } }
    public Player player; // TO-DO <- Player를 인스턴스화?

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
        DungeonSystem.Instance.CreateDungeon();
        DungeonSystem.Instance.Rooms[0].Clear();    // 첫번째 방은 클리어 된 상태
    }

    // -------------------------------------------------------------
    // Player 아이템 착용 / 해제
    // -------------------------------------------------------------
    public void Equip(Item item)
    {
        var spum = player.spumMgr;

        // 바뀌는 장비가 어느 부위인지 판단
        int partsIndex;
        int type = item.itemType;
        if (type == 0 || type == 1 || type == 2)
            partsIndex = 0;
        else
            partsIndex = item.itemType - 3;

        // 입고 있는 것 먼저 un-equip
        UnEquip(player.equipment[partsIndex]);

        // 플레이어 스탯 수정
        List<Stat> itemStat = new List<Stat> { item.stat };
        player.stat.SyncStat(itemStat);


        // TO-DO 상의 필요 (근접 무기 -> 활 / 스태프로 무기 변경 시 방패 자동 장착 해제)
        if (partsIndex == 0)
        {
            if (player.equipment[0].itemType == 1 && (item.itemType == 2 || item.itemType == 3))
            {
                UnEquip(player.equipment[4]);
                player.equipment[4] = ItemManager.Instance.GetItem(94);
            }
        }

        // 활 / 스태프 상태에서 방패 장착 시 활 / 스태프 자동 장착 해제
        else if (partsIndex == 4)
        {
            if (player.equipment[0].itemType == 2 || player.equipment[0].itemType == 3)
            {
                UnEquip(player.equipment[0]);
                player.equipment[0] = ItemManager.Instance.GetItem(1);
            }
        }

        // 플레이어 외형 수정
        switch (type)
        {
            case 1: // sword
                spum._weaponListString[0] = item.path;
                spum.SyncPath(spum._weaponList, spum._weaponListString);
                break;
            case 2: // bow
            case 3: // staff
                spum._weaponListString[2] = item.path;
                spum.SyncPath(spum._weaponList, spum._weaponListString);
                break;
            case 4: // helmet
                spum._hairListString[1] = item.path;
                spum.SyncPath(spum._hairList, spum._hairListString);
                break;
            case 5: // armor
                spum._armorListString[0] = spum._armorListString[1] =
                    spum._armorListString[2] = item.path;
                spum.SyncPath(spum._armorList, spum._armorListString);
                break;
            case 6: // pants
                spum._pantListString[0] = spum._pantListString[1] = item.path;
                spum.SyncPath(spum._pantList, spum._pantListString);
                break;
            case 7: // shield
                spum._weaponListString[3] = item.path;
                spum.SyncPath(spum._weaponList, spum._weaponListString);
                break;
        }

        // 장착 슬롯에 아이템 추가
        player.equipment[partsIndex] = item;
    }

    public void UnEquip(Item item)
    {
        var spum = player.spumMgr;
        int type = item.itemType;

        // 플레이어 스탯 수정
        List<Stat> itemStat = new List<Stat> { item.stat };
        player.stat.SubStat(itemStat);

        // Shield나 무기라면 플레이어 외형 수정
        if (type == 7)
        {
            spum._weaponListString[3] = "";
            spum.SyncPath(player.spumMgr._weaponList, player.spumMgr._weaponListString);
        }
        else if (type == 1)
        {
            spum._weaponListString[0] = "";
            spum.SyncPath(player.spumMgr._weaponList, player.spumMgr._weaponListString);
        }
        else if (type == 2 || type == 3)
        {
            spum._weaponListString[2] = "";
            spum.SyncPath(player.spumMgr._weaponList, player.spumMgr._weaponListString);
        }
    }

    // -------------------------------------------------------------
    // 프리팹 생성
    // -------------------------------------------------------------
    public GameObject CreateGO(string url, Transform parent)
    {
        Object obj = Resources.Load(url);
        GameObject go = Instantiate(obj) as GameObject;
        go.transform.SetParent(parent);
        
        return go;
    }
}
