using System.Collections;
using UnityEngine;

public class ZombieBoss : Monster
{
    bool rushing = false; // 돌진 중 여부
    int rushStep = 0; // 돌진 횟수
    protected float rushCoolTime = 0.6f; // 돌진 쿨타임
    protected float lastRushTime = 1f; // 마지막 돌진 시점

    bool rushReady = false; // 돌진 준비중 여부
    protected float timeForRushReady = 1f; // 돌진 준비시간

    // 몬스터 초기화
    protected override void Setup()
    {
        maxHealth = 400f;
        corpseHealth = 75f;
        damage = 20f;
        speed = 1f;

        base.Setup();
    }

    protected override void Update()
    {
        // 현재 행동 수행
        if (dead)  // **플레이어 사망여부 추가
        {
            rigidbody2d.velocity = Vector2.zero;
        }
        else if (action == "moving")
        {
            Moving();
        }
        else if (action == "rushing")
        {
            Rushing();
        }

        // 추적 대상의 존재 여부에 따라 다른 애니메이션을 재생
        animator.SetBool("HasTarget", hasTarget);
    }

    // 돌진 수행
    protected void Rushing()
    {
        if (Time.time < lastRushTime + timeForRushReady)
        {
            rigidbody2d.velocity = Vector2.zero;
        }
        else if (rushReady == true)
        {
            rushReady = false;
            rigidbody2d.AddForce(direction * 300f);
            damage = 40f;
        }
        else if (Time.time >= lastRushTime + timeForRushReady + 0.5f)
        {
            action = "moving";
            damage = 20f;
            rushing = false;
            if (rushStep <= 2)
            {
                rushStep++;
                rushCoolTime = 0.6f;
                timeForRushReady = 0.1f;
            }
            else
            {
                rushStep = 0;
                rushCoolTime = 5.5f;
                timeForRushReady = 1f;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (dead)
        {
            return;
        }

        // 목표가 가까워지면 돌진
        if (Time.time >= lastRushTime + rushCoolTime && rushing == false)
        {
            Entity entity = other.gameObject.GetComponent<Entity>();
            if (targetEntity == entity)
            {
                action = "rushing";
                lastRushTime = Time.time;
                rushing = true;
                rushReady = true;
            }
        }
    }
}

// 0 대기 돌진
// 1 돌진
// 2 돌진 쿨타임
// 