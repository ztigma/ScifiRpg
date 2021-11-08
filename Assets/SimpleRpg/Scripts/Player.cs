using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class Player : MonoBehaviour
{
    public DamageBody damageBody;
    public Text CashView;
    public Text DayView;
    public Text TrysView;
    public Text DeathsView;
    public ListView InventoryView;
    public ListView EquipView;
    public ListView TrainingView;
    public ListView HomeView;
    public ListView ShopView;
    public PermaCharacter character;
    public UnityEvent OnLoad; public void InvokeOnLoad() { OnLoad.Invoke(); }
    public UnityEvent OnSave; public void InvokeOnSave() { OnSave.Invoke(); }
    public static int ItemCount;
    public PersistenceVariable persistenceVariable;
    public static Player MySelf;

    public Player ()
    {
        "ScifiRpg".START_SPECIAL_FOLDER();
        
        MySelf = this;
    }
    void Start ()//LOAD
    {
        persistenceVariable = new PersistenceVariable("ItemCount");
        ItemCount = persistenceVariable.variable;

        character = new PermaCharacter(character.FileName);
        character.fileContent.NewShopItems();

        if(character.isFirsLoad)
        {
            character.fileContent.Update();
            character.fileContent.StatsFinal.ForEach(n => n.Min = n.Max);
        }

        OnLoad.Invoke();
        character.fileContent.GetOrigin(ref damageBody.character);
    }
    void OnDestroy ()//SAVE
    {
        OnSave.Invoke();

        character.Save();
        persistenceVariable.variable = ItemCount;
    }
    public void Update ()
    {
        character.fileContent.Update();
        CashView.text = "Cash: " + character.fileContent.cash;
        DayView.text = "Day: " + character.fileContent.day;
        TrysView.text = "Tryies: " + character.fileContent.intentos;
        DeathsView.text = "Deaths: " + character.fileContent.muertes;
    }

    //INVOKES

    public void InventoryToListView ()
    {
        InventoryView.ItemSource = character.fileContent.Inventory.ConvertAll
        (n => new SourceModel() { id = n.Id + "", model = n});
    }
    public void EquipToEquipView ()
    {
        EquipView.ItemSource = character.fileContent.Equiped.ConvertAll
        (n => new SourceModel() { id = n.Id + "", model = n});
    }
    public void StatsBaseToTrainingView ()
    {
        TrainingView.ItemSource = character.fileContent.StatsBase.ConvertAll
        (n => new SourceModel() { id = n.Name + "", model = n});
    }
    public void StatsFinalToHomeView ()
    {
        HomeView.ItemSource = character.fileContent.StatsFinal.ConvertAll
        (n => new SourceModel() { id = n.Name + "", model = n});
    }
    public void ShopToShopView ()
    {
        ShopView.ItemSource = character.fileContent.Shop.ConvertAll
        (n => new SourceModel() { id = n.Id + "", model = n});
    }
}
[System.Serializable]
public class PermaCharacter : PermaObject<Character>
{
    public PermaCharacter(string File_Name)
    {
        FileName = File_Name;
        _fileContent = variable;
    }
}
[System.Serializable]
public class Character
{
    public int lvl = 1; private string Online_lvl { set { lvl = value.TO_VARIABLE();}}
    public int cash = 3; private string Online_cash { set { cash = value.TO_VARIABLE();}}
    public int day; private string Online_day { set { day = value.TO_VARIABLE();}}
    public int intentos = 0; private string Online_intentos { set { intentos = value.TO_VARIABLE();}}
    public int muertes = 0; private string Online_muertes { set { muertes = value.TO_VARIABLE();}}
    public List<Item> Inventory = new List<Item>(); private string Online_Inventory { set { value.TO_OBJECT(ref Inventory);}}
    public List<Item> Equiped = new List<Item>(); private string Online_Equiped { set { value.TO_OBJECT(ref Equiped);}}
    public List<Item> Shop = new List<Item>(); private string Online_Shop { set { value.TO_OBJECT(ref Shop);}}
    public int ShopItems = 12; private string Online_ShopItems { set { ShopItems = value.TO_VARIABLE();}}
    public bool isNewItemsTime = true; private string Online_isNewItemsTime { set { isNewItemsTime = value.TO_VARIABLE();}}
    public List<SingleStats> StatsBase = new List<SingleStats>(); private string Online_StatsBase { set { value.TO_OBJECT(ref StatsBase);}}
    public List<SingleStats> StatsFinal = new List<SingleStats>(); private string Online_StatsFinal { set { value.TO_OBJECT(ref StatsFinal);}}

