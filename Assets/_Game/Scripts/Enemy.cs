using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : Character
{
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject AttackArea;
    private IState currentState;

    private bool isRight = true;

    private Character target;
    public Character Target => target;

    private void Update()
    {
        if (target != null && target is Player p && p.isStealth)
        {
            SetTarget(null);
        }

        if (currentState != null && !isDead)
        {
            currentState.OnExcute(this);
        }
    }

    override public void OnInit()
    {
        base.OnInit();
        ChangeState(new IdleState());
        DeactiveAttack();
    }

    override public void OnDespawn()
    {
        base.OnDespawn();
        Destroy(healthBar);
        Destroy(gameObject);
    }

    override protected void OnDead()
    {
        ChangeState(null);
        base.OnDead();
        ChangeAnim("die");
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }
        currentState = newState;
        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
            
    }

    internal void SetTarget(Character character)
    {
        if (this.target == character) return;

        if (character == this) return;
        this.target = character;

        if (Target == null)
        {
            ChangeState(new IdleState());
        }
        if (IsTargetInRange())
        {
            ChangeState(new AttackState());
        }
        else if (Target != null)
        {
            ChangeState(new PatrolState());
        }


    }

    private void AvtiveAttack()
    {
        AttackArea.SetActive(true);
    }

    private void DeactiveAttack()
    {
        AttackArea.SetActive(false);
    }

    public void Moving()
    {
        ChangeAnim("run");

        rb.linearVelocity = transform.right*moveSpeed;
    }
    public void StopMoving()
    {
        ChangeAnim("idle");
        rb.linearVelocity = Vector2.zero;
    }

    public virtual void Attack()
    {
        ChangeAnim("attack");
        AvtiveAttack();
        Invoke(nameof(DeactiveAttack), 0.4f);
        
    }

    public bool IsTargetInRange()
    {
        if (target != null&& Vector2.Distance(target.transform.position, transform.position) <= attackRange)
        {
            return true;
        }
            return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("EnemyWall"))
        {
            if (currentState is AttackState) return;
            ChangeDirection(!isRight);

            if (target != null)
            {
                SetTarget(null);
            }
        }
    }

    public void ChangeDirection(bool isRight)
    {
        this.isRight = isRight;
        transform.rotation = isRight ? Quaternion.Euler(Vector3.zero):Quaternion.Euler(Vector3.up * 180);
    }


}
