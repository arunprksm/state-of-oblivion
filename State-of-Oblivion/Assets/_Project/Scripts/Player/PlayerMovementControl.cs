using System.Collections;
using System.IO.IsolatedStorage;
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

    private bool jump, attack, isGrounded;
    public bool isdead;

    [SerializeField] AudioClip[] audioClip;

    private Rigidbody2D rb2D;
    private Animator animator;
    private float move;
    [SerializeField] private SceneController sceneController;
    //SingleTon Class
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
        gameObject.SetActive(true);
        InitializeComponenet();
    }

    private void InitializeComponenet()
    {
        isdead = false;
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCurrentHealth = playerMaxHealth;
        PlayerHealthController.Instance.SetMaxHealth(playerMaxHealth);
        //SoundManager.Instance.PlayMusic(Sounds.GameMusic_scene1);
        SoundManager.Instance.PlayMusic(sceneController.CheckGameScene());
        //SceneController.Instance.CheckGameScene();
    }
    private void Update()
    {

        if (SceneController.IsGamePaused || isdead) //guard class
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
        if (isdead) //guard class
        {
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
            return;
        }
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
        if (isGrounded && jump)
        {
            SoundManager.Instance.PlaySFX(Sounds.playerJump);
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpValue);
            animator.SetTrigger("Jump");
        }
    }
    private void PlayerFall()
    {
        if (!isGrounded)
        {
            animator.SetBool("Fall", true);
            return;
        }
        animator.SetBool("Fall", false);
    }

    private void PlayerAttack()
    {
        Collider2D[] hitEnemies; // = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, playerAttackableLayers);

        if (isGrounded && attack)
        {
            SoundManager.Instance.PlaySFX(Sounds.PlayerAttack);
            animator.SetTrigger("Attack");
            animator.SetBool("JumpAttack", false);
            hitEnemies = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, playerAttackableLayers);
        }
        else if (isGrounded && attack)
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
            enemy.GetComponent<EnemyController>().EnemyTakeDamage(playerAttackValue);
        }
    }
    public void PlayerWalkSound()
    {
        SoundManager.Instance.PlaySFX(Sounds.PlayerMove);
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
        SoundManager.Instance.PauseMusic(sceneController.CheckGameScene());
        SoundManager.Instance.PlaySFX(Sounds.PlayerDeath);
        //SoundManager.Instance.PauseMusic(Sounds.GameMusic_scene1);
        animator.SetBool("Die", true);
        isdead = true;

        yield return new WaitForSecondsRealtime(2);
        //Debug.Log("Player Died");
        gameObject.SetActive(false);
        sceneController.Pause();

    }
    public void PlayerWin()
    {
        //Debug.Log("Player Win");
    }
}