using System.IO;
using UnityEngine;

public class DieSpece : MonoBehaviour
{
    
    public GameObject respawn;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //Debug.Log("Player");
            //other.transform.parent.position = respawn.transform.position;
        }
    }
}
