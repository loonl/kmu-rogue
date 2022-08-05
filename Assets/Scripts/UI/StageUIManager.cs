using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class StageUIManager : MonoBehaviour
{
    GameObject playerUI;
    public Slider hpbar;
    public float maxHp;
    public Player curhp;

    public GameObject coin;
    TextMeshProUGUI coinTXT;
    int curCoin = 0;

    GameObject bossUI;
    Slider bossHpbar;
    float bossMaxHp;
    Monster curBoss;

    GameObject backIMG;
    GameObject statusFrame;
    bool openStatus = false;

    // Start is called before the first frame update
    private void Awake()
    {
        backIMG = GameObject.Find("BackPannel");
        statusFrame = GameObject.Find("StatusFrame");

        coinTXT = coin.GetComponent<TextMeshProUGUI>();

        bossUI = GameObject.Find("BossHp");

        playerUI = GameObject.Find("PlayerHp");

        addCoin(0);
    }
    void Start()
    {
        bossHpbar = bossUI.GetComponent<Slider>();
        hpbar = playerUI.GetComponent<Slider>();

        backIMG.SetActive(false);
        statusFrame.SetActive(false);
        bossUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (bossUI.activeSelf)
        {
            //bossHpbar.value = curBoss.health / bossMaxHp;
        }

        if (playerUI.activeSelf)
        {
            //hpbar.value = curhp.health / maxHp;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            openStatus = !openStatus;
            backIMG.SetActive(openStatus);
            statusFrame.SetActive(openStatus);
            if (openStatus)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }
        }
    }

    public void RoadBossUI(float maxhp, Monster boss)
    {
        curBoss = boss.GetComponent<Monster>();
        bossUI.SetActive(true);
        bossMaxHp = maxHp;
    }
    public void RoadPlayerUI(float maxPlayerHP, Player player)
    {
        curhp = player.GetComponent<Player>();
        playerUI.SetActive(true);
        maxHp = maxPlayerHP;
    }

    public void addCoin(int coin)
    {
        curCoin += coin;
        coinTXT.text = curCoin.ToString();
    }
}
