using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Player3D : MonoBehaviour
{

    public Player player;
    public Transform Player3d_Transform;
    public Rigidbody Player3d_Rb;
    public float MoveOffset = 1000;
    public float RotateOffset = 1000;
    public float DragOffset = 0.25f;
    public float VelocidadAhora;

    public void SpeedToText(Text t)
    {
        t.text = VelocidadAhora + "";
    }
    public void UiRotate(Text vector3)
    {
        var rotacion = player.character.fileContent.StatsFinal.RO();
        var formula = vector3.text.ToVector3().PlusVector3Predicate(rotacion.Min, n => n != 0) * Time.fixedDeltaTime;
        Player3d_Transform.Rotate(formula);
    }
    public void Rotate(string vector3)
    {
        var rotacion = player.character.fileContent.StatsFinal.RO();
        var formula = vector3.ToVector3().PlusVector3Predicate(rotacion.Min, n => n != 0) * Time.fixedDeltaTime;
        Player3d_Transform.Rotate(formula);
    }
    public void Move(string vector3)
    {
        var velocidad = player.character.fileContent.StatsFinal.VE();
        var formula = (vector3.ToVector3() * velocidad.Min) * Time.fixedDeltaTime;
        Player3d_Rb.AddRelativeForce(formula * MoveOffset);
    }
    void FixedUpdate ()
    {
        Player3d_Rb.drag = Player3d_Rb.velocity.magnitude * DragOffset;// mas rapido mas resistencia a la velocidad
        Player3d_Rb.angularDrag = Player3d_Rb.velocity.magnitude * DragOffset;// mas rapido mas control de la rotacion
        VelocidadAhora = Player3d_Rb.velocity.magnitude;
    }
    public void MouseInvert ()
    {
        Cursor.visible = !Cursor.visible;
    }
    public void MouseOff ()
    {
        Cursor.visible = false;
    }
}
public static class VectorMethods
{
    public static Vector3 ToVector3(this string vector3)
    {
        var r = vector3.Splitting(new string[] { "x", "y", "z" });
        return new Vector3(r[0].TO_FLOAT(), r[1].TO_FLOAT(), r[2].TO_FLOAT());
    }
    public const string ToUiVectorFormat = "{0}x{1}y{2}z";
    public static string ToUiVector3(this Vector3 vector3)
    {
        return ToUiVectorFormat.TO_FORMAT(vector3.x, vector3.y, vector3.z);
    }
    public static string ToUiVector3(this Vector3 vector3, string format)
    {
        return format.TO_FORMAT(vector3.x, vector3.y, vector3.z);
    }
    public static Vector3 MultiplyVector3(this Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }
    public static Vector3 PlusVector3Predicate(this Vector3 a, Variable b, Predicate<float> p)
    {
        if(p.Invoke(a.x))
        {
            a += new Vector3(b, 0f, 0f);
        }
        if(p.Invoke(a.y))
        {
            a += new Vector3(0f, b, 0f);
        }
        if(p.Invoke(a.z))
        {
            a += new Vector3(0f, 0f, b);
        }
        return a;
    }
}