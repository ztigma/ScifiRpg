using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    public float speed = 1;
    public Vector3 IniPosition;
    public SendLineModel sendLineModel;

    void Start ()
    {
        IniPosition = sendLineModel.target.localPosition;
    }
    // Update is called once per frame
    void Update()
    {
        sendLineModel.isHit();
    }
    public void OnHit ()
    {
        sendLineModel.target.position = sendLineModel.data.point;
    }
    public void OnMiss ()
    {
        sendLineModel.target.localPosition = 
        Vector3.Lerp
        (
            sendLineModel.target.localPosition
            ,
            IniPosition
            , 
            speed * Time.fixedDeltaTime
        )
        ;
    }
}
[System.Serializable]
public class SendLineModel
{
    public Transform from;
    public Transform target;
    public LayerMask layer;
    public RaycastHit data;
    public UnityEvent OnHit;
    public UnityEvent OnMiss;
    public bool isHit()
    {
        var b = Physics.Linecast(from.position, target.position, out data, layer);
        if(b)
        {
            OnHit.Invoke();
        }
        else
        {
            OnMiss.Invoke();
        }
        return b;    
    }
}