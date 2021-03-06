using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobRandom : MonoBehaviour
{
    public DamageBody damageBody
    {
        get
        {
            return GetComponent<DamageBody>();
        }
    }
    public ListGameObject listGameObjects;
    public bool isFirsLoad = true;

    void Start ()
    {
        if(isFirsLoad)
        {
            damageBody.character = new Character().RandomCharacter
            (Player.MySelf.character.fileContent.lvl);

            name = damageBody.character.Name;

            listGameObjects.Mobs.Add(gameObject);
            isFirsLoad = false;
        }
    }
}