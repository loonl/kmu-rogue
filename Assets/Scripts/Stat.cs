using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat
{
    public int hp { get; set; }
    public int maxHp { get; set; }

    public int damage { get; set; }
    public float range { get; set; }
    public int skillDamage { get; set; }
    public float coolTime { get; set; }

    public float speed { get; set; }
    public Stat(bool empty)
    {
        if (!empty)
        {
            maxHp = 100;
            hp = 100;
            damage = 0;
            range = 0;
            skillDamage = 0;
            coolTime = 0f;
            speed = 2f;
        }
        else
        {
            maxHp = 0;
            hp = 0;
            damage = 0;
            range = 0;
            skillDamage = 0;
            coolTime = 0f;
            speed = 0f;
        }
    }

    public Stat(int _hp, int _damage, int _range, int _skillDamage, int _coolTime, int _speed)
    {
        maxHp = _hp;
        hp = _hp;
        damage = _damage;
        range = _range;
        skillDamage = _skillDamage;
        coolTime = _coolTime;
        speed = _speed;
    }

    public void SyncStat(List<Stat> stats)
    {
        foreach (var stat in stats)
        {
            maxHp += stat.maxHp;
            hp += stat.hp;
            damage += stat.damage;
            range += stat.range;
            skillDamage += stat.skillDamage;
            coolTime += stat.coolTime;
            speed += stat.speed;
        }
    }

    public void SubStat(List<Stat> stats)
    {
        foreach (var stat in stats) 
        {
            maxHp -= stat.maxHp;
            hp -= stat.hp;
            damage -= stat.damage;
            range -= stat.range;
            skillDamage -= stat.skillDamage;
            coolTime -= stat.coolTime;
            speed -= stat.speed;
        }
    }

    public void SyncHP(int _hp, int _maxHp)
    {
        hp = _hp;
        maxHp = _maxHp;
    }

    public void Damaged(int amount)
    {
        hp = Mathf.Clamp(hp - amount, 0, maxHp);
    }

    public void Recover(int amount)
    {
        hp = Mathf.Clamp(hp + amount, 0, maxHp);
    }


}
