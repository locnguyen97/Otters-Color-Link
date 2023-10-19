using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPoint : MonoBehaviour
{
    private bool isSelected = false;
    public int id = 0;
    public void SetData(Color color,int id)
    {
        this.id = id;
        isSelected = false;
        GetComponent<SpriteRenderer>().color = color;
        GetComponent<BoxCollider2D>().enabled = true;
        foreach (Transform tr in transform)
        {
            tr.gameObject.SetActive(false);
        }
        transform.GetChild(Random.Range(0,transform.childCount-1)).gameObject.SetActive(true);
    }

    public void SetCollected()
    {
        if(isSelected) return;
        isSelected = true;
        transform.GetChild(transform.childCount-1).gameObject.SetActive(true);
        GetComponent<BoxCollider2D>().enabled = false;
    }
    public void SetUnCollected()
    {
        isSelected = false;
        transform.GetChild(transform.childCount-1).gameObject.SetActive(false);
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
