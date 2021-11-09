using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mob3D : MonoBehaviour
{
    public DamageBody damageBody;
    public Transform Player3d_Transform;
    public Rigidbody Player3d_Rb;
    public float MoveOffset = 1000;
    public float RotateOffset = 1000;
    public float DragOffset = 0.25f;
    public float VelocidadAhora;
    public Transform Punteria;

    public void SpeedToText(Text t)
    {
        t.text = VelocidadAhora + "";
    }
    public void UiRotate(Text vector3)
    {
        var rotacion = damageBody.character.StatsFinal.RO();
        var formula = vector3.text.ToVector3().PlusVector3Predicate(rotacion.Min, n => n != 0) * Time.fixedDeltaTime;
        Player3d_Transform.Rotate(formula);
    }
    public void Rotate(string vector3)
    {
        var rotacion = damageBody.character.StatsFinal.RO();
        var formula = vector3.ToVector3().PlusVector3Predicate(rotacion.Min, n => n != 0) * Time.fixedDeltaTime;
        Player3d_Transform.Rotate(formula);
    }
    public void Move(string vector3)
    {
        var velocidad = damageBody.character.StatsFinal.VE();
        var formula = (vector3.ToVector3() * velocidad.Min) * Time.fixedDeltaTime;
        Player3d_Rb.AddRelativeForce(formula * MoveOffset);
    }
    public void Apuntar(Transform o)
    {
        var rotacion = damageBody.character.StatsFinal.RO();
        Punteria.LookAt(o);

        transform.rotation = Quaternion.Lerp
        (damageBody.transform.rotation, Punteria.rotation, (30f+rotacion.Min)/360f);
    }
}