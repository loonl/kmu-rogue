using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatus : MonoBehaviour
{
    // Start is called before the first frame update
    TextMeshProUGUI statusTxt;
    Player playerInfo;
    void Awake()
    {
        playerInfo = GameObject.Find("Player").GetComponent<Player>();
        statusTxt = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnEnable()
    {
        if (statusTxt != null)
        {
            statusTxt.text = "HP : " + playerInfo.GetHP() + "\n\n" + "SPEED : " + playerInfo.speed + "\n\n" + "ATK : " + playerInfo.attackDamage + "\n\n" + "SKILL DAMAGE : " + playerInfo.skillDamage;
        }
    }
}
