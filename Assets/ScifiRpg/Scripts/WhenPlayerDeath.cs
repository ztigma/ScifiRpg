using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WhenPlayerDeath : MonoBehaviour
{

    public Player player;
    public ListGameObject listGameObject;
    public GeneradorDeCubos generadorDeCubos;
    public UnityEvent OnPlayerDeath;

    // Update is called once per frame
    void Update()
    {
        if(player.character.fileContent.StatsFinal.HP().Min < 0)
        {
            player.persistenceVariable.fileName.DELETE_FILE();
            player.character.FileName.DELETE_FILE();
            listGameObject.FileName.DELETE_FILE();
            generadorDeCubos.Memoria.FileName.DELETE_FILE();
            OnPlayerDeath.Invoke();
            Player.Muertes.fileContent++;
        }
    }
}