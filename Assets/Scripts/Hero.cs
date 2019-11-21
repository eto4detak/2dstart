using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Hero : Unit
{
    private Bullet bullet;
    
    protected override void Start()
    {
        base.Start();
        GameManager.allUnit.Add(this);
        speed = 10f;
        jumpForce = 70f;
        directionRight = 1;
        SizeX = 1.48f;
        SizeY = 1f;

        rigibody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        bullet = Resources.Load<Bullet>("Bullet");
        SetInfo("text text");

    }

    protected override void Update()
    {
        base.Update();

       // State = CharState.Idle;

        if (Input.GetButtonDown("Fire1")) Shoot();
        if (Input.GetButton("Horizontal")) Move(transform.right * Input.GetAxis("Horizontal"));
        if (isGrounded && Input.GetButtonDown("Jump") && !Input.GetKey(KeyCode.S)) Jump();


    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.GetComponent<Unit>();
        if (unit)
        {
            ReceiveDamage(collider.gameObject);
        }
    }


    protected override void Move(Vector3 direction)
    {
        //State = CharState.Run;
        //Vector3 direction = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
        Flip(direction);
    }

    //method base class 
    private void FixedUpdate()
    {
        CheckGround();
    }


    private void OnCollisionStay2D(Collision2D other)
    {

        if (Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.S))
        {
            JumpDown(other.collider);
            return;
        }
        if (other.collider.tag == "Weapon")
        {
            Unit atackingUnit = other.gameObject.GetComponentInParent<Unit>();
            if (atackingUnit != null)
            {
                ReceiveDamage(other.gameObject);
            }
        }

    }



    public void SetInfo(string text)
    {
        
    }

    private void Jump()
    {
        rigibody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }
    private void Shoot()
    {
        Vector3 position = transform.position;
        position.y += 1.0f;
        Bullet newBullet = Instantiate(bullet, position, bullet.transform.rotation) as Bullet;
        bullet.gameObject.layer = commandLayer;
        newBullet.Parent = gameObject;
        newBullet.Direction = newBullet.transform.right *(sprite.flipX ? -1.0f : 1.0f) ;
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        isGrounded = colliders.Length > 1;
    }

    //public override void ReceiveDamage(GameObject enemy)
    //{
    //    if (godMode) return;
    //    godMode = true;

    //    health--;
    //    Vector3 newv = transform.position - enemy.transform.position;
    //    rigibody.AddForce(newv, ForceMode2D.Impulse);
    //    Debug.Log(health);
    //    StartCoroutine("Blinking");
    //    Invoke("OffGodMode", damageTimer);
    //    Invoke("StopBlinking", damageTimer);
    //    GameManager.messageConvas.Message(this , "-1", 2f);
    //}


}
