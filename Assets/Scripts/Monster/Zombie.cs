using UnityEngine;

public class Zombie : Monster
{
    // 몬스터 초기화
    protected override void Setup()
    {
        maxHealth = 100f;
        corpseHealth = 75f;
        damage = 20f;
        speed = 1f;

        base.Setup();
    }
}
