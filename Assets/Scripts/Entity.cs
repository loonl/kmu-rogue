using System;
using UnityEngine;

// 피조물 객체들의 뼈대 제공
public class Entity : MonoBehaviour {
    public float maxHealth = 100f; // 시작 체력
    public float health { get; protected set; } // 현재 체력
    public bool dead { get; protected set; } // 사망 상태
    public event Action onDeath; // 사망 시 발동 이벤트

    // 생명체가 활성화될때 상태 초기화
    protected virtual void OnEnable() {
        dead = false;
        health = maxHealth;
    }

    // 피격 처리
    public virtual void OnDamage(float damage) {
        health -= damage;

        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    // 사망 처리
    public virtual void Die() {
        if (onDeath != null)
        {
            onDeath();
        }

        dead = true;
    }
}