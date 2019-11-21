using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinderObstacle : Obstacle
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        maxHealth = 2;
        health = maxHealth;
        rigibody.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
