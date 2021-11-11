using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListGameObject : MonoBehaviour
{
    public string FileName = "Mobs";
    public GameObject Prefab;
    public List<GameObject> Mobs = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Mobs.Load(Prefab, FileName);
    }
    void Update ()
    {
        Mobs.RemoveAll(n => n == null);
    }
    // Update is called once per frame
    void OnApplicationQuit()
    {
        Mobs.Save(FileName);
    }
}