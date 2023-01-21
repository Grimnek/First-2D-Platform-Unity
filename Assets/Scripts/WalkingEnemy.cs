using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : Entity
{
    private Vector3 dir;
    private Animator anim;
    private bool flipRight = true;

    private void Awake()
    {
        flipRight = false;
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        lives = 2;
        State = States.idle;
        dir = transform.right;
    }

    private void Update()
    {
        if (lives < 1)
        {
            State = States.death;
        }
        else
            Move();
    }

    private void Move()
    {
        State = States.run;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.1f + transform.right * dir.x * 0.7f, 0.1f);

        if (colliders.Length > 0) 
        {
            dir *= -1f;
            Flip();
        } 
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, Time.deltaTime);
    }

    private void Flip()
    {
        flipRight = !flipRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (lives > 0 && collision.gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.GetDamage();
        }
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
        death
    }
}