    public Character ()
    {
        StatsBase.StartStats(0, 1);
        StatsFinal.StartStats();
    }
    public void NewShopItems ()
    {
        if(isNewItemsTime)
        {
            Shop.Clear();
            for(int i = 0; i < ShopItems; i++)
            {
                Shop.RandomItem(lvl);
            }
            isNewItemsTime = false;
        }
    }
    public void Update ()
    {
        StatsFinal.ToZero();
        for(int i = 0; i < StatsFinal.Count; i++)
        {
            StatsFinal[i] += StatsBase[i];
        }

        foreach(var c in Equiped)
        {
            if(c == null){ continue; }
            for(int i = 0; i < StatsFinal.Count; i++)
            {
                StatsFinal[i] += c.Stats[i];
            }   
        }
    }
    public void Sleep ()
    {
        if(cash - 1 < 0)
        {
            PopUp.MySelf.Show("Out of Cash", "You are living in Venezuela");
        }
        else
        {
            StatsFinal.ForEach(n => n.Min = n.Max);
            cash -= 1;
            day += 1;
        }
    }
}
[System.Serializable]
public class Item
{
    public string Name;
    public int lvl;
    public int Id;
    public int price;
    public List<SingleStats> Stats;
    public string StatsToFormat
    {
        get
        {
            string r = "lvl: " + lvl + "\n";
            Stats.ForEach(n => r += n.Name + " = " + n.Max + "\n");
            return r;
        }
    }
    public string StatsToPrice
    {
        get
        {
            string r = "lvl: " + lvl + "\n";
            Stats.ForEach(n => r += n.Name + " = " + n.Max + "\n");
            r += "\n" + "Precio: " + price;
            return r;
        }
    }
}
[System.Serializable]
public class SingleStats
{
    public string Name;
    [SerializeField] private int _Min;
    [SerializeField] private int _Max;
    public int Min
    {
        get
        {
            if(_Min < _Max)
            {
                return _Min;
            }
            else
            {
                return _Max;
            }
        }
        set
        {
            _Min = value;
        }
    }
    public int Max
    {
        get
        {
            return _Max;
        }
        set
        {
            _Max = value;
        }
    }
    public static SingleStats operator +(SingleStats a, SingleStats b)
    {
        a.Max += b.Max;
        return a;
    }
    public static SingleStats operator +(SingleStats a, int b)
    {
        a.Min += b;
        return a;
    }
    public static SingleStats operator -(SingleStats a, int b)
    {
        a.Min -= b;
        return a;
    }
    public void RandomSingleStats(int lvl)
    {
        Max = (-lvl).RandomMinMax(lvl);
    }
}
public static class SingleStatsMethods
{
    public const string hp = "Vida";
    public const string rh = "Regen de vida";

    public const string mp = "Mana";
    public const string rm = "Regen de mana";
    
    public const string at = "Ataque";
    public const string df = "Defensa";
    
    public const string pe = "Precision";
    public const string ev = "Evacion";
    
    public const string ve = "Velocidad";
    public const string ro = "Rotacion";

    public const string ats = "Ataque Speed";
    public const string vb = "Velocidad Bala";

