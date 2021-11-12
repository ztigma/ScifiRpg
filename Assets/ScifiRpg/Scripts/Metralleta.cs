using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metralleta : MonoBehaviour
{
    public DamageBody damageBody;
    public GameObject prefab;

    void OnEnable ()
    {
        var w = Instantiate(prefab, transform.position, transform.rotation);
        w.name = damageBody.character.StatsFinal.TO_JSON_LIST();
        w.SetActive(true);
    }
}