using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MovebleMonster : Monster
{

    private Bullet bullet;

    void Awake()
    {
        bullet = Resources.Load<Bullet>("Bullet");
    }
    protected override void Start()
    {
        base.Start();
        direction = transform.right;
    }

    protected override void Update()
    {
        base.Update();
    }


    protected override void OnTriggerEnter2D(Collider2D other)
    {
        Unit unit = other.GetComponent<Unit>();

        if (unit && unit is Hero)
        {

        }

        Bullet bullet = other.GetComponent<Bullet>();
        if (bullet)
        {
            ReceiveDamage(bullet.gameObject);
        }

    }

}
