using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrantOrc : MovebleMonster
{
    protected override void Start()
    {
        base.Start();
        SizeX = 2.4f;
        SizeY = 3f;
        attackRadius = 2f;
    }

    protected override void Update()
    {
        base.Update();
    }
    public override void Flip(Vector3 direction)
    {
        var s = transform.localScale;

        if (direction.x < 0.0f)
        {
            s.x = Mathf.Abs(s.x) *  -1;
            transform.localScale = s;
        }
        else
        {
            s.x = Mathf.Abs(s.x);
            transform.localScale = s;
        }

    }

}
