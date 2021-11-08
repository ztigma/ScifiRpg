using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDetector : MonoBehaviour
{

    public float distance;
    public float current;
    public List<ViewPersistence> persistence;

    void Update ()
    {
        Detect();
    }
    void Detect ()
    {
        if(transform.childCount > 0)
        {
            return;
        }

        if(Mouse2D.MySelf.isPick)
        {
            var d = Vector3.Distance(transform.position, Mouse2D.MySelf.transform.position);
            var b = d < distance;
            current = d;
            if(b)
            {
                Mouse2D.MySelf.Pick.position = transform.position;
                Mouse2D.MySelf.Pick.parent = transform;
                persistence.ForEach(n => n.Save());
            }
        }
    }
}