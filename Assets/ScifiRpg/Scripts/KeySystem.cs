using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class KeySystem : MonoBehaviour
{
    public List<KeyModel> Keys;

    void OnValidate ()
    {
        Keys.ForEach(n => n.Name = n.Key.ToString());
    }
    void FixedUpdate ()
    {
        //if(EventSystem.current.IsPointerOverGameObject(-1)){ return; }

        Keys.ForEach(n => n.Update());
    }
}
public enum KeyEnum { GetKey, GetKeyDown, GetKeyUp }
[System.Serializable]
public class KeyModel
{
    public string Name;
    public KeyCode Key;
    public KeyEnum isGetKey;
    public UnityEvent Methods;

    public void Update ()
    {
        if(isGetKey == KeyEnum.GetKey)
        {
            if(Input.GetKey(Key))
            {
                Methods.Invoke();
            }
        }
        else if(isGetKey == KeyEnum.GetKeyDown)
        {
            if(Input.GetKeyDown(Key))
            {
                Methods.Invoke();
            }
        }
        else if(isGetKey == KeyEnum.GetKeyUp)
        {
            if(Input.GetKeyUp(Key))
            {
                Methods.Invoke();
            }
        }
    }
}