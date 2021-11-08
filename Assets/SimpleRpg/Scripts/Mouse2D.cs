using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse2D : MonoBehaviour
{
    public static Mouse2D MySelf;
    public bool isPick
    {
        get
        {
            return transform.childCount > 0;
        }
    }
    public Transform Pick
    {
        get
        {
            if(isPick)
            {
                return transform.GetChild(0);
            }
            else
            {
                return null;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MySelf = this;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
    }
}