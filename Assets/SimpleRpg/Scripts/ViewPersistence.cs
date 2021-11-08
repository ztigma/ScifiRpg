using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPersistence : MonoBehaviour
{
    public GameObject Prefab;
    public string FileName;
    public string tipo;
    public bool inEnable = true;
    public List<ViewPersistenceModel> PersistenceModels = new List<ViewPersistenceModel>();

    void OnEnable ()
    {
        if(inEnable)
        {
            Load();
        }
    }
    public void Save ()
    {
        Full();
        FileName.SET_PERSISTENCE(ref PersistenceModels);
    }
    void Full()
    {
        var w = transform.FIND_ALL(n => n.gameObject.activeInHierarchy, tipo);
        PersistenceModels = w.ConvertAll(n => new ViewPersistenceModel(n.name, n.TYPE(), n.TO_JSON()));
    }
    public void Load ()
    {
        if(FileName.EXIST_PERSISTENCE())
        {
            PersistenceModels = FileName.GET_PERSISTENCE(ref PersistenceModels);
            Show(PersistenceModels);
        }
        else
        {
            Debug.Log("Persistence dont exist", this);
        }
    }
    void Show (List<ViewPersistenceModel> o)
    {
        transform.DESTROY_ALL_GAMEOBJECT<Transform>(n => n.gameObject.activeInHierarchy);

        foreach(var c in o)
        {
            if(c == null)
            {
                continue;
            }
            GameObject p = Instantiate(Prefab, transform);
            p.SetActive(true);
            p.name = c.name;
            var gc = p.GetComponent(tipo);
            c.json.TO_OBJECT(ref gc);
        }
    }
}
[System.Serializable]
public class ViewPersistenceModel
{
    public string name;
    public string tipo;
    [TextArea]
    public string json;
    public ViewPersistenceModel (string n, string t, string j)
    {
        name = n;
        tipo = t;
        json = j;
    }
}