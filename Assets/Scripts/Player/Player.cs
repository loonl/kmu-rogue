using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity {
    // public variables
    public float speed;
    public float damage;
    public float attackRange; // 0 ~ 25
    public bool isAttacking;

    Animator anim;
    WeaponCollider wpnColl;

    void Start()
    {
        anim = transform.GetChild(0).gameObject.GetComponent<Animator>();
        wpnColl = transform.GetChild(0).gameObject.GetComponent<WeaponCollider>();

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


        // variables init
        maxHealth = 100.0f;
        attackRange = 5.0f;
        health = maxHealth;
        damage = 25.0f;
        speed = 3.0f;
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
        transform.position += moveInput * Time.deltaTime * speed;

        // change animation depending on speed
        anim.SetFloat("Speed", moveInput.magnitude);

        if (Input.GetButtonDown("Fire1") && !isAttacking)
        {
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