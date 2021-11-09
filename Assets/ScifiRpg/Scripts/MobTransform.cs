using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobTransform : MonoBehaviour
{
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
        transform.position = damageBody.character.position;
        transform.rotation = damageBody.character.rotation;
    }
    // Update is called once per frame
    void Update()
    {
        damageBody.character.position = transform.position;
        damageBody.character.rotation = transform.rotation;
    }
}
