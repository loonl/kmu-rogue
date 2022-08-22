using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    // player stat variables
    public Stat stat = new Stat(false);
    public bool isAttacking;
    public float remainCool;
    public bool dead;

    // equipments => 0 : Weapon, 1 : Helmet, 2 : Armor, 3 : Pants, 4 : Shield
    public List<Item> equipment;

    Animator anim;
    WeaponCollider wpnColl;
    Rigidbody2D rig;
    public SPUM_SpriteList spumMgr;

    void Start()
    {
        anim = transform.GetChild(0).gameObject.GetComponent<Animator>();
        wpnColl = transform.GetChild(0).gameObject.GetComponent<WeaponCollider>();
        spumMgr = transform.GetChild(0).GetChild(0).GetComponent<SPUM_SpriteList>();
        rig = GetComponent<Rigidbody2D>();

        // player's first equipments (플레이어 첫 장비)
        equipment = new List<Item> { ItemManager.Instance.GetItem(0), // sword
                                     ItemManager.Instance.GetItem(35), // helmet
                                     ItemManager.Instance.GetItem(57), // armor
                                     ItemManager.Instance.GetItem(78), // pants
                                     ItemManager.Instance.GetItem(94)  // shield
                                    };

        for (int i = 0; i < equipment.Count; i++)
        {
            GameManager.Instance.Equip(equipment[i]);
        }

        // used in animator end event - death
        anim.GetComponent<PlayerAnimreciver>().onDieComplete = () =>
        {
            // hide character
            gameObject.SetActive(false);
        };

        // used in animator end event - attack
        anim.GetComponent<PlayerAnimreciver>().onAttackComplete = () =>
        {
            // enable re-attack
            isAttacking = false;

            // disable attack collider
            wpnColl.poly.enabled = false;

            // clear attack collider monster list
            if (wpnColl.monsters.Count > 0) 
            {
                wpnColl.monsters.Clear();
            }
        };

        // used in animator end event - skill
        anim.GetComponent<PlayerAnimreciver>().onSkillComplete = () =>
        {
            // enable re-attack
            isAttacking = false;

            // reset elasped skill cool-time
            remainCool = equipment[0].stat.coolTime;
        };

        // player stat variables init
        dead = false;
        remainCool = -1f;

        // attack & skill range init
        wpnColl.SetAttackRange(equipment[0].stat.range);
    }

    void Update()
    {
        remainCool -= Time.deltaTime;

        // should not work in dead condition
        if (dead)
            return;

        // get move-related input
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized;

        // FLIP character depending on heading direction
        if (moveInput.x > 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (moveInput.x < 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        // change character's position
        rig.velocity = moveInput * stat.speed;

        // change animation depending on speed
        anim.SetFloat("Speed", moveInput.magnitude * stat.speed);


        /**
        * Input Handling
        */

        // Attack Input
        if (Input.GetButtonDown("Fire1") && !isAttacking)
        {
            // update weapon state
            anim.SetInteger("WpnState", equipment[0].itemType);

            // change animation to attack
            anim.SetTrigger("Attack");
            isAttacking = true;

            // enable weapon collider
            wpnColl.poly.enabled = true;
        }

        // test debugging skill cool-time
        if (Input.GetButtonDown("Fire2"))
        {
            print("remaining skill cool : " + remainCool);
        }

        // test skill input
        if (Input.GetButtonDown("Fire2") && remainCool <= 0.0f && !isAttacking)
        {
            // update weapon state
            anim.SetInteger("WpnState", equipment[0].itemType);

            // change animation to skill
            anim.SetTrigger("Skill");

            // do not let attack and use skill at the same time
            isAttacking = true;

            // TO-DO
            // 스킬 관련 구현
        }

        // test code - change equipments
        if (Input.GetKeyDown(KeyCode.G)) // helmet
            GameManager.Instance.Equip(ItemManager.Instance.GetItem(Random.Range(35, 56)));

        if (Input.GetKeyDown(KeyCode.H)) // armor
            GameManager.Instance.Equip(ItemManager.Instance.GetItem(Random.Range(57, 77)));

        if (Input.GetKeyDown(KeyCode.J)) // pants
            GameManager.Instance.Equip(ItemManager.Instance.GetItem(Random.Range(78, 93)));

        if (Input.GetKeyDown(KeyCode.K)) // shield
            GameManager.Instance.Equip(ItemManager.Instance.GetItem(Random.Range(94, 103)));

        if (Input.GetKeyDown(KeyCode.B)) // sword
            GameManager.Instance.Equip(ItemManager.Instance.GetItem(Random.Range(1, 14)));

        if (Input.GetKeyDown(KeyCode.N)) // bow
            GameManager.Instance.Equip(ItemManager.Instance.GetItem(Random.Range(28, 31)));

        if (Input.GetKeyDown(KeyCode.M)) // staff
            GameManager.Instance.Equip(ItemManager.Instance.GetItem(Random.Range(32, 34)));

        if (Input.GetKeyDown(KeyCode.P))
        {
            print("MaxHP : " + stat.maxHp + "\nHP : " + stat.hp + "\nAttackPower : " + stat.damage
               + "\nAttackRange : " + stat.range + "\nSkillPower : " + stat.skillDamage
               + "\nSpeed : " + stat.speed + "\nCoolTime : " + stat.coolTime);
        }
    }

    // change player's equipment parts
    //private void ChangePlayerParts(int ver, int index)
    //{
    //    switch (ver)
    //    {
    //        case 0: // helmet
    //            // change info & re-sync data
    //            spumMgr._hairListString[1] = parts.helmetsList[index].info;
    //            curHelmet = parts.helmetsList[index];
    //            UpdateHP();
    //            spumMgr.SyncPath(spumMgr._hairList, spumMgr._hairListString);
    //            break;

    //        case 1: // armors
    //            // change info & re-sync data
    //            spumMgr._armorListString[0] = spumMgr._armorListString[1] =
    //                spumMgr._armorListString[2] = parts.armorsList[index].info;
    //            curArmor = parts.armorsList[index];
    //            UpdateHP();
    //            spumMgr.SyncPath(spumMgr._armorList, spumMgr._armorListString);
    //            break;

    //        case 2: // pants
    //            // change info & re-sync data
    //            spumMgr._pantListString[0] = spumMgr._pantListString[1] =
    //                parts.pantsList[index].info;
    //            curPants = parts.pantsList[index];
    //            UpdateHP();
    //            spumMgr.SyncPath(spumMgr._pantList, spumMgr._pantListString);
    //            break;

    //        case 3: // shield
    //            // change info & re-sync data
    //            spumMgr._weaponListString[3] = parts.shieldsList[index].info;
    //            curShield = parts.shieldsList[index];

    //            // if player is equipping left-handed weapons, then un-equip
    //            if (spumMgr._weaponListString[2] != "")
    //            {
    //                curWeapon = parts.weaponsList[0];
    //                spumMgr._weaponListString[2] = "";
    //            }

    //            UpdateHP();
    //            spumMgr.SyncPath(spumMgr._weaponList, spumMgr._weaponListString);
    //            break;

    //        case 4: // weapon
    //            // weapon depends on weapon_type 
    //            WeaponInfo nextWeapon = parts.weaponsList[index];
    //            if (nextWeapon.weapontype == WeaponType.Melee)
    //                spumMgr._weaponListString[0] = nextWeapon.info;
    //            else // if weapon type == bow or staff
    //            {
    //                spumMgr._weaponListString[0] = ""; // un-equip right hand weapon
    //                spumMgr._weaponListString[3] = ""; // cannot equip shields
    //                curShield = parts.shieldsList[0];
    //                spumMgr._weaponListString[2] = nextWeapon.info;
    //            }

    //            curWeapon = parts.weaponsList[index];
    //            UpdateStats();
    //            spumMgr.SyncPath(spumMgr._weaponList, spumMgr._weaponListString);
    //            break;
    //    }
    //}

    public void OnDamage(float damage)
    {
        stat.Damaged(damage);

        // trigger die if health is below 0
        if (stat.hp == 0)
            Die();

        // change animation to stunned
        anim.SetTrigger("Hit");

        // debug
        print("Player's health : " + stat.hp);
    }

    public void Die()
    {
        // change animation to death
        anim.SetTrigger("Die");
    }
}