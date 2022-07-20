public class Zombie : Monster
{
    // 몬스터의 스텟 초기화
    public override void Setup()
    {
        maxHealth = 100f;
        health = 100f;
        damage = 20f;
        speed = 1f;
    }
}
