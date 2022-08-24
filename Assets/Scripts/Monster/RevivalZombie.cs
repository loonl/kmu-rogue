using System.Collections;
using UnityEngine;

public class RevivalZombie : Monster
{
    protected float timeBetRevive = 5f; // 부활 대기시간
    float startReviveTime; // 부활 시작시간

    // 시체 상태 수행 후 부활 처리
    protected override IEnumerator Dying()
    {
        startReviveTime = Time.time;

        while (dead)
        {
            rigidbody2d.velocity = Vector2.zero;
            if (Time.time >= startReviveTime + timeBetRevive)
            {
                Revive();
            }

            yield return new WaitForSeconds(0.05f);
        }
    }
}
