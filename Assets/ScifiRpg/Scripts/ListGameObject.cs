using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ListGameObject : MonoBehaviour
{
    public string FileName = "Mobs";
    public GameObject Prefab;
    public int Range;
    public UnityEvent OutRange;
    public UnityEvent InRange;
    public List<GameObject> Mobs = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Mobs.Load(Prefab, FileName);
    }
    void Update ()
    {
        Mobs.RemoveAll(n => n == null);
        if(Mobs.Count < Range)
        {
            InRange.Invoke();
        }
        else
        {
            OutRange.Invoke();
        }
    }
    // Update is called once per frame
    void OnApplicationQuit()
    {
        Mobs.Save(FileName);
    }
}