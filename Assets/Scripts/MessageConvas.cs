using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageConvas : MonoBehaviour
{
    private static UnitMessage message;

    private void Awake()
    {
        message = (UnitMessage)FindObjectOfType(typeof(UnitMessage));
    }

    public void Message(Unit target, string txt, float timer = 1f)
    {

        if (target.messageManager)
        {
            StartCoroutine(AddMessage(target, txt, timer));
        }
        else
        {
            Debug.Log("No messageManager");
        }
    }

    IEnumerator AddMessage(Unit target, string txt,  float delayTime)
    {
        var child = message.gameObject;
        var newChild = Instantiate(child, target.messageManager.transform.position, Quaternion.identity);
        newChild.name = child.name;
        newChild.GetComponent<Text>().text = txt;
        newChild.transform.SetParent(target.messageManager.transform);
        yield return new WaitForSeconds(delayTime);
        Destroy(newChild);
    }


}
