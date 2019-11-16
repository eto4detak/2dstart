using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Monster : Unit
{
    protected Vector3 direction;
    internal List<Vector3> oldPosiztions =new List<Vector3>();
    internal float oldPositionsTime = 0f;
    private float maxOldPositionsTime = 2f;
    private UnitCommand command;
    private bool isTired = false;

    protected UnitCommand Command { get => command;
        set
        {
            command = value;

        }
    }
    public bool IsTired { get => isTired; private set => isTired = value; }

    protected virtual void Awake() { }
    protected override void Start() {
        base.Start();
        Command = new IdleCommand(this);

    }
    protected override void Update() {
        base.Update();

        UpdatePositionStatus();
        SavePosition();
        
        if (!IsTired)
        {
           UpdateCommand();
           Command.DoCommand();
        }

    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        Bullet bullet = collider.GetComponent<Bullet>();

        if (bullet)
        {
            ReceiveDamage();
        }
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.tag != "Weapon") return;
        if (animator.GetBool("Attacking") && this.tag != other.gameObject.tag)
        {
            Unit atackingUnit = other.gameObject.GetComponentInParent<Unit>();
            if (atackingUnit != null)
            {
                ReceiveDamage();
            }
        }
    }


    protected void SetHealth(int newHealth )
    {
        health = newHealth;
        command = new TiredCommand(this);
        IsTired = true;
    }
    private void SavePosition()
    {
        if (oldPositionsTime > maxOldPositionsTime)
        {
            oldPosiztions.Clear();
            oldPositionsTime = 0;
        }
        oldPosiztions.Add(transform.position);
        oldPositionsTime += Time.deltaTime;
    }

    public override void ReceiveDamage()
    {
        base.ReceiveDamage();
    }


    /// <summary>
    /// Обновить статус действия от позиции юнита
    /// </summary>
    private void UpdatePositionStatus()
    {
        CheckIsGround();
    }

    private void CheckIsGround()
    {
        Collider2D[] collidersUndefoot = Physics2D.OverlapCircleAll(transform.position + transform.right * direction.x * SizeX / 2, 0.2f);
        foreach (var undefoot in collidersUndefoot)
        {
            if (undefoot.tag == "Ground")
            {
                isGrounded = true;
                return;
            }
        }
        isGrounded = false;
    }
    private void UpdateCommand()
    {
        UnitCommand newCommand = CheckRadiusAttack();
        if (newCommand != null)
        {
            Debug.Log("new command= " + newCommand);
           // GameManager.messageConvas.Message(this, "new command= " + newCommand);
            Command = newCommand;
            return;
        }
    }

    private UnitCommand CheckRadiusAttack()
    {
        if (IsTired || health <= 0)
        {
            IsTired = true;
           return new TiredCommand(this);
        }
        float distance = Mathf.Abs(GameManager.hero.transform.position.x - transform.position.x);

        if (distance < attackRadius && !(Command is AttackCommand) )
        {
            return new AttackCommand(this);
        }
        else if(distance < attackRadius *3 && !(Command is RunAttackCommand))
        {
            return new RunAttackCommand(this, GameManager.hero);
        }
        else if(!(Command is MoveCommand))
        {
            return new MoveCommand(this, GameManager.hero);
        }
        return null;
    }

    public void ReplenishHealth(int countHealth)
    {
        StartCoroutine(AddHealth(countHealth));
    }

    IEnumerator AddHealth(int countHealth)
    {
        for (int i = 0; i < countHealth; i++)
        {
            ReplenishHealth();
            yield return new WaitForSeconds(replenishmentTime);
        }
        IsTired = false;
        animator.SetTrigger("startactive");
        SetDefaultLayers();
        StopCoroutine("AddHealth");
    }

}