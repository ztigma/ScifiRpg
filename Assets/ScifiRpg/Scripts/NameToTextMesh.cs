using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameToTextMesh : MonoBehaviour
{

    public GameObject target;
    public TextMesh text;

    public bool LookAt = true;

    // Update is called once per frame
    void Update()
    {
        if(LookAt)
        {
            transform.LookAt(Camera.main.transform);
        }
        text.text = target.name;
    }
}
