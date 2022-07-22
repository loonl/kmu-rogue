using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity {
    // 이동 속도 조절하는 변수
    public float speed;
    public float damage;
    public bool isAttacking;

    Animator anim;

    void Start()
    {
        anim = transform.GetChild(0).gameObject.GetComponent<Animator>();

        // 애니메이터의 애니메이션이 끝나면 호출할 함수들 정의 - Die
        anim.GetComponent<PlayerAnimreciver>().onDieComplete = () =>
        {
            // Die 애니메이션 출력이 끝나면 비활성화
            gameObject.SetActive(false);
        };

        // 애니메이터의 애니메이션이 끝나면 호출할 함수 정의 - Attack
        anim.GetComponent<PlayerAnimreciver>().onAttackComplete = () =>
        {
            // Attack 애니메이션 출력이 끝나면 재공격 가능하게 변환
            isAttacking = false;
        };


        // 초기 변수 세팅
        maxHealth = 100.0f;
        health = maxHealth;
        damage = 25.0f;
        speed = 3.0f;
    }

    void Update()
    {
        // 죽었으면 input 안 받게
        if (dead)
            return;

        // 상하좌우 움직임 input 가져오기
        // 대각선 길이도 1로 맞춰주기 위해서 normalize
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized;

        // 좌우 이동에 따라 캐릭터 뒤집어주기
        if (moveInput.x >
            0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (moveInput.x < 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        // 상하좌우 움직임 조절
        transform.position += moveInput * Time.deltaTime * speed;

        // 애니메이션 변경
        anim.SetFloat("Speed", moveInput.magnitude);

        if (Input.GetButtonDown("Fire1") && !isAttacking)
        {
            // 애니메이션 Attack으로 변경
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

    // 피격 처리
    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);

        // 체력이 음수면 0으로 조정
        if (health < 0f)
            health = 0f;

        // 애니메이션 Stunned로 변경
        anim.SetTrigger("Hit");

        // debug
        print(health);
    }

    // 사망 처리
    public override void Die()
    {
        base.Die();

        // 애니메이션 Death로 변경
        anim.SetTrigger("Die");
    }
}
