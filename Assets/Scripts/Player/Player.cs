using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity {
    // �̵� �ӵ� �����ϴ� ����
    public float speed;

    Animator anim;

    void Start()
    {
        anim = transform.Find("UnitRoot").gameObject.GetComponent<Animator>();
    }

    void Update()
    {
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

        if (Input.GetButtonDown("Fire1"))
        {
            // �ִϸ��̼� Attack���� ����
            anim.SetTrigger("Attack");
        }

        // test onDamage
        if (Input.GetKeyDown(KeyCode.P))
        {
            print("onDamageTest");
            OnDamage(15.0f);
        }

    }

    // �ǰ� ó��
    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);

        // ü���� ������ 0���� ����
        if (health < 0f)
            health = 0f;

        // debug
        print(health);
    }

    // ��� ó��
    public override void Die()
    {
        base.Die();

        // �ִϸ��̼� Death�� ����
        anim.SetTrigger("Die");

        Invoke("Hide", 0.5f);
    }

    // ��� �ִϸ��̼� ��� �� �÷��̾� ����
    public void Hide() {
        gameObject.SetActive(false);
    }
}
