using System;
using UnityEngine;

// 피조물 객체들의 뼈대 제공
public class Entity : MonoBehaviour
{
    protected float maxHealth = 100f; // 최대 체력
    protected float health; // 현재 체력
    public bool dead = false; // 사망 상태
    public event Action onDeath; // 사망 시 발동 이벤트

    protected virtual void OnEnable()
    {
        // 피조물이 활성화될 때 상태 초기화
        dead = false;
        health = maxHealth;
    }

    // 피격 처리
    public virtual void OnDamage(float damage)
    {
        health -= damage;

        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    // 사망 처리
    public virtual void Die()
    {
        if (onDeath != null)
        {
            onDeath();
        }

        dead = true;
    }

    public float GetHP() { return health; }
    public float GetMaxHP() { return maxHealth; }
}