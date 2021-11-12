using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFishEyeFx : MonoBehaviour
{
    public float InitEye;
    public Camera cam;
    public Text textSpeed;
    void Start ()
    {
        InitEye = cam.fieldOfView;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        cam.fieldOfView = InitEye + (textSpeed.text.TO_FLOAT() / 10);
    }
}