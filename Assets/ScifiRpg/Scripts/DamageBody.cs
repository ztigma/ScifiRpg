﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBody : MonoBehaviour
{
    public const string DamageTag = "Damage";
    public Character character;

    public float coolMetra
    {
        get
        {
            var w = 1f - (character.StatsFinal.ATS().Min / (float)Top);
            if(w < 0)
            {
                w = 0;
            }
            character.StatsFinal.ATS().Average = w;
            return w;
        }
    }
    public float memoMetra;

    void Start ()
    {
        memoMetra = 0;
        InvokeRepeating("Regenerar", 0f, 60f);
    }
    public void Regenerar()
    {
        var hp = character.StatsFinal.HP();
        if(hp.Min < hp.Max)
        {
            hp.Min++;
        }
    }
    public void Disparar (GameObject o)
    {
        if(Time.time > coolMetra + memoMetra)
        {
            o.SetActive(true);
            memoMetra = Time.time;
        }
    }
    public void OnCollisionEnter (Collision c)
    {
        var o = c.transform.root;
        if(o.tag != DamageTag) { return; }

        character.StatsFinal.HP(n => n.Min -= DamageProcess(o.name));
    }
    public const int Top = 1000;
    public int DamageProcess(string damage)
    {
        List<SingleStats> statsDamage = new List<SingleStats>();
        statsDamage = damage.TO_OBJECT_LIST(ref statsDamage);

        int r = statsDamage.AT().Min;

        var ev = character.StatsFinal.EV();

        if(ev.Min > 1)//evasion
        {
            var t = Top + statsDamage.PE().Min;
            int u = (0).RandomMinMax(t);
            if(u <= ev.Min)
            {
                Debug.Log("Evadido, top = " + t, this);
                r /= ev.Min;
                character.StatsFinal.EV(n => n.Min -= 1);
            }
        }

        var df = character.StatsFinal.DF();

        if(df.Min > 1)//defensa
        {
            Debug.Log("Defendido", this);
            r -= df.Min;
            character.StatsFinal.DF(n => n.Min -= 1);
        }

        if(r < 0)
        {
            r = 0;
        }

        Debug.Log("Damage = " + r, this);
        return r;
    }
}