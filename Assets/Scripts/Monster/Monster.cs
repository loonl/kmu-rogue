using System;
using System.Collections;
using UnityEngine;

public class Monster : Entity
{
    public AudioClip deathSound; // 사망시 재생 소리
    public AudioClip hitSound; // 피격시 재생 소리

    protected Animator animator; // 애니메이터 컴포넌트
    protected AudioSource audioPlayer; // 오디오 소스 컴포넌트
    protected Rigidbody2D rigidbody2d; // 리지드바디 컴포넌트
    protected CapsuleCollider2D capsuleCollider2d; // 캡슐콜라이더 컴포넌트
    protected CircleCollider2D circleCollider2d; //서클콜라이더 컴포넌트

    protected float corpseHealth; // 시체 체력
    protected float damage ; // 공격력
    protected float speed; // 이동 속도
    protected float attackCoolTime = 1f; // 공격 쿨타임
    protected float lastAttackTime; // 마지막 공격 시점

    protected Entity targetEntity; // 추적 대상
    protected string action = "moving"; // 현재 행동

    public event Action onRevive; // 부활 시 발동 이벤트
    public event Action onEliminate; // 시체제거 시 발동 이벤트

    protected Vector2 direction;

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

        // 몬스터의 스텟 초기화
        Setup();
    }

    protected override void OnEnable()
    {
        
    }

    protected void Start()
    {

        Setup();
    }

    protected virtual void Update()
    {
        // 현재 행동 수행
        if (dead)  // **플레이어 사망여부 추가
        {
            rigidbody2d.velocity = Vector2.zero;
        }
        else if (action == "moving")
        {
            Moving();
        }

        // 추적 대상의 존재 여부에 따라 다른 애니메이션을 재생
        animator.SetBool("HasTarget", hasTarget);
    }

    // 경로 갱신
    protected IEnumerator UpdatePath()
    {
        while(!dead)
        {
            if (hasTarget)
            {
                if (targetEntity.transform.position.x - transform.position.x > 0)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                else
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }

                direction = (targetEntity.transform.position - transform.position).normalized;
            }
            else 
            {
                targetEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    // 이동 행동
    protected virtual void Moving()
    {
        rigidbody2d.velocity = direction * speed;
    }

    // 피격 처리
    public override void OnDamage(float damage)
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

        Debug.Log("Zombie OnDamage: " + health);
    }

    // 사망 처리
    public override void Die()
    {
        base.Die();
        health = corpseHealth;

        animator.SetTrigger("Die");
        //audioPlayer.PlayOneShot(deathSound);

        Debug.Log("Zombie Dead");
    }

    // 시체제거 처리
    protected void Eliminate()
    {
        if (onEliminate != null)
        {
            onEliminate();
        }

        Debug.Log("Zombie Eliminate");
    }

    // 부활 처리
    protected void Revive()
    {
        if (onRevive != null)
        {
            onRevive();
        }

        Setup();

        animator.SetTrigger("Revive");

        Debug.Log("Zombie Revive");
    }

    protected void OnCollisionStay2D(Collision2D other)
    {
        if (dead)
        {
            return;
        }

        // 충돌한 상대방 게임 오브젝트가 추적 대상이라면 공격 실행
        if (Time.time >= lastAttackTime + attackCoolTime)
        {
            Entity attackTarget = other.gameObject.GetComponent<Entity>();

            if (attackTarget != null && attackTarget == targetEntity)
            {
                lastAttackTime = Time.time;

                attackTarget.OnDamage(damage);
            }
        }
    }

    // 몬스터의 스텟 초기화
    protected virtual void Setup() {
        health = maxHealth;
        dead = false;
        action = "moving";
        StartCoroutine(UpdatePath());
    }
}
