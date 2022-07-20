using System.Collections;
using UnityEngine;

public class Monster : Entity
{
    private Entity targetEntity; // 추적할 대상

    public AudioClip deathSound; // 사망시 재생할 소리
    public AudioClip hitSound; // 피격시 재생할 소리

    protected Rigidbody2D rigidbody2d; // 리지드바디 컴포넌트
    protected Animator animator; // 애니메이터 컴포넌트
    protected AudioSource audioPlayer; // 오디오 소스 컴포넌트

    protected float damage ; // 공격력
    protected float speed; // 스피드
    protected float timeBetAttack = 0.5f; // 공격 간격
    protected float lastAttackTime; // 마지막 공격 시점

    //  추적 대상의 존재 여부
    private bool hasTarget
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

    private void Awake()
    {
        // 컴포넌트 초기화
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // 몬스터의 스텟 초기화
        Setup();
    }

    private void Update()
    {
        // 추적 대상의 존재 여부에 따라 다른 애니메이션을 재생
        animator.SetBool("HasTarget", hasTarget);

        // 주기적으로 추적할 대상의 위치를 찾아 경로를 갱신
        UpdatePath();
    }

    // 주기적으로 추적할 대상의 위치를 찾아 경로를 갱신
    protected void UpdatePath()
    {
        if (hasTarget)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetEntity.transform.position, speed * Time.deltaTime);
        }
        else
        {
            rigidbody2d.velocity = Vector2.zero;
            targetEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
        }
    }

    // 피격 처리
    public override void OnDamage(float damage)
    {
        if (!dead)
        {
            audioPlayer.PlayOneShot(hitSound);
        }

        base.OnDamage(damage);
    }

    // 사망 처리
    public override void Die()
    {
        base.Die();

        Collider[] colliders = GetComponents<Collider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }

        animator.SetTrigger("Die");
        audioPlayer.PlayOneShot(deathSound);
    }

    private void OnTriggerStay(Collider other)
    {
        // 트리거 충돌한 상대방 게임 오브젝트가 추적 대상이라면 공격 실행   
        if (!dead && Time.time >= lastAttackTime + timeBetAttack)
        {
            Entity attackTarget = other.GetComponent<Entity>();

            if (attackTarget != null && attackTarget == targetEntity)
            {
                lastAttackTime = Time.time;

                attackTarget.OnDamage(damage);
            }
        }
    }

    // 몬스터의 스텟 초기화
    public virtual void Setup() { }
}
