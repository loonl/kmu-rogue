using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillCollider : MonoBehaviour
{
    Player playerUnit;
    public CircleCollider2D circle;
    public List<GameObject> monsters;

    // Start is called before the first frame update
    void Start()
    {
        playerUnit = GameObject.Find("Player").GetComponent<Player>();
        circle = GetComponent<CircleCollider2D>();
        circle.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetAttackRange(float value)
    {
        circle.radius = value;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster" && playerUnit.remainCool <= 0.0f && !monsters.Contains(collision.gameObject))
        {
            monsters.Add(collision.gameObject);
            // execute ondamage function when monster is in range
            collision.gameObject.SendMessage("OnDamage", playerUnit.skillDamage);
        }
    }
}
