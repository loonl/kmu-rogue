using UnityEngine;

public class RevivalZombie : Monster
{
    protected float timeBetRevive = 5f; // 부활 대기시간
    float startReviveTime; // 부활 시작시간

    // 몬스터 초기화
    protected override void Setup()
    {
        maxHealth = 150f;
        corpseHealth = 75f;
        damage = 20f;
        speed = 1f;

        base.Setup();
    }

    protected override void Update()
    {
        // 현재 행동 수행
        if (action == "moving")
        {
            Moving();
        }
        else if (action == "reviving")
        {
            rigidbody2d.velocity = Vector2.zero;
            Reviving();
        }

        // 추적 대상의 존재 여부에 따라 다른 애니메이션을 재생
        animator.SetBool("HasTarget", hasTarget);
    }

    // 부활 행동
    protected void Reviving()
    {
        if (Time.time >= startReviveTime + timeBetRevive)
        {
            Revive();
        }
    }

    // 사망 처리 후 부활 대기
    public override void Die()
    {
        base.Die();
        startReviveTime = Time.time;
        action = "reviving";
    }

}
