using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private const string LEFT = "left";
    private const string RIGHT = "right";
    [Header("Player Game Object")]
    [SerializeField] private Transform playerGameObject;

    [Header("Enemy Control")]
    [SerializeField] private float enemySpeed;
    [SerializeField] private float agroRange;
    [SerializeField] private float baseCastDist;
    [SerializeField] private Transform castPos;
    [SerializeField] private LayerMask wallLayerMask,groundLayerMask;


    private Rigidbody2D rb;
    private Vector2 baseScale;
    private string facingDirection;
    private bool isFacingLeft;

    //public bool IsFacingLeft { get => isFacingLeft; set => isFacingLeft = value; }

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
        baseScale = transform.localScale;
        facingDirection = RIGHT;
        isFacingLeft = false;
    }

    private void EnemyMovement()
    {
        float distToPlayer = Vector2.Distance(transform.position, playerGameObject.position);

        if (distToPlayer < agroRange)
        {
            EnemyChasePlayer();
        }
        else
        {
            EnemyPatrol();
        }
    }
    private void EnemyChasePlayer()
    {
        float chaseSpeed = enemySpeed * 3f;
        if (transform.position.x < playerGameObject.position.x)
        {
            rb.velocity = new Vector2(chaseSpeed, rb.velocity.y);
            ChangeFacingDirection(RIGHT);
            isFacingLeft = false;
        }
        else if (transform.position.x > playerGameObject.position.x)
        {
            rb.velocity = new Vector2(-chaseSpeed, rb.velocity.y);
            ChangeFacingDirection(LEFT);
            isFacingLeft = true;
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
        transform.localScale = newScale;
        facingDirection = newDirection;
    }

    private void EnemyPatrol()
    {
        float vX = enemySpeed;
        if (facingDirection == LEFT)
        {
            vX = -enemySpeed;
        }
        //else
        //{
        //    vX = enemySpeed;
        //}
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
        bool isNearEdge = false;
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
}
