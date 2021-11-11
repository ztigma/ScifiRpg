using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonoPersistence : MonoBehaviour
{
    public MonoBehaviour ToSave;
    public string fileName;
    public UnityEvent MethodsInSave;
    public UnityEvent MethodsInLoad;

    // Start is called before the first frame update
    void Start()
    {
        if(fileName.EXIST_PERSISTENCE())
		{
			fileName.GET_PERSISTENCE(ref ToSave);
            MethodsInLoad.Invoke();
		}
    }
    void OnApplicationQuit ()
    {
        MethodsInSave.Invoke();
        fileName.SET_PERSISTENCE(ToSave);
    }
}
[System.Serializable]
public class MonoMethod
{
    public MonoBehaviour Mono;
    public string Tipo;
    public string MethodName;
    public void Invoke ()
    {
        if(Mono.TYPE() == Tipo)
        {
            Mono.Invoke(MethodName, 0f);
        }
        else
        {
            Mono = (MonoBehaviour)Mono.GetComponent(Tipo);
            Mono.Invoke(MethodName, 0f);
        }
    }
}