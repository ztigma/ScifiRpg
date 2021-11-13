using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhenMobDeath : MonoBehaviour
{
    public GameObject DestroyTarget;
    public DamageBody damageBody;

    void Start ()
    {
        damageBody.character.StatsFinal.FOR(n => n.IsMinimus, n => n.Min = n.Max);
    }
    // Update is called once per frame
    void Update()
    {
        var w = damageBody.character.StatsFinal.HP().Min;
        if(w < 0)
        {
            Debug.Log("hp: " + w + " Name: " + transform.root.name);
            Destroy(DestroyTarget, 0f);
            Player.MySelf.character.fileContent.cash += (1).RandomMinMax(damageBody.character.lvl);
            Player.MySelf.character.fileContent.kills++;
        }
    }
}
