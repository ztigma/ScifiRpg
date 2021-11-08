using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameToText : MonoBehaviour
{

    public GameObject NAME;

    public Text TEXT;

    // Start is called before the first frame update
    void Start()
    {
        TEXT.text = NAME.name;
    }
}