using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBody : MonoBehaviour
{
    public const string DamageTag = "Damage";
    public Character character;
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
        statsDamage = damage.TO_OBJECT(ref statsDamage);

        int r = statsDamage.AT().Min;

        var ev = character.StatsFinal.EV();

        if(ev.Min > 0)
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

        if(df.Min > 0)
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