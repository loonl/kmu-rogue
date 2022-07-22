using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    public Player PlayerUnit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster" && PlayerUnit.isAttacking)
        {
            // 몬스터에게 데미지
            collision.gameObject.SendMessage("OnDamage", PlayerUnit.damage);
        }
    }
}
