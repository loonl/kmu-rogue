using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class StageUIManager : MonoBehaviour
{
    [SerializeField]
    GameObject playerUI;
    [SerializeField]
    Slider hpbar;

    [SerializeField]
    TextMeshProUGUI playerstat;

    Stat player;
    Stat boss;

    [SerializeField]
    GameObject bossUI;
    Slider bossHpbar;

    [SerializeField]
    GameObject backIMG;
    [SerializeField]
    GameObject statusFrame;
    bool openStatus = false;
    void Start()
    {
        bossHpbar = bossUI.GetComponent<Slider>();
        hpbar = playerUI.GetComponent<Slider>();

        backIMG.SetActive(false);
        statusFrame.SetActive(false);
        bossUI.SetActive(false);
    }

    public void init(Stat player)
    {
        this.player = player;
    }

    [SerializeField]
    Image playerRightImg;
    [SerializeField]
    SpriteRenderer rightWeaponItemImg;
    [SerializeField]
    Image playerHelmetImg;
    [SerializeField]
    SpriteRenderer helmetItemImg;
    [SerializeField]
    Image playerArmorImg;
    [SerializeField]
    SpriteRenderer armorItemImg;
    [SerializeField]
    Image playerLeftFantsImg;
    [SerializeField]
    Image playerRightFantsImg;
    [SerializeField]
    SpriteRenderer fantsItemImg;

    public Image playerLeftImg;
    public SpriteRenderer leftWeaponItemImg;
    public SpriteRenderer leftShildItemImg;

    // Update is called once per frame
    void Update()
    {
        if (playerUI.activeSelf)
        {
            hpbar.value = player.hp / player.maxHp;
        }
        if (bossUI.activeSelf)
        {
            bossHpbar.value = boss.hp / boss.maxHp;
        }

        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
        {
            loadStatus();
            loadItem();
            playerstat.text = "HP : " + player.hp + "\n\n" + "SPEED : " + player.speed + "\n\n" + "ATK : " + player.damage + "\n\n" + "SKILL DAMAGE : " + player.skillDamage;
        }
    }

    public void loadBossUI(Stat boss)
    {
        this.boss = boss;
        bossUI.SetActive(true);
    }

    public void loadItem()
    {
        if (leftWeaponItemImg.sprite || leftShildItemImg.sprite)
        {
            playerLeftImg.enabled = true;

            if (leftWeaponItemImg.sprite)
                playerLeftImg.sprite = leftWeaponItemImg.sprite;
            if (leftShildItemImg.sprite)
                playerLeftImg.sprite = leftShildItemImg.sprite;

            playerLeftImg.SetNativeSize();
        }
        else
            playerLeftImg.enabled = false;

        if (helmetItemImg.sprite)
        {
            playerHelmetImg.enabled = true;
            playerHelmetImg.sprite = helmetItemImg.sprite;
            playerHelmetImg.SetNativeSize();
        }
        else
            playerHelmetImg.enabled = false;

        if (rightWeaponItemImg.sprite)
        {
            playerRightImg.enabled = true;
            playerRightImg.sprite = rightWeaponItemImg.sprite;
            playerRightImg.SetNativeSize();
        }
        else
            playerRightImg.enabled = false;

        if (armorItemImg.sprite)
        {
            playerArmorImg.enabled = true;
            playerArmorImg.sprite = armorItemImg.sprite;
            playerArmorImg.SetNativeSize();
        }
        else
            playerArmorImg.enabled = false;

        if (fantsItemImg.sprite)
        {
            playerRightFantsImg.enabled = playerLeftFantsImg.enabled = true;
            playerRightFantsImg.sprite = playerLeftFantsImg.sprite = fantsItemImg.sprite;
            playerLeftFantsImg.SetNativeSize();
            playerRightFantsImg.SetNativeSize();
        }
        else
            playerRightFantsImg.enabled = playerLeftFantsImg.enabled = false;
    }

    public void loadStatus()
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
