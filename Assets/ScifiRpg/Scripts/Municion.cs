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
            return stats.VB().Min;
        }
    }
    public int Rango
    {
        get
        {
            return stats.RG().Min;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        stats = new List<SingleStats>();
        stats = name.TO_OBJECT_LIST(ref stats);
        rb.AddRelativeForce(0f, 0f, VelocidadBala);
        Invoke("Destruir", Rango);
    }
    public void Destruir ()
    {
        Destroy(gameObject, 0f);
    }
}
