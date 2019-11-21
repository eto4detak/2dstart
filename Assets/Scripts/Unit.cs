using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    /// <summary>
    /// command(tag), value aggression
    /// </summary>
    protected Dictionary<Faction, int> aggression = new Dictionary<Faction, int>();
    internal Animator animator;
    protected float aggressionRadius = 1.0f;
    protected float attackRadius = 1.0f;
    /// <summary>
    /// for retun dafault
    /// </summary>
    protected Dictionary<Transform, int> childLayers = new Dictionary<Transform, int>();
    public float damageTimer = 2f;
    protected float directionRight = 1f;
    protected Faction faction;

    internal float jumpForce = 10f;
    internal Collider2D jumpPlatform;
    internal bool isGrounded = false;
    protected bool isAttacking = false;
    internal bool godMode = false;
    protected int health = 3;
    public int maxHealth = 3;
    internal float speed = 3.0f;
    protected SpriteRenderer sprite;
    internal Rigidbody2D rigibody;
    /// <summary>
    /// for retun dafault 
    /// </summary>
    protected int commandLayer;

    protected Unit target;
    internal float SizeX = 1f;
    internal float SizeY = 1f;
    internal MessageConvas messageManager;
    private static int maxAgression = 100;

    //time
    protected float replenishmentTime = 1f;


    protected virtual void Awake()
    {

    }


    protected virtual void Start()
    {
        Faction startFaction = (Faction)System.Enum.Parse(typeof(Faction), tag);

        if (faction.GetType() != null)
        {
            faction = startFaction;
        }
        GameManager.allUnit.Add(this);
        aggression.Add(Faction.Blue, 20);
        aggression.Add(Faction.Green, 20);
        aggression.Add(Faction.Grey, 20);
        aggression.Add(Faction.Orange, 20);
        aggression.Add(Faction.Player, 21);
        aggression.Add(Faction.Red, 20);
        aggression.Add(Faction.White, 20);
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
    void OnDestroy()
    {
        GameManager.allUnit.Remove(this);
    }

    protected void AddAggression(Faction enemy, int deltaAggresive)
    {
        aggression[enemy] += deltaAggresive;
        int maxAggr = 0;
        Faction maxAggressionCommmand = 0;
        foreach (var commandAggression in aggression)
        {
            if (commandAggression.Value >= maxAggr)
            {
                maxAggressionCommmand = commandAggression.Key;
                maxAggr = commandAggression.Value;
            }
        }
        if (maxAggr > maxAgression)
        {
            int deltaGeneralAgression = maxAggr - maxAgression;

            for (int i = 0; i < System.Enum.GetNames(typeof(Faction)).Length; i++)
            {
                aggression[(Faction)i] =
                    Mathf.Max(aggression[(Faction)i] - deltaAggresive, 0);
            }
        }
        if (aggression[enemy] > maxAgression)
        {
            aggression[enemy] = maxAgression;
        }
        foreach (var aggr in aggression)
        {
            // Debug.Log("aggr " + aggr.Key + " = " + aggr.Value);

        }


    }


    public virtual void Attack(Unit target)
    {

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
    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    public virtual void Flip(Vector3 direction)
    {
        sprite.flipX = direction.x < 0.0f;
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
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
        Flip(direction);
    }


    public virtual void Jump(Vector3 direction)
    {
        if (!isGrounded) return;
        
        rigibody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    public virtual void JumpInDirection(Vector3 force )
    {
        if (!isGrounded) return;
        rigibody.AddForce(force, ForceMode2D.Impulse);
    }

    protected void JumpDown(Collider2D platform)
    {
        GetComponent<Collider2D>().isTrigger = true;
        Invoke("SetDefaulJumpCollision", 0.3f);
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


    protected virtual void ReplenishHealth()
    {
        health++;
        GameManager.messageConvas.Message(this, "+1", 0.6f);
    }


    protected void SetDefaultLayers()
    {
        gameObject.layer = commandLayer;
        foreach (var child in childLayers)
        {
            child.Key.gameObject.layer = child.Value;
        }
    }



    private void SaveStartLayers()
    {
        commandLayer = gameObject.layer;
        foreach (var child in GetComponentsInChildren<Transform>())
        {
            childLayers.Add(child, child.gameObject.layer);
        }

    }

    protected void SetDefaulJumpCollision()
    {
        GetComponent<Collider2D>().isTrigger = false;
        
    }


    public virtual void ReceiveDamage(GameObject enemy)
    {
        if (godMode) return;
        godMode = true;
        health--;

        Vector2 damageForceVector = (transform.position - enemy.transform.position).normalized * 200;
        rigibody.AddForce(damageForceVector, ForceMode2D.Impulse);
        Unit enemyUnit = enemy.GetComponent<Unit>();
        if (enemyUnit)
        {
            AddAggression(enemyUnit.faction, 1);
        }
        StartCoroutine("Blinking");
        OffGodMode(damageTimer);
        StopBlinking(damageTimer);

        GameManager.messageConvas.Message(this, "-1");
        if (messageManager != null)
        {
            messageManager.Message(this, "-1 = " + health, 0.6f);
        }
    }

    private void UpdateDirection()
    {
        directionRight = Input.GetAxis("Horizontal");
    }



}



public enum CharState
{
    Idle,
    Run
}

public enum Faction
{
    Player = 0,
    Green,
    Grey,
    Blue,
    Red,
    White,
    Orange,

}