using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoLayout : MonoBehaviour
{
    public Vector3 localPosition;
    void Update()
    {
        var w = new List<InfoView>(GetComponentsInChildren<InfoView>(false));
        w.ForEach(n => n.transform.localPosition = localPosition);
    }
}