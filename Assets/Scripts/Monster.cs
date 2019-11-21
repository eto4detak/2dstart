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
    protected float weaponAttackRadius = 0f;

    protected UnitCommand Command { get => command;
        set
        {
            command = value;

        }
    }
    public bool IsTired { get => isTired; private set => isTired = value; }



    protected override void Awake() {
        base.Awake();

    }
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
            ReceiveDamage(bullet.gameObject);
        }
    }
    private void OnCollisionStay2D(Collision2D other)
    {

        foreach (ContactPoint2D contact in other.contacts)
        {
           // Debug.Log(other.collider.tag + " hit " + contact.otherCollider.tag);
        }

        if (other.collider.tag != "Weapon") return;
        if (animator.GetBool("Attacking") && this.tag != other.gameObject.tag)
        {
            Unit atackingUnit = other.gameObject.GetComponentInParent<Unit>();
            if (atackingUnit != null)
            {
                ReceiveDamage(other.gameObject);
            }
        }
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

    private UnitCommand CheckRadiusAttack()
    {
        if (IsTired || health <= 0)
        {
            IsTired = true;
            return new TiredCommand(this);
        }
        Unit target = GetTarget();

        Debug.Log(name + "  GetTarget = " + target.name
            + " aggrred=" + aggression[Faction.Red] + " aggrGreen=" + aggression[Faction.Green]
            + " aggrPlayer=" + aggression[Faction.Player]);

        if (target == null) return null;
        float distance = Mathf.Abs(target.transform.position.x - transform.position.x);

        if (distance < attackRadius && !(Command is AttackCommand))
        {
            return new AttackCommand(this, target);
        }
        else if (distance > attackRadius && distance < attackRadius * 3 && !(Command is RunAttackCommand))
        {
            return new RunAttackCommand(this, target);
        }
        else if (distance > attackRadius * 3 && !(Command is MoveCommand))
        {
            return new MoveCommand(this, target);
        }
        return null;
    }


    private Unit GetTarget()
    {
        int maxAggr = 0;
        Faction Aggrfaction = 0;
        foreach (var commandAggr in aggression)
        {
            if (maxAggr <= commandAggr.Value)
            {
                Aggrfaction = commandAggr.Key;
                maxAggr = commandAggr.Value;
            }
        }

        Unit selectedEnemy = null;
        foreach (var enemy in GameManager.allUnit)
        {
            if (this == enemy) continue;
            if (tag == enemy.tag) continue;

            if (enemy.gameObject.tag == Aggrfaction.ToString())
            {
                if (selectedEnemy == null)
                {
                    selectedEnemy = enemy;
                    continue;
                }
                float distance = Mathf.Abs(enemy.transform.position.x - transform.position.x);
                float selectedDistance = Mathf.Abs(selectedEnemy.transform.position.x - transform.position.x);
                if (selectedDistance > distance)
                {
                    selectedEnemy = enemy;
                    continue;
                }
            }
        }

        return selectedEnemy;

    }



    public void ReplenishHealth(int countHealth)
    {
        StartCoroutine(AddHealth(countHealth));
    }

    protected void SetHealth(int newHealth)
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

    public override void ReceiveDamage(GameObject enemy)
    {
        base.ReceiveDamage(enemy);
    }


    /// <summary>
    /// Обновить статус действия от позиции юнита
    /// </summary>
    private void UpdatePositionStatus()
    {
        CheckIsGround();
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
}