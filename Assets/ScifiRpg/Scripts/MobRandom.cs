using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobRandom : MonoBehaviour
{
    public DamageBody damageBody;
    public ListGameObject listGameObjects;
    public bool isFirsLoad = true;

    void Start ()
    {
        if(isFirsLoad)
        {
            damageBody.character = new Character().RandomCharacter
            (Player.MySelf.character.fileContent.lvl + 10);

            name = damageBody.character.Name;

            listGameObjects.Mobs.Add(gameObject);
            isFirsLoad = false;
        }
    }
}