using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobTransform : MonoBehaviour
{
    public Vector3 RandomPosition;
    private DamageBody _damageBody;
    public DamageBody damageBody
    {
        get
        {
            if(_damageBody == null)
            {
                return _damageBody = GetComponent<DamageBody>();
            }
            return _damageBody;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if(damageBody.character.position.magnitude == 0)
        {
            transform.position = RandomPosition.Random_Vector3();
            transform.rotation = Random.rotation;
            Debug.Log(transform.position);
        }
        else
        {
            transform.position = damageBody.character.position;
            transform.rotation = damageBody.character.rotation;
        }
    }
    // Update is called once per frame
    void Update()
    {
        damageBody.character.position = transform.position;
        damageBody.character.rotation = transform.rotation;
    }
}
