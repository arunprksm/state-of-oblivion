using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private const string LEFT = "left";
    private const string RIGHT = "right";
    [SerializeField] [Tooltip("Define Player GameObject 'HERE'")] private Transform playerGameObject;
    [SerializeField] [Tooltip("Define Enemy GameObject 'HERE'")] private GameObject enemyGameObject;
    [SerializeField] [Tooltip("Define Enemy Health Bar GameObject 'HERE'")] private GameObject enemyHealthBar;

    [Header("Stats")]
    [Range(1f, 10f)] [Tooltip("Define the Moving speed of the Enemy")] [SerializeField] private float enemySpeed;
    [Range(1f, 10f)] [Tooltip("Define the Range between Enemy and Player")] [SerializeField] private float agroRange;
    [SerializeField] private float baseCastDist;
    [SerializeField] private Transform castPos;
    [SerializeField] private LayerMask wallLayerMask, groundLayerMask;

    [SerializeField] private int enemyAttackValue = 5;
    [Header("Health")]
    [SerializeField] private int enemyMaxHealth = 50;
    [SerializeField] private int enemyCurrentHealth;

    private Rigidbody2D rb;
    private Vector2 baseScale;
    private string facingDirection;
    public bool IsFacingLeft { get; set; }

    public static EnemyController instance;
    public static EnemyController Instance { get { return instance; } }

    //private void Awake()
    //{
    //    if (instance != null)
    //    {
    //        Destroy(gameObject);
    //    }
    //    else
    //    {
    //        instance = this;
    //    }
    //}
    private void Start()
    {
        InitializeComponents();
    }

    private void FixedUpdate()
    {
        EnemyMovement();
    }

    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        baseScale = enemyGameObject.transform.localScale;
        facingDirection = RIGHT;
        IsFacingLeft = false;
        enemyHealthBar.SetActive(false);
        enemyCurrentHealth = enemyMaxHealth;
        EnemyHealthController.Instance.SetMaxHealth(enemyMaxHealth);

    }

    private void EnemyMovement()
    {
        float distToPlayer = Vector2.Distance(transform.position, playerGameObject.position);

        if (distToPlayer < agroRange)
        {
            EnemyChasePlayer();
            enemyHealthBar.SetActive(true);
        }
        else
        {
            EnemyPatrol();
            enemyHealthBar.SetActive(false);

        }
    }
    private void EnemyChasePlayer()
    {
        float chaseSpeed = enemySpeed * 1.8f;
        if (transform.position.x < playerGameObject.position.x)
        {
            rb.velocity = new Vector2(chaseSpeed, rb.velocity.y);
            ChangeFacingDirection(RIGHT);
            IsFacingLeft = false;
        }
        else if (transform.position.x > playerGameObject.position.x)
        {
            rb.velocity = new Vector2(-chaseSpeed, rb.velocity.y);
            ChangeFacingDirection(LEFT);
            IsFacingLeft = true;
        }
    }

    private void ChangeFacingDirection(string newDirection)
    {
        Vector2 newScale = baseScale;
        if (newDirection == LEFT)
        {
            newScale.x = -baseScale.x;
        }
        else if (newDirection == RIGHT)
        {
            newScale.x = baseScale.x;
        }
        enemyGameObject.transform.localScale = newScale;
        facingDirection = newDirection;
    }

    private void EnemyPatrol()
    {
        float vX = enemySpeed;
        if (facingDirection == LEFT)
        {
            vX = -enemySpeed;
        }
        if (IsHittingWall() || IsNearEdge())
        {
            if (facingDirection == LEFT)
            {
                ChangeFacingDirection(RIGHT);
            }
            else if (facingDirection == RIGHT)
            {
                ChangeFacingDirection(LEFT);
            }
        }
        rb.velocity = new Vector2(vX, rb.velocity.y);
    }

    private bool IsHittingWall()
    {
        bool isHitting = false;
        float castDist = baseCastDist;

        if (facingDirection == LEFT)
        {
            castDist = -baseCastDist;
        }
        else
        {
            castDist = baseCastDist;
        }

        Vector2 targetPos = castPos.position;
        targetPos.x += castDist;
        Debug.DrawLine(castPos.position, targetPos, Color.green); //Gizmos.DrawLine(castPos.position, targetPos);
        if (Physics2D.Linecast(castPos.position, targetPos, wallLayerMask))
        {
            isHitting = true;
        }
        else
        {
            isHitting = false;
        }
        return isHitting;
    }

    private bool IsNearEdge()
    {
        bool isNearEdge;
        float castDist = baseCastDist;

        Vector2 targetPos = castPos.position;
        targetPos.y -= castDist;
        Debug.DrawLine(castPos.position, targetPos, Color.green); //Gizmos.DrawLine(castPos.position, targetPos);

        if (Physics2D.Linecast(castPos.position, targetPos, groundLayerMask))
        {
            isNearEdge = false;
        }
        else
        {
            isNearEdge = true;
        }
        return isNearEdge;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        PlayerMovementControl.instance = collision.gameObject.GetComponent<PlayerMovementControl>();
        if (PlayerMovementControl.Instance != null)
        {
            PlayerMovementControl.Instance.PlayerTakeDamage(enemyAttackValue);
        }
    }

    internal void EnemyTakeDamage(int damage)
    {
        enemyCurrentHealth -= damage;
        EnemyHealthController.Instance.SetHealth(enemyCurrentHealth);

        if (enemyCurrentHealth < 0)
        {
            enemyCurrentHealth = 0;
            EnemyDie();
        }
    }
    private void EnemyDie()
    {
        Debug.Log("Enemy Died");
    }
}