    public static SingleStats HP (this List<SingleStats> o, Action<SingleStats> a = null)
    {
        if(a != null)
        {
            a.Invoke(o.Find(n => n.Name == hp));
        }
        return o.Find(n => n.Name == hp);
    }
    public static SingleStats RH (this List<SingleStats> o,  Action<SingleStats> a = null)
    {
        if(a != null)
        {
            a.Invoke(o.Find(n => n.Name == hp));
        }
        return o.Find(n => n.Name == rh);
    }
    public static SingleStats MP (this List<SingleStats> o, Action<SingleStats> a = null)
    {
        if(a != null)
        {
            a.Invoke(o.Find(n => n.Name == hp));
        }
        return o.Find(n => n.Name == mp);
    }
    public static SingleStats RM (this List<SingleStats> o, Action<SingleStats> a = null)
    {
        if(a != null)
        {
            a.Invoke(o.Find(n => n.Name == hp));
        }
        return o.Find(n => n.Name == rm);
    }
    public static SingleStats AT (this List<SingleStats> o, Action<SingleStats> a = null)
    {
        if(a != null)
        {
            a.Invoke(o.Find(n => n.Name == hp));
        }
        return o.Find(n => n.Name == at);
    }
    public static SingleStats DF (this List<SingleStats> o, Action<SingleStats> a = null)
    {
        if(a != null)
        {
            a.Invoke(o.Find(n => n.Name == hp));
        }
        return o.Find(n => n.Name == df);
    }
    public static SingleStats PE (this List<SingleStats> o, Action<SingleStats> a = null)
    {
        if(a != null)
        {
            a.Invoke(o.Find(n => n.Name == hp));
        }
        return o.Find(n => n.Name == pe);
    }
    public static SingleStats EV (this List<SingleStats> o, Action<SingleStats> a = null)
    {
        if(a != null)
        {
            a.Invoke(o.Find(n => n.Name == hp));
        }
        return o.Find(n => n.Name == ev);
    }
    public static SingleStats VE (this List<SingleStats> o, Action<SingleStats> a = null)
    {
        if(a != null)
        {
            a.Invoke(o.Find(n => n.Name == hp));
        }
        return o.Find(n => n.Name == ve);
    }
    public static SingleStats RO (this List<SingleStats> o, Action<SingleStats> a = null)
    {
        if(a != null)
        {
            a.Invoke(o.Find(n => n.Name == hp));
        }
        return o.Find(n => n.Name == ro);
    }
    public static SingleStats ATS (this List<SingleStats> o, Action<SingleStats> a = null)
    {
        if(a != null)
        {
            a.Invoke(o.Find(n => n.Name == hp));
        }
        return o.Find(n => n.Name == ats);
    }
    public static SingleStats VB (this List<SingleStats> o, Action<SingleStats> a = null)
    {
        if(a != null)
        {
            a.Invoke(o.Find(n => n.Name == hp));
        }
        return o.Find(n => n.Name == vb);
    }
    public static List<SingleStats> StartStats(this List<SingleStats> o, int min = 0, int max = 0)
    {
        o.ADD_NO_REPEAT(new SingleStats() { Name = hp, Min = min, Max = max}, n => n.Name == hp);
        o.ADD_NO_REPEAT(new SingleStats() { Name = rh, Min = min, Max = max}, n => n.Name == rh);

        o.ADD_NO_REPEAT(new SingleStats() { Name = mp, Min = min, Max = max}, n => n.Name == mp);
        o.ADD_NO_REPEAT(new SingleStats() { Name = rm, Min = min, Max = max}, n => n.Name == rm);

        o.ADD_NO_REPEAT(new SingleStats() { Name = at, Min = min, Max = max}, n => n.Name == at);
        o.ADD_NO_REPEAT(new SingleStats() { Name = df, Min = min, Max = max}, n => n.Name == df);

        o.ADD_NO_REPEAT(new SingleStats() { Name = pe, Min = min, Max = max}, n => n.Name == pe);
        o.ADD_NO_REPEAT(new SingleStats() { Name = ev, Min = min, Max = max}, n => n.Name == ev);

        o.ADD_NO_REPEAT(new SingleStats() { Name = ve, Min = min, Max = max}, n => n.Name == ve);
        
        o.ADD_NO_REPEAT(new SingleStats() { Name = ro, Min = min, Max = max}, n => n.Name == ro);

        o.ADD_NO_REPEAT(new SingleStats() { Name = ats, Min = min, Max = max}, n => n.Name == ats);
        o.ADD_NO_REPEAT(new SingleStats() { Name = vb, Min = min, Max = max}, n => n.Name == vb);

        return o;
    }
    public static List<SingleStats> ToZero(this List<SingleStats> o)
    {
        o.ForEach(n => n.Max = 0);
        return o;
    }
    public static List<SingleStats> RandomStats(this List<SingleStats> o, int lvl)
    {
        o.ForEach(n => n.RandomSingleStats(lvl));
        return o;
    }
    public static Item RandomItem(this List<Item> o, int lvl)
    {
        var i = Player.ItemCount;
        var c = (0).RandomCount(ItemNamesList.Count);
        var item = new Item()
        { 
            lvl = lvl
            ,
            price = lvl
            ,
            Id = i
            ,
            Name = ItemNamesList[c] + " (" + i + ")"
            ,
            Stats = new List<SingleStats>().StartStats().RandomStats(lvl)
        };
        o.Add(item);
        Player.ItemCount++;
        return item;
    }
    public static List<string> ItemNamesList
    {
        get
        {
            return ItemNames.Splitting(new string[] { ":" });
        }
    }

    public const string ItemNames = 
    "TurbinaXL:Casco Reforzado:Potenciadores:Acelerometros:Rotores:Motores:"
    +
    "Acelerador de particulas:Cañon de Riel:Escudo Electromagnetico:Armadura Viva:Radar:Cpu:"
    +
    "Automata Asistente:Algoritmos de Combate:Turbinas Aisladas:Casco Aerodinamico:Rocio de Metralla:Laser de Calor:"
    +
    "Casco Ultra Denso:Turbina Cuantica:Alas Plegables:Alerones Dinamicos:Cpu Aerodinamico:Turbina Electromagnetica:"
    +
    "Cpu Vivo:Alas Vivas:Generador Electrico:Rotor Cuantico:Nano Nucleo:"
    +
    "Corazon Nuclear:Propulsion Quad:Omni Cpu:"
    +
    "Patrones de Combate:Programa Amigable:Armadura Reforzada:Armadura Ultra Ligera:"
    +
    "Ondas De Fase:Radares Gravitacionales:Radar De Micro-Ondas:Detector de Vida:"
    +
    "Camara de Alimentacion:Camara de Oxigeno:Fuente De Poder:Potenciador de Onda:"
    +
    "Lasers en Fase:Control Gravitacional:Turbina G:"
    +
    "Alimentadores Solares:Algoritmos de Hipermovilidad:Turbina De Luz:"
    +
    "Casco De Energia:Turbina de Antimateria:Lanzadora Nuclear:Lanzadora Electrica:Rieles Hipercargados:"
    +
    "Consensadores de Antimateria:Nucleo Atomico:"
    ;
}