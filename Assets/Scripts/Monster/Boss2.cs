using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : Monster
{
    protected float timeBetSkill = 5f; // ��ų ���ð�
    protected float lastSkillTime; // ��ų ���۽ð�

    protected override IEnumerator Moving()
    {
        lastSkillTime = Time.time;
        while (!dead && hasTarget && action == "moving")
        {
            rigidbody2d.velocity = direction * speed;
            if (Time.time >= lastSkillTime + timeBetSkill)
            {
                lastSkillTime = Time.time;
                Skill();
            }
            yield return new WaitForSeconds(0.05f);
        }

        rigidbody2d.velocity = Vector2.zero;
    }

    // ��ų��� �� ����
    protected void Skill()
    {
        MonsterSpawner spawner = transform.GetComponentInParent<MonsterSpawner>();
        foreach (Monster monster in spawner.monsters) {
            if (monster.dead)
            {
                monster.Revive();
            }
        }

        animator.SetTrigger("Skill_Magic");
    }
}