using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Hero : MonoBehaviour
{
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource damageSound;
    [SerializeField] private AudioSource missAttack;
    [SerializeField] private AudioSource attackMob;
    [SerializeField] private AudioSource death;
    [SerializeField] private float speed = 3f;
    [SerializeField] private int lives;
    [SerializeField] private int health;
    [SerializeField] private Image[] hearts;
    [SerializeField] private int jumpForce = 500;
    private bool flipRight = true;
    private bool isGrounded;

    [SerializeField] private Sprite aliveHeart;
    [SerializeField] private Sprite deadHeart;

    public bool isAttacking = false;
    public bool isRecharged = true;

    public Transform attackpos;
    public float attackRange;
    public LayerMask enemy;

    public static Hero Instance { get; set; }

    private Animator anim;

    private void Awake()
    {
        lives = 5;
        health = lives;
        anim = GetComponent<Animator>();
        Instance = this;
        isRecharged = true;
    }

    void Update()
    {
        if (isGrounded && !isAttacking) State = States.idle;

        if (!isAttacking && Input.GetButton("Horizontal"))
            Run();
        if (!isAttacking && isGrounded && Input.GetButtonDown("Jump"))
            Jump();
        if (Input.GetButtonDown("Fire1"))
        {
            State = States.attack;
            Attack();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            State = States.attack1;
            Attack();
        }

        if (health > lives)
            health = lives;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
                hearts[i].sprite = aliveHeart;
            else
                hearts[i].sprite = deadHeart;

            if (i < lives)
                hearts[i].enabled = true;
            else
                hearts[i].enabled = false;
        }
    }

    void OnCollisionEnter2D()
    {
        isGrounded = true;
    }

    private void Run()
    {
        if (isGrounded) State = States.run;

        float move = Input.GetAxis("Horizontal");

        GetComponent<Rigidbody2D>().velocity = new Vector2(move * speed, GetComponent<Rigidbody2D>().velocity.y);

        if (move > 0 && !flipRight)
        {
            Flip();
        }
        else if (move < 0 && flipRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        flipRight = !flipRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void Jump()
    {
        isGrounded = false;
        if (!isGrounded) State = States.jump;
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce);
        jumpSound.Play();
    }

    public void GetDamage()
    {
        lives -= 1;
        damageSound.Play(); 
        if (health <= 1)
        {
            Die();
            foreach (var h in hearts)
                h.sprite = deadHeart;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void Attack()
    {
        if (isGrounded && isRecharged)
        {
            isAttacking = true;
            isRecharged = false;

            StartCoroutine(AttackAnimation());
            StartCoroutine(AttackCooldown());
        }
    }

    private void onAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackpos.position, attackRange, enemy);

        if (colliders.Length == 0)
            missAttack.Play();
        else
            attackMob.Play();

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<Entity>().GetDamage();
            StartCoroutine(EnemyOnAttack(colliders[i]));
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackpos.position, attackRange);
    }

    private IEnumerator EnemyOnAttack(Collider2D enemy)
    {
        SpriteRenderer enemyColor = enemy.GetComponentInChildren<SpriteRenderer>();
        enemyColor.color = new Color(1f, 0f, 0f);
        yield return new WaitForSeconds(0.2f);
        enemyColor.color = new Color(1f, 1f, 1f);
    }

    private IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(0.4f);
        isAttacking = false;
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        isRecharged = true;
    }

    public virtual void Die()
    {
        death.Play();
    }

    private States State
    {
        get { return (States)anim.GetInteger("state"); }
        set { anim.SetInteger("state", (int)value); }
    }

    public enum States
    {
        idle,
        run,
        jump,
        attack,
        attack1
    }
}
