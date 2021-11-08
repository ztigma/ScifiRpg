using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    public GameObject PopUpGraphic;
    public static PopUp MySelf;
    public InputField Title;
    public InputField Content;
    public Text Accept;
    public PopUp ()
    {
        MySelf = this;
    }
    void Start ()
    {
        MySelf = this;
    }

    public void Show(string title, string content, string accept = "Accept")
    {
        Title.text = title;
        Content.text = content;
        Accept.text = accept;
        PopUpGraphic.SetActive(true);
    }
}