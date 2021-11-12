using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Municion : MonoBehaviour
{
    public Rigidbody rb;
    public List<SingleStats> stats = new List<SingleStats>();

    public int VelocidadBala
    {
        get
        {
            var r = stats.VB().Min;
            if(r <= 1)
            {
                return 1;
            }
            else
            {
                return r;
            }
        }
    }
    public int Rango
    {
        get
        {
            var r = stats.RG().Min;
            if(r <= 1)
            {
                return 1;
            }
            else
            {
                return r;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        stats = new List<SingleStats>();
        stats = name.TO_OBJECT_LIST(ref stats);
        rb.AddRelativeForce(0f, 0f, VelocidadBala);
        //Debug.Log("Velocidad Bala: " + VelocidadBala);
        Invoke("Destruir", Rango);
    }
    public void Destruir ()
    {
        Destroy(gameObject, 0f);
    }
}
