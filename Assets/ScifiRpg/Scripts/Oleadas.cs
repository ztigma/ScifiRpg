using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oleadas : MonoBehaviour
{
    public DamageBody prefab;
    public float TimeSpawn = 60;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", 0f, TimeSpawn);
    }
    public void Spawn ()
    {
        Player.MySelf.character.fileContent.intentos += 1;

        var c = Player.MySelf.character.fileContent.lvl;

        var rc = (1).RandomMinMax(c);

        if(rc > 30)
        {
            rc = 30;
        }

        for (int i = 0; i < rc; i++)
        {
            var g = prefab.Instantiate();
            Debug.Log(g.character.position);
            g.gameObject.SetActive(true);
        }
    }
}
