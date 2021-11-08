using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;

public class UiActions : MonoBehaviour
{
    public Player player
    {
        get
        {
            return Player.MySelf;
        }
    }
    public void InvertActive (GameObject o)
    {
        o.SetActive(!o.activeSelf);
    }
    public void Sleep ()
    {
        player.character.fileContent.Sleep();
        player.Update();
        player.InvokeOnLoad();
    }
    public void UpStat(Text t)
    {
        var c  = player.character.fileContent.StatsBase.Find(n => n.Name == t.text);
        if(c == null){ Debug.LogError("dont exist stat"); return;}
        if(player.character.fileContent.cash - 1 < 0)
        {
            PopUp.MySelf.Show("Out of Cash", "You are living in Venezuela");
        }
        else
        {
            c.Max += 1;
            var s = player.character.fileContent.StatsFinal.Find(n => n.Name == t.text);
            s.Min += 1;
            player.character.fileContent.cash -= 1;
            player.Update();
            player.InvokeOnLoad();
        }
    }
    public void ItemUnEquip (Text t)
    {
        var c = player.character.fileContent.Equiped.Find(n => n.Name == t.text);
        player.character.fileContent.Inventory.Add(c);
        player.character.fileContent.Equiped.RemoveAll(n => n.Id == c.Id);
        player.Update();
        player.InvokeOnLoad();
    }
    public void ItemEquip (Text t)
    {
        var c = player.character.fileContent.Inventory.Find(n => n.Name == t.text);

        if(player.character.fileContent.Equiped.Count < 4)
        {
            player.character.fileContent.Equiped.Add(c);
            player.character.fileContent.Inventory.RemoveAll(n => n.Id == c.Id);
            player.Update();
            player.InvokeOnLoad();
        }
        else
        {
            PopUp.MySelf.Show("Equip Slot Reached", "4 Slot it's the equip limit");
        }
    }
    public void ItemSell (Text t)
    {
        var c = player.character.fileContent.Inventory.Find(n => n.Name == t.text);
        player.character.fileContent.cash += c.price;
        player.character.fileContent.Inventory.RemoveAll(n => n.Id == c.Id);
        player.InvokeOnLoad();
    }
    public void BuyItem (Text t)
    {
        var c = player.character.fileContent.Shop.Find(n => n.Name == t.text);

        if(player.character.fileContent.cash - c.price < 0)
        {
            PopUp.MySelf.Show("Out of Cash", "You are living in Venezuela");
            return;
        }

        player.character.fileContent.cash -= c.price;
        player.character.fileContent.Inventory.ADD_NO_REPEAT(c, n => n.Name == c.Name);
        player.character.fileContent.Shop.RemoveAll(n => n.Name == c.Name);
        player.InvokeOnLoad();
    }
    public void GetShopDetails (Text t)
    {
        var c = player.character.fileContent.Shop.Find(n => n.Name == t.text);
        PopUp.MySelf.Show(t.text, c.StatsToPrice);
    }
    public void GetInventoryDetails (Text t)
    {
        var c = player.character.fileContent.Inventory.Find(n => n.Name == t.text);
        PopUp.MySelf.Show(t.text, c.StatsToPrice);
    }
    public void SetInputField (Text t)
    {
        GetComponent<InputField>().text = t.text;
    }
    public void SetText (Text t)
    {
        GetComponent<Text>().text = t.text;
    }
    public void PickObject ()
    {
        transform.parent = Mouse2D.MySelf.transform;
        transform.localPosition = new Vector3(0f, 0f, 0f);
    }
    public static List<GameObject> Historial = new List<GameObject>();

    public void SelectGameObject(GameObject o)
    {
        if(Historial.Count > 0)
        {
            if(Historial[0] == o)
            {
                return;
            }
        }

        if(Historial.Count == 0)
        {
            var c = o.transform.parent.FIND<Transform>
            (n => n.gameObject.activeInHierarchy);

            Historial.Insert(0, c.gameObject);
        }

        o.transform.parent.FOR_EACH<Transform>
        (n => n.gameObject.SetActive(false));

        o.SetActive(true);
        Historial.Insert(0, o);

        //_Historial = Historial;
    }
    public void Back ()
    {
        if(Historial.Count <= 1)
        {
            return;
        }
        Historial.RemoveAt(0);
        var o = Historial[0];

        o.transform.parent.FOR_EACH<Transform>
        (n => n.gameObject.SetActive(false));

        o.SetActive(true);

        //_Historial = Historial;
    }
}