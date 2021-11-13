using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metralleta : MonoBehaviour
{
    public DamageBody damageBody;
    public GameObject prefab;
    public const string ID = "(ID?)";
    void OnEnable ()
    {
        var w = Instantiate(prefab, transform.position, transform.rotation);

        w.name = damageBody.character.StatsFinal.TO_JSON_LIST()+ 
        ID + 
        transform.root.GetInstanceID();

        w.SetActive(true);
    }
}