﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCommand
{
    protected Monster monster;
    public UnitCommand(Monster paramUnit)
    {
        monster = paramUnit;
    }
    public UnitCommand()
    {
    }
    public virtual void DoCommand()
    {

    }

}


public class IdleCommand : UnitCommand
{
    public IdleCommand(Monster paramUnit)
    {
        monster = paramUnit;

    }
    public override void DoCommand()
    {
        monster.animator.SetFloat("speed", 0f);
        monster.animator.SetBool("Attacking", false);
    }
}

public class MoveCommand : UnitCommand
{
    public Unit target;
    private bool stopped = false;
    public MoveCommand(Monster self, Unit newPosition)
    {
        monster = self;
        target = newPosition;
    }
    public override void DoCommand()
    {
        // monster.Jump(target);
        monster.animator.SetBool("Attacking", false);
        monster.animator.SetFloat("speed", 0.5f);
        
        if (stopped && monster.isGrounded)
        {
            HelpMonster();
        }
        else
        {
            monster.MoveToPoint(target.transform.position);
        }
        SpecialLogic();
    }


    private void HelpMonster()
    {
        monster.animator.SetFloat("speed", 0f);
    }

    private void SpecialLogic()
    {
        if (monster.oldPositionsTime > 1)
        {
            float[] positionsX = new float[monster.oldPosiztions.Count];
            for (int i = 0; i < monster.oldPosiztions.Count; i++)
            {
                positionsX[i] =  monster.oldPosiztions[i].x;
            }
            float max =  Mathf.Max(positionsX);
            float min = Mathf.Min(positionsX);
            float distance = max - min;

            if (distance < 1f)
            {
                stopped = true;
                if (monster.isGrounded)
                {
                    monster.rigibody.velocity = new Vector2(2f, 10f);

                }
            }
        }
    }


}
public class AttackCommand : UnitCommand
{
    public Unit target;
    public AttackCommand(Monster paramUnit, Unit paramTarget)
    {
        monster = paramUnit;
        target = paramTarget;
    }
    public override void DoCommand()
    {
        monster.animator.SetFloat("speed", 0f);
        monster.animator.SetBool("Attacking", true);
    }
}

public class RunAttackCommand : UnitCommand
{
    public Unit target;
    public MoveCommand moveCommand;
    public AttackCommand attackCommand;
    public RunAttackCommand(Monster self, Unit paramTarget)
    {
        monster = self;
        target = paramTarget;
        moveCommand = new MoveCommand(self, paramTarget);
        attackCommand = new AttackCommand(self, paramTarget);
    }
    public override void DoCommand()
    {
        Vector3 direction = target.transform.position - monster.transform.position;
        monster.Flip(direction);

        monster.animator.SetBool("Attacking", true);
        monster.animator.SetFloat("speed", 0.5f);
        monster.MoveToPoint(target.transform.position);
    }

}

public class TiredCommand : UnitCommand
{
    public float commandTime = 5f;
    public bool isActivate = false;
    public TiredCommand(Monster self)
    {
        monster = self;
    }
    public override void DoCommand()
    {
        if (!isActivate)
        {
            isActivate = true;
            monster.godMode = true;
            monster.gameObject.layer = 13;
            var allChildComponents = monster.GetComponentsInChildren<Transform>();
            foreach (var child in allChildComponents)
            {
                child.gameObject.layer = 13;
            }
            monster.animator.SetFloat("speed", 0f);
            monster.animator.SetBool("Attacking", false);
            monster.animator.SetTrigger("Tired");
            monster.OffGodMode(4f);
            monster.ReplenishHealth(4);
        }
    }

    protected void OffAnimationTired()
    {
        monster.animator.SetBool("Tired", false);
    }

    

}