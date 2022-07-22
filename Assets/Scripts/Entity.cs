﻿using System;
using UnityEngine;

public class Entity : MonoBehaviour 
{
    protected float maxHealth; // 시작 체력
    protected float health; // 현재 체력
    public bool dead; // 사망 상태
    public event Action onDeath; // 사망 시 발동 이벤트

    // 생명체가 활성화될때 상태 초기화
    protected virtual void OnEnable() 
    {
        dead = false;
        health = maxHealth;
    }

    // 피격 처리
    public virtual void OnDamage(float damage) 
    {
        health -= damage;

        if (health <= 0)
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
}