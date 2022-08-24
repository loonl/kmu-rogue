using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    Player playerUnit;
    ArcCollider2D arc;
    public PolygonCollider2D poly;
    public List<GameObject> monsters;

    // Start is called before the first frame update
    void Start()
    {
        playerUnit = transform.GetComponentInParent<Player>();
        arc = GetComponent<ArcCollider2D>();
        poly = GetComponent<PolygonCollider2D>();

        poly.enabled = false;
    }

    public void SetAttackRange(float value)
    {
        arc.radius = value;
    } 

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster" && playerUnit.isAttacking && !monsters.Contains(collision.gameObject))
        {
            monsters.Add(collision.gameObject);
            // execute ondamage function when monster is in range
            collision.gameObject.SendMessage("OnDamage", playerUnit.stat.damage);
        }

        if (collision.gameObject.tag == "MapObject")
        {
            collision.gameObject.SendMessage("OnDamage");
        }
    }
}
