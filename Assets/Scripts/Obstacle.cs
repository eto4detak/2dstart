using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.gameObject.GetComponent<Unit>();

        if (unit && unit is Hero)
        {
            unit.ReceiveDamage();
        }
    }

}
