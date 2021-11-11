using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorDeCubos : MonoBehaviour
{
    public Transform RelativeTo;
    public float distanceLimit;
    public GameObject prefab;
    public int Limit = 3000;
    public Vector3 Vector3Limit;
    public PermaBricks Memoria;

    void Start ()
    {
        Memoria = new PermaBricks(Memoria.FileName);//LOAD PERSISTENCE
        foreach (var i in Memoria.fileContent)
        {
            if(i.gameObject == null)
            {
                i.gameObject = prefab.Instantiate();
                i.gameObject.transform.position = i.position;
                i.gameObject.transform.parent = transform;
                i.gameObject.SetActive(true);
            }
        }
    }
    void OnApplicationQuit ()
    {
        Memoria.fileContent.ForEach(n => n.position = n.gameObject.transform.position);
        Memoria.Save();//SAVE PERSISTENCE
    }
    // Start is called before the first frame update
    void FixedUpdate()
    {
        if(Memoria.fileContent.Count < Limit)
        {
            Memoria.fileContent.RandomBrick(transform, prefab, RelativeTo, Vector3Limit);
        }
        Memoria.fileContent.DestroyMostFar(RelativeTo, distanceLimit);
    }
}
[System.Serializable]
public class PermaBricks : PermaList<BrickModel>
{
    public PermaBricks(string File_Name)
    {
        FileName = File_Name;
        _fileContent = variable;
    }
}
[System.Serializable]
public class BrickModel
{
    [System.NonSerialized] public GameObject gameObject;
    public Vector3 position;

    public BrickModel (GameObject g)
    {
        gameObject = g;
        position = g.transform.position;
    }
}
public static class BrickModelMethods
{
    public static bool RandomBrick (this List<BrickModel> l, Transform parent, GameObject prefab, Transform RelativeTo, Vector3 Limits)
    {
        var r = (-Limits).Random_Vector3_int(Limits) + RelativeTo.position.Vector3_int();
        if(l.Exists(n => n.position == r))
        {
            return false;
        }
        else
        {
            var p = prefab.Instantiate();
            p.SetActive(true);
            p.transform.parent = parent;
            p.transform.position = r;
            var w = new BrickModel(p);
            l.Add(w);
            return true;   
        }
    }
    public static int DestroyMostFar(this List<BrickModel> l, Transform RelativeTo, float distanceLimit)
    {
        return l.RemoveAll(n => Vector3.Distance(n.gameObject.transform.position, RelativeTo.position) > distanceLimit);
    }
}

