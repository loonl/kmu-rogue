using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    public static GameManager Instance { get { return _instance; } }
    public Player Player { get; private set; }

    public StageUIManager stageUIManager;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            this.Player = GameObject.Find("Player").GetComponent<Player>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        stageUIManager.init(Player.stat);
        DungeonSystem.Instance.CreateDungeon();
        DungeonSystem.Instance.Rooms[0].Clear();    // 첫번째 방은 클리어 된 상태
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
