using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class StageUIManager : MonoBehaviour
{
    GameObject playerUI;
    public Slider hpbar;
    public Entity playerInfo;

    GameObject bossUI;
    Slider bossHpbar;
    Entity curBoss;

    GameObject backIMG;
    GameObject statusFrame;
    bool openStatus = false;

    // Start is called before the first frame update
    private void Awake()
    {
        backIMG = GameObject.Find("BackPannel");
        statusFrame = GameObject.Find("StatusFrame");
        bossUI = GameObject.Find("BossHp");
        playerUI = GameObject.Find("PlayerHp");

    }
    void Start()
    {
        bossHpbar = bossUI.GetComponent<Slider>();
        hpbar = playerUI.GetComponent<Slider>();
        playerInfo = GameObject.Find("Player").GetComponent<Player>();

        backIMG.SetActive(false);
        statusFrame.SetActive(false);
        bossUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (bossUI.activeSelf)
        {
            bossHpbar.value = curBoss.GetHP() / curBoss.GetMaxHP();
        }

        if (playerUI.activeSelf)
        {
            hpbar.value = playerInfo.GetHP() / playerInfo.GetMaxHP();
        }

        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
        {
            RoadStatus();
        }
    }

    public void RoadBossUI(Monster boss)
    {
        curBoss = boss.GetComponent<Monster>();
        bossUI.SetActive(true);
    }

    public void RoadStatus()
    {
        openStatus = !openStatus;
        backIMG.SetActive(openStatus);
        statusFrame.SetActive(openStatus);
        if (openStatus)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
