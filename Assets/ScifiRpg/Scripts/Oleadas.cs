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
        var g = prefab.Instantiate();
        g.gameObject.SetActive(true);
    }
}
