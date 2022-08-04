using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity {
    // player stat variables
    public float speed;
    public float attackDamage;
    public float skillDamage;
    public bool isAttacking;
    public float remainCool;

    // equipments
    public WeaponInfo curWeapon; // used in weaponcollider.cs
    ArmourInfo curShield;
    ArmourInfo curArmor;
    ArmourInfo curPants;
    ArmourInfo curHelmet;

    Animator anim;
    WeaponCollider wpnColl;
    SPUM_SpriteList spumMgr;
    Rigidbody2D rig;
    PlayerParts parts;
    PlayerSkillCollider skillColl;
    Camera cam;

    void Start()
    {
        anim = transform.GetChild(0).gameObject.GetComponent<Animator>();
        wpnColl = transform.GetChild(0).gameObject.GetComponent<WeaponCollider>();
        spumMgr = transform.GetChild(0).GetChild(0).GetComponent<SPUM_SpriteList>();
        rig = GetComponent<Rigidbody2D>();
        parts = GetComponent<PlayerParts>();
        skillColl = GameObject.Find("PlayerSkill_HitCircle").GetComponent<PlayerSkillCollider>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();

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

            // disable skill collider
            skillColl.circle.enabled = false;

            // reset elasped skill cool-time
            remainCool = curWeapon.cooltime;

            // clear skill collider monster list
            if (skillColl.monsters.Count > 0)
            {
                skillColl.monsters.Clear();
            }
        };

        // first equipment init
        curWeapon = parts.weaponsList[1];
        curShield = parts.shieldsList[0];
        curHelmet = parts.helmetsList[0];
        curArmor = parts.armorsList[0];
        curPants = parts.pantsList[0];

        // player stat variables init
        maxHealth = 100.0f + curShield.extrahp + curHelmet.extrahp + curArmor.extrahp + curPants.extrahp;
        health = maxHealth;
        attackDamage = 1.0f + curWeapon.attackPower;
        skillDamage = curWeapon.skillPower;
        speed = 2.5f;
        remainCool = -1.0f;

        // attack & skill range init
        wpnColl.SetAttackRange(curWeapon.attackRange);
        skillColl.SetAttackRange(curWeapon.skillRange);
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
        rig.velocity = moveInput * speed;

        // change animation depending on speed
        anim.SetFloat("Speed", moveInput.magnitude * speed);


        /**
        * Input Handling
        */

        // test PlayerChange
        // 0 = Helmet, 1 = Armor, 2 = Pants, 3 = Shields, 4 = Weapons
        if (Input.GetKeyDown(KeyCode.G))
        {
            ChangePlayerParts(0, parts.helmetsList.IndexOf(curHelmet) + 1);
        } 

        if (Input.GetKeyDown(KeyCode.H))
        {
            ChangePlayerParts(1, parts.armorsList.IndexOf(curArmor) + 1);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            ChangePlayerParts(2, parts.pantsList.IndexOf(curPants) + 1);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            ChangePlayerParts(3, parts.shieldsList.IndexOf(curShield) + 1);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            ChangePlayerParts(4, parts.weaponsList.IndexOf(curWeapon) + 1);
        }

        // Attack Input
        if (Input.GetButtonDown("Fire1") && !isAttacking)
        {
            // update weapon state
            anim.SetInteger("WpnState", (int)curWeapon.weapontype);

            // change animation to attack
            anim.SetTrigger("Attack");
            isAttacking = true;

            // enable weapon collider
            wpnColl.poly.enabled = true;
        }

        // test onDamage
        if (Input.GetKeyDown(KeyCode.P))
        {
            print("onDamageTest");
            OnDamage(25.0f);
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
            anim.SetInteger("WpnState", (int)curWeapon.weapontype);

            // change animation to skill
            anim.SetTrigger("Skill");

            // do not let attack and use skill at the same time
            isAttacking = true;

            // track mouse position and locate skill collider there
            Vector2 mousePos = Input.mousePosition;
            mousePos = cam.ScreenToWorldPoint(mousePos);
            skillColl.transform.position = mousePos;

            // enable skill collider
            skillColl.circle.enabled = true;
        }
    }

    // change player's equipment parts
    private void ChangePlayerParts(int ver, int index)
    {
        switch (ver)
        {
            case 0: // helmet
                // change info & re-sync data
                spumMgr._hairListString[1] = parts.helmetsList[index].info;
                curHelmet = parts.helmetsList[index];
                UpdateHP();
                spumMgr.SyncPath(spumMgr._hairList, spumMgr._hairListString);
                break;

            case 1: // armors
                // change info & re-sync data
                spumMgr._armorListString[0] = spumMgr._armorListString[1] =
                    spumMgr._armorListString[2] = parts.armorsList[index].info;
                curArmor = parts.armorsList[index];
                UpdateHP();
                spumMgr.SyncPath(spumMgr._armorList, spumMgr._armorListString);
                break;

            case 2: // pants
                // change info & re-sync data
                spumMgr._pantListString[0] = spumMgr._pantListString[1] =
                    parts.pantsList[index].info;
                curPants = parts.pantsList[index];
                UpdateHP();
                spumMgr.SyncPath(spumMgr._pantList, spumMgr._pantListString);
                break;

            case 3: // shield
                // change info & re-sync data
                spumMgr._weaponListString[3] = parts.shieldsList[index].info;
                curShield = parts.shieldsList[index];

                // if player is equipping left-handed weapons, then un-equip
                if (spumMgr._weaponListString[2] != "")
                {
                    curWeapon = parts.weaponsList[0];
                    spumMgr._weaponListString[2] = "";
                }

                UpdateHP();
                spumMgr.SyncPath(spumMgr._weaponList, spumMgr._weaponListString);
                break;

            case 4: // weapon
                // weapon depends on weapon_type 
                WeaponInfo nextWeapon = parts.weaponsList[index];
                if (nextWeapon.weapontype == WeaponType.Melee)
                    spumMgr._weaponListString[0] = nextWeapon.info;
                else // if weapon type == bow or staff
                {
                    spumMgr._weaponListString[0] = ""; // un-equip right hand weapon
                    spumMgr._weaponListString[3] = ""; // cannot equip shields
                    curShield = parts.shieldsList[0];
                    spumMgr._weaponListString[2] = nextWeapon.info;
                }

                curWeapon = parts.weaponsList[index];
                UpdateStats();
                spumMgr.SyncPath(spumMgr._weaponList, spumMgr._weaponListString);
                break;
        }
    }

    // update player stats & skill ranges
    private void UpdateStats()
    {
        attackDamage = 1.0f + curWeapon.attackPower;
        skillDamage = curWeapon.skillPower;
        wpnColl.SetAttackRange(curWeapon.attackRange);
        skillColl.SetAttackRange(curWeapon.skillRange);
    }

    // update max hp when armour is changed
    private void UpdateHP()
    {
        maxHealth = 100.0f + curShield.extrahp + curHelmet.extrahp + curArmor.extrahp + curPants.extrahp;
        if (health > maxHealth)
            health = maxHealth;
    }

    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);

        // adjust health to 0 if minus
        if (health < 0f)
            health = 0f;

        // change animation to stunned
        anim.SetTrigger("Hit");

        // debug
        print("Player's health : " + health);
    }

    public override void Die()
    {
        base.Die();

        // change animation to death
        anim.SetTrigger("Die");
    }
}