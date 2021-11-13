using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float Delay = 3f;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyMe", Delay);
    }
    public void DestroyMe ()
    {
        Destroy(gameObject, 0f);
    }
}