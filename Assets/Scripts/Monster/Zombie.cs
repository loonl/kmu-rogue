public class Zombie : Monster
{
    // ������ ���� �ʱ�ȭ
    public override void Setup()
    {
        maxHealth = 100f;
        health = 100f;
        damage = 20f;
        speed = 1f;
    }
}
