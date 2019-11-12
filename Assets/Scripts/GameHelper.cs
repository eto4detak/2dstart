using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameHelper : MonoBehaviour
{
    public GameObject box;

    private const float SizeX = 4f;
    private const float SizeY = 4f;
    private const float StartX = 10f;
    private const float StartY = 10f;

    void Start()
    {
        //CreateBoxs();
    }
    void Update()
    {
        //OnChangeColor();
    }
    void CreateBoxs()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Vector2 newPos = new Vector2(i * SizeX, j * SizeY);
                GameObject newBox = Instantiate(box, newPos, Quaternion.identity) as GameObject;
            }
        }
    }
    void OnChangeColor()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                hit.collider.GetComponent<SpriteRenderer>().color = Color.blue;
            }
        }
    }

}
