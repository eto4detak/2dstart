using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public float speed = 20f;

    private Rigidbody2D mainHero;
    private bool faceRight = false;

    void Start()
    {
        mainHero = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        mainHero.MovePosition(mainHero.position + Vector2.right * moveX * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            mainHero.AddForce(Vector2.up * 8000);
        }

        if (moveX > 0 && !faceRight)
        {
            flip();
        }
        else if (moveX < 0 && faceRight)
        {
            flip();
        }
        void flip()
        {
            faceRight = !faceRight;
            transform.localScale =
                new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }


}

