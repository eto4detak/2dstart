using UnityEngine;

public class CamMove : MonoBehaviour
{
    public GameObject player;

    private float speed = 2.0f;

    private Transform target;
    private void Awake()
    {
        if (!target) target = FindObjectOfType<Hero>().transform;
    }


    void Update()
    {
        //transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
        Vector3 position = target.position;
        position.z = -10;
        position.y =  0;
        transform.position = Vector3.Lerp(transform.position, position, speed);
    }
}
