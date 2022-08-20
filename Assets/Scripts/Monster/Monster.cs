using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Monster: MonoBehaviour
{
    protected Animator animator;
    protected AudioSource audioPlayer;
    protected Rigidbody2D rigidbody2d;
    protected CapsuleCollider2D capsuleCollider2d;
    protected CircleCollider2D circleCollider2d;

    public AudioClip deathSound; // 사망시 재생 소리
    public AudioClip hitSound; // 피격시 재생 소리

    public int idNumber; // 아이디 넘버
    protected float scale; // 크기
    protected float maxHealth; // 최대 체력
    protected float health; // 현재 체력
    protected float corpseHealth; // 시체 체력
    protected float damage; // 공격력
    protected float speed; // 이동 속도

    protected float attackCoolTime = 0.5f; // 공격 쿨타임
    protected float lastAttackTime; // 마지막 공격 시점
    public bool dead = false; // 사망 상태
    protected string action; // 현재 수행 중인 상태
    protected Player targetEntity; // 추적 대상
    protected Vector2 direction; // 경로 방향

    public event Action onEliminate; // 시체제거 시 발동 이벤트
    // 추적 대상의 존재 여부
    protected bool hasTarget
    {
        get
        {
            if (targetEntity != null && !targetEntity.dead)
            {
                return true;
            }

            return false;
        }
    }

    protected void Awake()
    {
        // 컴포넌트 초기화
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        audioPlayer = GetComponent<AudioSource>();

        
    }
    protected void Start()
    {
        
        SetUp(); // 몬스터 초기화
        Generate(); // 몬스터 생성
    }

    // csv파일을 이용하여 몬스터 초기화
    protected void SetUp()
    {
        string path = "Datas/Monster";
        List<Dictionary<string, object>> data = CSVReader.Read(path);

        scale = float.Parse(data[idNumber]["Scale"].ToString());
        maxHealth = float.Parse(data[idNumber]["MaxHealth"].ToString());
        corpseHealth = float.Parse(data[idNumber]["CorpseHealth"].ToString());
        damage = float.Parse(data[idNumber]["Damage"].ToString());
        speed = float.Parse(data[idNumber]["Speed"].ToString());
    }

    // 몬스터 활성화
    protected virtual void Generate()
    {
        health = maxHealth;
        dead = false;
        action = "moving";
        StartCoroutine(UpdatePath());
        StartCoroutine(Moving());
    }

    // 경로 갱신
    protected IEnumerator UpdatePath()
    {
        while (!dead)
        {
            if (hasTarget)
            {
                direction = (targetEntity.transform.position - transform.position).normalized;

                if (direction.x > 0)
                {

                    transform.localScale = new Vector3(-scale, scale, scale);
                }
                else
                {
                    transform.localScale = new Vector3(scale, scale, scale);
                }

                animator.SetBool("HasTarget", hasTarget);
            }
            else 
            {
                targetEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                animator.SetBool("HasTarget", hasTarget);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    // 이동 상태 수행
    protected virtual IEnumerator Moving()
    {
        while (!dead && action == "moving")
        {
            rigidbody2d.velocity = direction * speed;
            yield return new WaitForSeconds(0.05f);
        }
    }

    // 시체 상태 수행
    protected virtual IEnumerator Dying()
    {
        while (dead)
        {
            rigidbody2d.velocity = Vector2.zero;
            yield return new WaitForSeconds(0.05f);
        }
    }

    // 피격 처리
    public void OnDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            if (!dead)
            {
                Die();
            }
            else
            {
                Eliminate();
            }
        }

        //audioPlayer.PlayOneShot(hitSound);

        Debug.Log("Zombie Health: " + health);
    }

    // 사망 처리
    public virtual void Die()
    {
        dead = true;
        health = corpseHealth;
        StartCoroutine(Dying());

        animator.SetTrigger("Die");
        //audioPlayer.PlayOneShot(deathSound);
    }

    // 시체제거 처리
    protected void Eliminate()
    {
        if (onEliminate != null)
        {
            onEliminate();
        }
    }

    // 부활 처리
    protected void Revive()
    {
        maxHealth = (float)Math.Ceiling(maxHealth * 2 / 3);
        Generate();

        animator.SetTrigger("Revive");
    }

    protected void OnCollisionStay2D(Collision2D other)
    {
        if (dead)
        {
            return;
        }

        // 충돌한 게임 오브젝트가 추적 대상이라면 공격
        if (Time.time >= lastAttackTime + attackCoolTime)
        {
            Player attackTarget = other.gameObject.GetComponent<Player>();

            if (attackTarget != null && attackTarget == targetEntity)
            {
                lastAttackTime = Time.time;
                attackTarget.OnDamage(damage);
                animator.SetTrigger("Attack_Normal");
            }
        }
    }
}
