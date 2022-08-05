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

    // Start is called before the first frame update
    void Start()
    {
        coinTXT = coin.GetComponent<TextMeshProUGUI>();

        bossUI = GameObject.Find("BossHp");
        bossHpbar = bossUI.GetComponent<Slider>();
        bossUI.SetActive(false);

        playerUI = GameObject.Find("PlayerHp");
        hpbar = playerUI.GetComponent<Slider>();
        playerUI.SetActive(false);

        addCoin(0);
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
