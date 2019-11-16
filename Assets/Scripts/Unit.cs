using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    protected int health = 3;
    public int maxHealth = 3;
    internal float speed = 3.0f;
    protected float agressionRadius = 1.0f;
    protected float attackRadius = 1.0f;
    protected bool isAttacking = false;
    internal Rigidbody2D rigibody;
    protected SpriteRenderer sprite;
    internal float jumpForce = 10f;
    internal bool isGrounded = false;
    protected float directionRight = 1f;
    /// <summary>
    /// for gameobject
    /// </summary>
    protected int layer;
    /// <summary>
    /// for gameobject
    /// </summary>
    protected Dictionary<Transform, int> childLayers = new Dictionary<Transform, int>();

    public float damageTimer = 2f;
    internal bool godMode = false;
    protected Unit target;

    internal float SizeX = 1f;
    internal float SizeY = 1f;
    internal Animator animator;
    internal MessageConvas messageManager;

    //time
    protected float replenishmentTime = 1f;

    protected virtual void Start()
    {
        rigibody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        messageManager = GetComponentInChildren<MessageConvas>();
        SaveStartLayers();
        if (rigibody != null)
        {
            jumpForce = rigibody.mass * 3 ;
        }
    }


    protected virtual void Update()
    {
        UpdateDirection();
    }

    private void UpdateDirection()
    {
        directionRight = Input.GetAxis("Horizontal");
    }


    private void SaveStartLayers()
    {
        layer = gameObject.layer;
        foreach (var child in GetComponentsInChildren<Transform>())
        {
            childLayers.Add(child, child.gameObject.layer);
        }
        
    }


    protected virtual void Die()
    {
        Destroy(gameObject);
    }
    public virtual void Attack(Unit target)
    {

    }

    public void MoveToPoint(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        if (direction.x != 0)
        {
            Move(new Vector3(direction.x, 0, 0));
            return;
        }

        if (direction.y != 0)
        {
            Jump(new Vector3(0, direction.y, 0));
            return;
        }
    }
    protected virtual void Move(Vector3 direction)
    {
       // State = CharState.Run;
        //animator.SetTrigger("speed");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
        Flip(direction);
    }

    public virtual void Flip(Vector3 direction)
    {
        sprite.flipX = direction.x < 0.0f;
    }

    public virtual void Jump(Vector3 direction)
    {
        if (!isGrounded) return;

        rigibody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    public virtual void JumpInDirection(Vector3 force )
    {
        if (!isGrounded) return;

        Debug.Log("force");

        rigibody.AddForce(force, ForceMode2D.Impulse);
    }
    public virtual void ReceiveDamage()
    {
        if (godMode) return;
        godMode = true;
        health--;
        StartCoroutine("Blinking");
        OffGodMode(damageTimer);
        StopBlinking(damageTimer);

        GameManager.messageConvas.Message(this, "-1");
        if (messageManager != null)
        {
            messageManager.Message(this, "-1 = " + health, 0.6f);
        }
    }

    public void OffGodMode(float damageTimer = 1f)
    {
        Invoke("OffGodMode", damageTimer);
    }
    private void OffGodMode()
    {
        godMode = false;
    }
    public void StopBlinking(float damageTimer = 1f)
    {
        Invoke("StopBlinking", damageTimer);
    }
    private void StopBlinking()
    {
        StopCoroutine("Blinking");
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
    }

    IEnumerator Blinking()
    {
        float time = 0.1f; //Время на смену каждого цвета в секундах
        while (true)
        {
            float i = 0.0f;
            while (i < 1.0f)
            {
                if (i < 1)
                {
                    i += Time.deltaTime / time;
                }
                else
                {
                    i -= Time.deltaTime / time;
                }

                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, i);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }



    protected virtual void ReplenishHealth()
    {
        health++;
        GameManager.messageConvas.Message(this, "+1", 0.6f);
    }


    protected void SetDefaultLayers()
    {
        gameObject.layer = layer;
        foreach (var child in childLayers)
        {
            child.Key.gameObject.layer = child.Value;
        }
    }

}



public enum CharState
{
    Idle,
    Run
}