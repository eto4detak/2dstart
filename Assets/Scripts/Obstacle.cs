using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    
    protected int health = 1;
    public int maxHealth = 1;
    protected Rigidbody2D rigibody;

    protected virtual void Start()
    {
        rigibody = GetComponent<Rigidbody2D>();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Weapon")
        {
            Unit Unit = other.GetComponentInParent<Unit>();
            if (Unit && Unit.animator.GetBool("Attacking"))
            {
                ReceiveDamage();
            }

        }
    }
    private void OnCollisionStay2D(Collision2D other)
    {
      //  Debug.Log("Collision2D " + other.gameObject.tag);
    }


    private void ReceiveDamage()
    {
        health--;
        if(health <= 0)
        {
            Destroy(gameObject, 1f);

        }
    }


}
