using Enums;
using UnityEngine;

namespace Enums {
    public enum Equip_Position { Head, Body, Arms, Weapon, Legs, Back };
    public enum Quality { Common, Rare, Epic, Legendary };
    public enum Stat { Damage, Health, Skill, Luck};
}

[System.Serializable]
public class GiveStat {
    public Stat stat_type;
    public int value;
}

public class Equipment
{
    public Equipable_Item head;
    public Equipable_Item body;
    public Equipable_Item arms;
    public Equipable_Item weapon;
    public Equipable_Item legs;
    public Equipable_Item back;
}

[System.Serializable]
public class Equipable_Item
{
    public Sprite sprite;
    public int ID;
    public string nombre;
    public Equip_Position position;
    public Quality quality;
    public GiveStat[] addStat;

}

public class Criatura
{
    public string nombre;
    public int nivel;
    public Equipment equipment;
}

public class Player
{
    public string ID;
    public int gold;
    public int gold_VIP;
}

