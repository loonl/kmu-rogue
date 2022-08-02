using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Melee = 0,
    Bow = 1,
    Staff = 2
}

public struct WeaponInfo
{
    public string info;
    public WeaponType weapontype;
    public WeaponInfo(string _info, WeaponType _weapontype)
    {
        info = _info;
        weapontype = _weapontype;
    }
}

public class Player : Entity {
    // public variables
    public float speed;
    public float damage;
    public float attackRange; // 0 ~ 25
    public bool isAttacking;

    // equipments
    public List<string> helmetsList = new List<string>();
    public List<string> armorsList = new List<string>();
    public List<string> pantsList = new List<string>();
    public List<WeaponInfo> weaponsList = new List<WeaponInfo>();

    Animator anim;
    WeaponCollider wpnColl;
    SPUM_SpriteList spumMgr;
    Rigidbody2D rig;
    int curWpnIndex;

    void Start()
    {
        anim = transform.GetChild(0).gameObject.GetComponent<Animator>();
        wpnColl = transform.GetChild(0).gameObject.GetComponent<WeaponCollider>();
        spumMgr = transform.GetChild(0).GetChild(0).GetComponent<SPUM_SpriteList>();
        rig = GetComponent<Rigidbody2D>();

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

            // clear attack collider monster list
            if (wpnColl.monsters.Count > 0)
            {
                wpnColl.monsters.Clear();
            }
        };


        // variables init
        maxHealth = 100.0f;
        attackRange = 5.0f;
        health = maxHealth;
        damage = 25.0f;
        speed = 2.5f;
        curWpnIndex = 0;
    }

    void Update()
    {
        // should not work in dead condition
        if (dead)
            return;

        // get input
        // normalize for making speed 1
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized;

        // flip character depending on heading direction
        if (moveInput.x >
            0 && transform.localScale.x > 0)
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
        // 0 = Helmet, 1 = Armor, 2 = Pants, 3 = Weapons
        if (Input.GetKeyDown(KeyCode.H))
        {
            PlayerPartsChange(0, Random.Range(0, helmetsList.Count));
        } 

        if (Input.GetKeyDown(KeyCode.J))
        {
            PlayerPartsChange(1, Random.Range(0, armorsList.Count));
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            PlayerPartsChange(2, Random.Range(0, pantsList.Count));
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            PlayerPartsChange(3, Random.Range(0, weaponsList.Count));
        }

        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            PlayerPartsChange(3, 163);
        }

        if (Input.GetKeyDown(KeyCode.Quote))
        {
            PlayerPartsChange(3, 167);
        }

        // Attack Input
        if (Input.GetButtonDown("Fire1") && !isAttacking)
        {
            // update weapon state
            anim.SetInteger("WpnState", (int)weaponsList[curWpnIndex].weapontype);

            // change animation to attack
            anim.SetTrigger("Attack");
            isAttacking = true;
        }

        // test onDamage
        if (Input.GetKeyDown(KeyCode.P))
        {
            print("onDamageTest");
            OnDamage(damage);
        }

        // test skill input
        if (Input.GetKeyDown(KeyCode.O))
        {
            // update weapon state
            anim.SetInteger("WpnState", (int)weaponsList[curWpnIndex].weapontype);

            // change animation to skill
            anim.SetTrigger("Skill");
            isAttacking = true;
        }
    }

    // change player's equipment parts
    private void PlayerPartsChange(int ver, int index)
    {
        switch (ver)
        {
            case 0: // helmet
                var temp = helmetsList[index].Split(",");

                // insert new equipment's data into spumSpriteList
                for (int i = 0; i < temp.Length; i++) 
                    spumMgr._hairListString[i] = temp[i];

                // re-sync data
                spumMgr.SyncPath(spumMgr._hairList, spumMgr._hairListString);
                break;

            case 1: // armors
                var temp2 = armorsList[index].Split(",");

                // insert new equipment's data into spumSpriteList
                for (int i = 0; i < temp2.Length; i++)
                    spumMgr._armorListString[i] = temp2[i];

                // re-sync data
                spumMgr.SyncPath(spumMgr._armorList, spumMgr._armorListString);
                break;
            case 2: // pants
                var temp3 = pantsList[index].Split(",");

                // insert new equipment's data into spumSpriteList
                for (int i = 0; i < temp3.Length; i++)
                    spumMgr._pantListString[i] = temp3[i];

                // re-sync data
                spumMgr.SyncPath(spumMgr._pantList, spumMgr._pantListString);
                break;
            case 3: // weapon
                curWpnIndex = index;
                var temp4 = weaponsList[index].info.Split(",");

                // insert new equipment's data into spumSpriteList
                for (int i = 0; i < temp4.Length; i++)
                    spumMgr._weaponListString[i] = temp4[i];

                // re-sync data
                spumMgr.SyncPath(spumMgr._weaponList, spumMgr._weaponListString);
                break;
        }
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