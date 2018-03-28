using Enums;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace Enums {
    public enum Equip_Position { Head, Body, Arms, Legs, Back };
    public enum Quality { Common, Rare, Epic, Legendary };
    public enum Stat { Damage, Health, Skill, Luck};
    public enum Class { Assassin, Pacifist, Charming, Alpha};
    public enum Skill_Type { Punch, Spell };
}

public struct BodyBounds
{
    public Vector3 head_POS;
    public Vector3 leg_right_POS;
    public Vector3 leg_left_POS;
    public Vector3 arm_right_POS;
    public Vector3 arm_left_POS;
    public Vector3 back_POS;
}


[Serializable]
public struct GiveStat {
    public Stat stat_type;
    public int value;
}

[SerializeField]
public struct Equipment
{
    public Equipable_Item head;
    public Equipable_Item body;
    public Equipable_Item arms;
    public Equipable_Item legs;
    public Equipable_Item back;
}

[Serializable]
public struct Equipable_Item
{
    //public Sprite sprite;
    public int ID;
    public string nombre;
    //public Equip_Position position;
    public Quality quality;
    public List<GiveStat> addStat;

}

public class CombatAction
{

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

