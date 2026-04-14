using System;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 5f;
    [SerializeField] private CapsuleCollider2D col;

    [SerializeField] private float jumpForce = 350;

    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject AttackArea;

    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isAttack = false;
   
    private float horizontal;

    private int coin = 0;

    private Vector3 savePoint;

    [Header("Stealth Skill")]
    [SerializeField] private float stealthDuration = 3f; 
    [SerializeField] private float stealthCooldown = 5f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private float stealthTimer;
   
    public bool isStealth { get; private set; } = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        coin = PlayerPrefs.GetInt("coin", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }

        isGrounded = CheckGrounded();

        horizontal = Input.GetAxisRaw("Horizontal"); //-1 0 1



        if (isGrounded && !isAttack)
        {

            //jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            //attack
            if (Input.GetKeyDown(KeyCode.C))
            {
                Attack();
            }
               
            //throw
            if (Input.GetKeyDown(KeyCode.V))
            {
                Throw();
            }

            //stealth
            if (Input.GetKeyDown(KeyCode.F) && Time.time >= stealthTimer)
            {
                ActivateStealth();
            }

        }
        if (!isAttack)
        {
            //check falling
            if (!isGrounded && _rb.linearVelocity.y < 0)
            {
                ChangeAnim("fall");
                isJumping = false;
            }
            //move
            if (Mathf.Abs(horizontal) > 0.1f)//k so sanh float vs 0
            {
                if (isGrounded && !isJumping)
                {
                    ChangeAnim("run");
                }
                _rb.linearVelocity = new Vector2(horizontal * speed * Time.deltaTime, _rb.linearVelocity.y);
                //transform.localScale = new Vector3(horizontal, 1, 1);
                transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));//quay nguoc lai khi di chuyen sang ben trai
            }
            //idle
            else if (isGrounded && !isJumping)
            {
                ChangeAnim("idle");
                _rb.linearVelocity = Vector2.zero;
            }
        }
        
    }

    private void ActivateStealth()
    {
        isStealth = true;
        stealthTimer = Time.time + stealthCooldown;

        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = 0.5f;
            spriteRenderer.color = c;
        }

        
        Invoke(nameof(DeactivateStealth), stealthDuration);
    }

    private void DeactivateStealth()
    {
        isStealth = false;

        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = 1f;
            spriteRenderer.color = c;
        }
    }

    public override void OnInit() //khoi tao nhan vat ve savepoint va reset trang thai
    {
        base.OnInit();
        
        isAttack = false;

        CancelInvoke(nameof(DeactivateStealth));
        DeactivateStealth();

        transform.position = savePoint;
        ChangeAnim("idle");
        DeactiveAttack();
        SavePoint();
        UIManager._instance.SetCoin(coin);
    }
    override public void OnDespawn()
    {
        base.OnDespawn();
        OnInit();
    }
    
    override protected void OnDead()
    {
        base.OnDead();
    }

    //private bool CheckGrounded()
    //{
    //    Debug.DrawLine(transform.position, transform.position + Vector3.down * 1f, Color.red);
    //    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f,groundLayer);
    //    return hit.collider != null;
    //}
    private bool CheckGrounded()
    {
        float distanceToGround = (col.size.y / 2) - col.offset.y + 0.01f;

        Debug.DrawLine(transform.position, transform.position + Vector3.down * distanceToGround, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distanceToGround, groundLayer);
        return hit.collider != null;
    }

    public void Attack()
    {
        ChangeAnim("attack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
        AvtiveAttack();
        Invoke(nameof(DeactiveAttack), 0.4f);
    }

    private void AvtiveAttack()
    {
        AttackArea.SetActive(true);
    }

    private void DeactiveAttack()
    {
        AttackArea.SetActive(false);
    }

    public void Throw()
    {
        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 1f);
        Instantiate(kunaiPrefab, throwPoint.position, throwPoint.rotation);
    }

    private void ResetAttack()
    {
        ChangeAnim("idle");
        isAttack = false;
    }

    public void Jump()
    {
        ChangeAnim("jump");
        isJumping = true;
        _rb.AddForce(jumpForce * Vector2.up);
    }
   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            coin++;
            PlayerPrefs.SetInt("coin", coin);
            UIManager._instance.SetCoin(coin);
            Destroy(collision.gameObject);
            Debug.Log("Coin: " + coin);
        }
        if(collision.gameObject.tag == "DeadZone")
        {
            OnHit(999f);
            ChangeAnim("die");
            Debug.Log("Game Over");
            _rb.linearVelocity = Vector2.zero;
            Invoke(nameof(OnInit), 1f);
        }
    }

    internal void SavePoint()
    {
        savePoint = transform.position;
    }

    public void setMove(float horizontal)
    {
        this.horizontal = horizontal;
    }

    public override void OnHit(float damage)
    {
        if (isStealth)
        {
            return;
        }

        base.OnHit(damage);
    }
}
