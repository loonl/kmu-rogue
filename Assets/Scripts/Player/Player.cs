using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity {
    // 이동 속도 조절하는 변수
    public float speed;

    Animator anim;

    void Start()
    {
        anim = transform.Find("UnitRoot").gameObject.GetComponent<Animator>();
    }

    void Update()
    {
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

        if (Input.GetButtonDown("Fire1"))
        {
            // 애니메이션 Attack으로 변경
            anim.SetTrigger("Attack");
        }

        // test onDamage
        if (Input.GetKeyDown(KeyCode.P))
        {
            print("onDamageTest");
            OnDamage(15.0f);
        }

    }

    // 피격 처리
    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);

        // 체력이 음수면 0으로 조정
        if (health < 0f)
            health = 0f;

        // debug
        print(health);
    }

    // 사망 처리
    public override void Die()
    {
        base.Die();

        // 애니메이션 Death로 변경
        anim.SetTrigger("Die");

        Invoke("Hide", 0.5f);
    }

    // 사망 애니메이션 재생 후 플레이어 제거
    public void Hide() {
        gameObject.SetActive(false);
    }
}
