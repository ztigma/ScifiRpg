using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameToTextMesh : MonoBehaviour
{

    public GameObject target;
    public TextMesh text;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);
        text.text = target.name;
    }
}
