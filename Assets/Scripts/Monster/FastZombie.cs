using UnityEngine;

public class FastZombie : Monster
{
    // 몬스터 초기화
    protected override void Setup()
    {
        maxHealth = 125f;
        corpseHealth = 75f;
        damage = 20f;
        speed = 1f;

        base.Setup();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (dead)
        {
            return;
        }

        // 목표가 가까워지면 이동속도 증가
        Entity attackTarget = other.gameObject.GetComponent<Entity>();
        if (targetEntity == attackTarget)
        {
            speed = 2f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (dead)
        {
            return;
        }

        // 목표 멀어지면 이동속도 감소
        Entity attackTarget = other.gameObject.GetComponent<Entity>();
        if (targetEntity == attackTarget)
        {
            speed = 1f;
        }
    }
}
