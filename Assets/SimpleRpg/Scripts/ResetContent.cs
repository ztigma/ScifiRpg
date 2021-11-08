using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ResetContent : MonoBehaviour
{
    public RectTransform rectTransform;

    // Start is called before the first frame update
    void OnEnable()
    {
        rectTransform.anchoredPosition3D = new Vector3(0f, 0f, 0f);
    }
}
