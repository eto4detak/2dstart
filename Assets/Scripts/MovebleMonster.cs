using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MovebleMonster : Monster
{
    [SerializeField]
    private float speed = 2.0f;

    private Vector3 direction;

    private Bullet bullet;

    protected override void Awake()
    {
        bullet = Resources.Load<Bullet>("Bullet");
    }

    protected override void Update()
    {
        Move();
    }
    protected override void Start()
    {
        direction = transform.right;
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.GetComponent<Unit>();

        if (unit && unit is Hero)
        {

        }

    }

    private void Move()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.5f + transform.right * direction.x * 0.5f, 0.2f);
        if (collider.Length > 0) 
        {
            direction *= 1.0f;
        }

        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }


}
