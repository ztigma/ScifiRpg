using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaToText : MonoBehaviour
{
    public DamageBody damageBody;
    public TextMesh text;


    // Update is called once per frame
    void Update()
    {
        var hp = damageBody.character.StatsFinal.HP();
        text.text = hp.Min + " / " + hp.Max;
    }
}