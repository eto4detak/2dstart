using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Hero : Unit
{
    public float speed = 10.0f;
    //new private Rigidbody2D rigidbody;
    private Rigidbody2D mainHero;
    private Animator animator;
    private SpriteRenderer sprite;
    private float jumpForce = 20.0f;
    private int lives = 10;

    private bool isGrounded = false;
    private Bullet bullet;
    private CharState State
    {
        get { return (CharState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }

    void Start()
    {
        mainHero = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        bullet = Resources.Load<Bullet>("Bullet");
    }

    void Update()
    {
        State = CharState.Idle;

        if (Input.GetButtonDown("Fire1")) Shoot();
        if (Input.GetButton("Horizontal")) Run();
        if (isGrounded && Input.GetButtonDown("Jump")) Jump();

    }

    //method base class 
    private void FixedUpdate()
    {
        CheckGround();
    }


    private void Run()
    {
        State = CharState.Run;
        Vector3 direction = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
        sprite.flipX = direction.x > 0.0f;
        //float moveX = Input.GetAxis("Horizontal");
        //mainHero.MovePosition(mainHero.position + Vector2.right * moveX * speed * Time.deltaTime);
    }
    private void Jump()
    {
        mainHero.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }
    private void Shoot()
    {
        Vector3 position = transform.position;
        position.y += 1.0f;
        Bullet newBullet = Instantiate(bullet, position, bullet.transform.rotation) as Bullet;

        newBullet.Parent = gameObject;
        newBullet.Direction = newBullet.transform.right * (sprite.flipX ? 1.0f : -1.0f);
    }
    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        isGrounded = colliders.Length > 1;
    }


    public override void ReceiveDamage()
    {
        lives--;
        mainHero.velocity = Vector3.zero;
        mainHero.AddForce(transform.up * 15.0f, ForceMode2D.Impulse);
        Debug.Log(lives);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.GetComponent<Unit>();

        if (unit)
        {
            ReceiveDamage();
        }
    }



}


public enum CharState
{
    Idle,
    Run
}