using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private bool canDrag = true;
    private int startIndex = 1;

    private int currentIndex;
    public bool isStartGame = false;
    [SerializeField] List<GameObject> particleVFXs;
    
    List<TouchPoint> listAllTouchPoint = new List<TouchPoint>();
    List<TouchPoint> listCollected = new List<TouchPoint>();
    private TouchPoint fistCollected;
    [SerializeField] private List<Color> listColorRandom;
    private int id1 = 0;
    private int id2 = 0;
    private int id3 = 0;
    private int id4 = 0;
    private int id5 = 0;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        currentIndex = startIndex;
        RandomDataLevel();
        
    }

    void RandomDataLevel()
    {
        listAllTouchPoint.Clear();
        listAllTouchPoint = FindObjectsOfType<TouchPoint>(true).ToList();
        foreach (var tp in listAllTouchPoint)
        {
            tp.gameObject.SetActive(true);
        }
        canDrag = true;
        isStartGame = true;
        int count = currentIndex + 3;
        id1 = 0;
        id2 = 0;
        id3 = 0;
        id4 = 0;
        id5 = 0;
        foreach (var tp in listAllTouchPoint)
        {
            int id = Random.Range(1, count);
            AddId(id);
            tp.SetData(listColorRandom[id-1],id);
        }
    }

    void NextLevel()
    {
        currentIndex++;
        if (currentIndex > 3) currentIndex = startIndex;
        GameObject explosion = Instantiate(particleVFXs[Random.Range(0,particleVFXs.Count)], transform.position, transform.rotation);
        Destroy(explosion, .75f);
        Invoke(nameof(RandomDataLevel),1.0f);
    }
    void AddId(int id)
    {
        switch (id)
        {
            case 1 : id1++;break;
            case 2 : id2++;break;
            case 3 : id3++;break;
            case 4 : id4++;break;
            case 5 : id5++;break;
        }
    }
    
    void Update()
    {
        if(!canDrag) return;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            listCollected.Clear();
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);
            if (targetObject)
            {
                var tp = targetObject.GetComponent<TouchPoint>();
                if (tp != null)
                {
                    fistCollected = tp;
                    tp.SetCollected();
                    listCollected.Add(tp);
                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);
            if (targetObject)
            {
                var tp = targetObject.GetComponent<TouchPoint>();
                if (tp != null)
                {
                    if (fistCollected == null)
                    {
                        fistCollected = tp;
                        tp.SetCollected();
                        listCollected.Add(tp);
                    }else if (tp.id == fistCollected.id)
                    {
                        tp.SetCollected();
                        listCollected.Add(tp);
                    }
                }
            }
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            
            if (listCollected.Count >0)
            {
                if (CheckId(fistCollected.id))
                {
                    foreach (var tr in listCollected)
                    {
                        tr.gameObject.SetActive(false);
                        listAllTouchPoint.Remove(tr);
                        GameObject explosion = Instantiate(particleVFXs[Random.Range(0,particleVFXs.Count)], tr.transform.position, transform.rotation);
                        Destroy(explosion, .75f);
                    }

                    if (listAllTouchPoint.Count == 0)
                    {
                        NextLevel();
                    }
                }
                else
                {
                    foreach (var tp in listCollected)
                    {
                        tp.SetUnCollected();
                    }
                }
            }
            fistCollected = null;
        }
    }

    bool CheckId(int id)
    {
        switch (id)
        {
            case 1 : return listCollected.Count == id1;
            case 2 : return listCollected.Count == id2;
            case 3 : return listCollected.Count == id3;
            case 4 : return listCollected.Count == id4;
            case 5 : return listCollected.Count == id5;
            default: return false;
        }
    }
}