using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListView : MonoBehaviour
{
    public GameObject prefabItemTemplate;
    public List<TemplateModel> ItemTemplate;
    public List<SourceModel> ItemSource;

    public void Show ()
    {
        transform.DESTROY_ALL_GAMEOBJECT<Transform>(n => n.gameObject.activeSelf);
        
        foreach(var s in ItemSource)
        {
            foreach(var t in ItemTemplate)
            {
                t.Start();
                //Debug.Log(t.receptor + " : " + t.propiedad_receptora + " : " + s.model + " : " + t.propiedad_emisora);
                if(t.receptor.SET_VALUE(t.propiedad_receptora, s.model.GET_VALUE(t.propiedad_emisora)))
                {

                }
                else
                {
                    Debug.LogError("FAIL");
                    Debug.Log(t.receptor.PROPERTYS().The_List.IE_TO_STRING());
                }
            }
            var g = Instantiate(prefabItemTemplate, transform);
            g.SetActive(true);
            g.name = s.id;
        }
    }
}
[System.Serializable]
public class SourceModel
{
    public string id;
    public object model;
}
[System.Serializable]
public class TemplateModel
{
    public string propiedad_receptora;
    public Component receptor;
    public string tipo;
    public string propiedad_emisora;
    public void Start ()
    {
        if(tipo == receptor.TYPE())
        {

        }
        else
        {
            receptor = receptor.GetComponent(tipo);
        }
    }
}