using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerMovementControl : MonoBehaviour
{
    [Header("Player Control")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpValue;

    [Header("Player Ground Check")]
    [SerializeField] private Transform feetPosition;
    [SerializeField] private float feetCheckRadius;

    [SerializeField] private LayerMask whatGroundLayer;

    [Header("Player Attack Check")]
    [SerializeField] private Transform attackPosition;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask playerAttackableLayers;
    
    [SerializeField] private int playerMaxHealth = 100;
    [SerializeField] internal int playerCurrentHealth;

    [SerializeField] private int playerAttackValue = 20;

    private bool jump, attack;
    private bool isGrounded;

    private Rigidbody2D rb2D;
    private Animator animator;
    private float move;

    internal static PlayerMovementControl instance;
    internal static PlayerMovementControl Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        InitializeComponenet();
    }

    private void InitializeComponenet()
    {
        
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCurrentHealth = playerMaxHealth;
        PlayerHealthController.Instance.SetMaxHealth(playerMaxHealth);
    }
    private void Update()
    {
        if (SceneController.IsGamePaused) //guard class
        {
            return;
        }

        PlayerInput();
        PlayerMovement();
        PlayerFlip();
        PlayerJump();
        PlayerFall();
        PlayerAttack();
    }
    private void PlayerInput()
    {
        move = Input.GetAxisRaw("Horizontal");
        jump = Input.GetKeyDown(KeyCode.Space);
        attack = Input.GetKeyDown(KeyCode.Mouse0);
    }

    private void PlayerMovement()
    {
        //bool isGrounded = Collision.
        rb2D.velocity = new Vector2(move * moveSpeed, rb2D.velocity.y);
        animator.SetFloat("Blend", Mathf.Abs(move));
    }

    private void PlayerFlip()
    {
        Vector2 playerFlip = transform.localScale;

        if (move < 0)
        {
            playerFlip.x = -1f * Mathf.Abs(playerFlip.x);
        }
        else if (move > 0)
        {
            playerFlip.x = Mathf.Abs(playerFlip.x);
        }
        transform.localScale = playerFlip;
    }
    private void PlayerJump()
    {
        isGrounded = Physics2D.OverlapCircle(feetPosition.position, feetCheckRadius, whatGroundLayer);
        //if (!isGrounded)
        //{
        //    return;
        //}
        if (isGrounded == true && jump)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpValue);
            animator.SetTrigger("Jump");
        }
    }
    private void PlayerFall()
    {
        if (!isGrounded)
        {
            animator.SetBool("Fall", true);
        }
        else
        {
            animator.SetBool("Fall", false);
        }
    }

    private void PlayerAttack()
    {
        Collider2D[] hitEnemies; // = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, playerAttackableLayers);

        if (isGrounded && attack)
        {
            animator.SetTrigger("Attack");
            animator.SetBool("JumpAttack", false);
            hitEnemies = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, playerAttackableLayers);
        }
        else if (isGrounded == false && attack)
        {
            animator.SetBool("JumpAttack", true);
            hitEnemies = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, playerAttackableLayers);
        }
        else
        {
            animator.SetBool("JumpAttack", false);
            hitEnemies = Physics2D.OverlapCircleAll(attackPosition.position, 0, 0);
        }

        foreach (Collider2D enemy in hitEnemies)
        {
            //Debug.Log("we Hit" + enemy.name);
            enemy.GetComponent<EnemyController>().EnemyTakeDamage(playerAttackValue);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPosition == null)
        {
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DieLimit"))
        {
            StartCoroutine(PlayerDie());
        }
    }

    internal void PlayerTakeDamage(int damage)
    {
        playerCurrentHealth -= damage;
        PlayerHealthController.Instance.SetHealth(playerCurrentHealth);
        if (playerCurrentHealth < 0)
        {
            playerCurrentHealth = 0;
            StartCoroutine(PlayerDie());
        }
    }
    private IEnumerator PlayerDie()
    {
        yield return new WaitForSecondsRealtime(5);
        Debug.Log("Player Died");
    }
    internal void PlayerWin()
    {
        Debug.Log("Player Win");
    }
}
