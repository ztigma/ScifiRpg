using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Ejemplo de como seria trabajar online
///</summary>
public class OnlineCharacter : MonoBehaviour
{
    public bool isLocalPlayer;
    public bool isServer;
    public DamageBody body;
    public int lvl
    {
        get
        {
            return lvl;
        }
        set
        {
            if(isLocalPlayer)
            {
                body.character.lvl = value;
                CmdValue("Online_lvl", body.character.lvl.ToString());
            }
            else
            {
                if(isServer)
                {
                    
                }
                else
                {

                }
            }
        }
    }
    public List<Item> Inventory
    {
        get
        {
            return body.character.Inventory;
        }
        set
        {
            if(isLocalPlayer)
            {
                body.character.Inventory = value;
                CmdValue("Online_Inventory", body.character.Inventory.TO_JSON_RAW());
                //el accesor (get; set) del objeto debe tener el algoritmo de jsoneo en el set
                //body.character.SET_VALUE(Variable_Name, data);
            }
            else
            {
                if(isServer)
                {
                    
                }
                else
                {

                }
            }
        }
    }
    public void CmdValue (string Variable_Name, string data)
    {
        body.character.SET_VALUE(Variable_Name, data);
        RpcValue(Variable_Name, data);
    }
    public void RpcValue (string Variable_Name, string data)
    {
        body.character.SET_VALUE(Variable_Name, data);
    }
}