using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity {
    // �̵� �ӵ� �����ϴ� ����
    public float speed;
    public float damage;
    public bool isAttacking;

    Animator anim;

    void Start()
    {
        anim = transform.GetChild(0).gameObject.GetComponent<Animator>();

        // �ִϸ������� �ִϸ��̼��� ������ ȣ���� �Լ��� ���� - Die
        anim.GetComponent<PlayerAnimreciver>().onDieComplete = () =>
        {
            // Die �ִϸ��̼� ����� ������ ��Ȱ��ȭ
            gameObject.SetActive(false);
        };

        // �ִϸ������� �ִϸ��̼��� ������ ȣ���� �Լ� ���� - Attack
        anim.GetComponent<PlayerAnimreciver>().onAttackComplete = () =>
        {
            // Attack �ִϸ��̼� ����� ������ ����� �����ϰ� ��ȯ
            isAttacking = false;
        };


        // �ʱ� ���� ����
        maxHealth = 100.0f;
        health = maxHealth;
        damage = 25.0f;
        speed = 3.0f;
    }

    void Update()
    {
        // �׾����� input �� �ް�
        if (dead)
            return;

        // �����¿� ������ input ��������
        // �밢�� ���̵� 1�� �����ֱ� ���ؼ� normalize
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized;

        // �¿� �̵��� ���� ĳ���� �������ֱ�
        if (moveInput.x >
            0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (moveInput.x < 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        // �����¿� ������ ����
        transform.position += moveInput * Time.deltaTime * speed;

        // �ִϸ��̼� ����
        anim.SetFloat("Speed", moveInput.magnitude);

        if (Input.GetButtonDown("Fire1") && !isAttacking)
        {
            // �ִϸ��̼� Attack���� ����
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

    // �ǰ� ó��
    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);

        // ü���� ������ 0���� ����
        if (health < 0f)
            health = 0f;

        // �ִϸ��̼� Stunned�� ����
        anim.SetTrigger("Hit");

        // debug
        print(health);
    }

    // ��� ó��
    public override void Die()
    {
        base.Die();

        // �ִϸ��̼� Death�� ����
        anim.SetTrigger("Die");
    }
}
