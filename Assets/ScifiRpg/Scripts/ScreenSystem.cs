using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using UnityEngine.UI;

public class ScreenSystem : MonoBehaviour
{
    public List<ScreenModel> Screens;

    void OnValidate ()
    {
        Screens.ForEach(n => n.Name = n.screenCode.ToString());
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(EventSystem.current.IsPointerOverGameObject(-1)){ return; }

        Screens.ForEach(n => n.Update());
    }
}
[System.Serializable]
public class ScreenModel
{
    public string Name;
    public ScreenCode screenCode;
    public ScreenInput screenInput;
    public string format = "{0}x{1}y{2}z";
    public Text ToText;
    public UnityEvent Method;
    public void Update ()
    {
        Vector3 v = screenInput;
        if(ToText != null)
        {
            ToText.text = v.ToUiVector3(format);
        }

        if(screenCode == ScreenCode.Xplus)
        {
            if(v.x > 0f)
            {
                Method.Invoke();
            }
        }
        else if(screenCode == ScreenCode.Xminus)
        {
            if(v.x < 0f)
            {
                Method.Invoke();
            }
        }
        else if(screenCode == ScreenCode.Yplus)
        {
            if(v.y > 0f)
            {
                Method.Invoke();
            }
        }
        else if(screenCode == ScreenCode.Yminus)
        {
            if(v.x < 0f)
            {
                Method.Invoke();
            }
        }
        else if(screenCode == ScreenCode.AllVectors)
        {
            Method.Invoke();
        }
    }
}
public enum ScreenCode { Xplus, Xminus, Yplus, Yminus, AllVectors }
[System.Serializable]
public class ScreenInput
{
    public Vector3 OffSet;
    [SerializeField] private Vector3 ScreenPosition;
    public static implicit operator Vector3 (ScreenInput o)
    {
        var p =  Camera.main.pixelRect;
        var w = p.width - p.center.x;
        var h = p.height - p.center.y;
        var m = new Vector3(Input.mousePosition.x - w, Input.mousePosition.y - h, 0f);
        o.ScreenPosition = new Vector3((m.x / w) * o.OffSet.x, (m.y / h) * o.OffSet.y, 0f);
        return o.ScreenPosition;
    }
}