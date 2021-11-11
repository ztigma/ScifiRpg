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

    public float rotationForce;

    public float Angulo;

    void OnValidate ()
    {
        damageBody.character.Update();
    }
    void FixedUpdate ()
    {
        Player3d_Rb.drag = Player3d_Rb.velocity.magnitude * DragOffset;// mas rapido mas resistencia a la velocidad
        Player3d_Rb.angularDrag = Player3d_Rb.velocity.magnitude * DragOffset;// mas rapido mas control de la rotacion
        VelocidadAhora = Player3d_Rb.velocity.magnitude;
    }
    public void SpeedToText(Text t)
    {
        t.text = VelocidadAhora + "";
    }
    public void UiRotate(Text vector3)
    {
        var rotacion = damageBody.character.StatsFinal.RO();
        var w = vector3.text.ToVector3();

        var e = new Vector3
        (-w.x * (30 + rotacion.Min), -w.y * (30 + rotacion.Min), -w.z * (30 + rotacion.Min));

        var formula = e * Time.fixedDeltaTime;
        
        Player3d_Transform.Rotate(formula);
    }
    public void Rotate(string vector3)
    {
        var rotacion = damageBody.character.StatsFinal.RO();
        var w = vector3.ToVector3();

        var e = new Vector3
        (-w.x * (30 + rotacion.Min), -w.y * (30 + rotacion.Min), -w.z * (30 + rotacion.Min));

        var formula = e * Time.fixedDeltaTime;
        Player3d_Transform.Rotate(formula);
    }
    public void Move(string vector3)
    {
        var velocidad = damageBody.character.StatsFinal.VE();
        var formula = (vector3.ToVector3() * velocidad.Min) * Time.fixedDeltaTime;
        Player3d_Rb.AddRelativeForce(formula * MoveOffset);
        velocidad.Average = VelocidadAhora;
    }

    public const float gradesOffset = 180;

    public void Apuntar(Transform o)
    {
        var rotacion = damageBody.character.StatsFinal.RO();
        Punteria.LookAt(o);


        rotationForce = (30f+rotacion.Min)/gradesOffset;

        Angulo = rotationForce * gradesOffset;

        rotacion.Average = Angulo;

        Player3d_Transform.rotation = Quaternion.SlerpUnclamped
        (damageBody.transform.rotation, Punteria.rotation, rotationForce * Time.fixedDeltaTime);
    }
}