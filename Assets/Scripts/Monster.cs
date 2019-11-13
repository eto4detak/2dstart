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
    protected UnitCommand command;


    protected virtual void Awake() { }
    protected override void Start() {
        base.Start();
        command = new IdleCommand(this);

    }
    protected override void Update() {
        base.Update();

        UpdatePositionStatus();
        SavePosition();
        UpdateCommand();
        command.DoCommand();

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
            command = newCommand;
            return;
        }
    }

    private UnitCommand CheckRadiusAttack()
    {
        float distance = Mathf.Abs(GameManager.hero.transform.position.x - transform.position.x);

        if (distance < attackRadius && !(command is AttackCommand) )
        {
            return new AttackCommand(this);
        }
        else if(distance < attackRadius *3 && !(command is RunAttackCommand))
        {
            return new RunAttackCommand(this, GameManager.hero);
        }
        else if(!(command is MoveCommand))
        {
            return new MoveCommand(this, GameManager.hero);
        }
        return null;
    }





}