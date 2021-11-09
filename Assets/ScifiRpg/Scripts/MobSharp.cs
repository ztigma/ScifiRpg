using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MobSharp : MonoBehaviour
{
    public int index = 0;
    public float RePensar = 3f;
    public List<MobSharpModel> mobSharpModels;

    void Start ()
    {
        InvokeRepeating("Repensando", 0f, RePensar);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        mobSharpModels[index].PatronesDeCombate.Invoke();
    }
    public void Repensando ()
    {
        index = (0).RandomCount(mobSharpModels.Count);
    }
}
[System.Serializable]
public class MobSharpModel
{
    public string Name;
    public UnityEvent PatronesDeCombate;
}