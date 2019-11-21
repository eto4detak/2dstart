using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

   public static Hero hero;
   public static MessageConvas messageConvas;
    public static List<Unit> allUnit = new List<Unit>();
    
    void Start()
    {
        hero = (Hero)FindObjectOfType(typeof(Hero));
        
        messageConvas = (MessageConvas)FindObjectOfType(typeof(MessageConvas));
    }

    void Update()
    {
        
    }


}
