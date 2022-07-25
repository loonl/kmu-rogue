using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    Player playerUnit;
    ArcCollider2D arc;

    public List<GameObject> monsters;

    // Start is called before the first frame update
    void Start()
    {
        playerUnit = transform.GetComponentInParent<Player>();
        arc = GetComponent<ArcCollider2D>();

        arc.radius = playerUnit.attackRange;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster" && playerUnit.isAttacking && !monsters.Contains(collision.gameObject))
        {
            monsters.Add(collision.gameObject);
            // execute ondamage function when monster is in range
            collision.gameObject.SendMessage("OnDamage", playerUnit.damage);
        }
    }
}
