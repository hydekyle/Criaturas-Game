﻿using Enums;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace Enums {
    public enum Equip_Position { Head, Body, Arms, Legs, Back };
    public enum Quality { Common, Rare, Epic, Legendary };
    public enum Stat { Damage, Health, Skill, Luck};
    public enum Class { Assassin, Pacifist, Charming, Alpha};
    public enum Skill_Type { Attack, Spell, Buff };
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

public class Visor
{
    public Image headgear;
    public Image body;
    public Image back;
    public Image arm_left;
    public Image arm_right;
    public Image leg_left;
    public Image leg_right;
    public Transform myTransform;
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
public class MySkylls
{
    public List<int> head = new List<int>();
    public List<int> body = new List<int>();
    public List<int> arms = new List<int>();
    public List<int> legs = new List<int>();
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
    public List<int> skills_ID;

}

[SerializeField]
public class Criatura
{
    public string nombre;
    public Equipment equipment;
}

[SerializeField]
public class Player
{
    public string ID;
    public string nombre;
    public Criatura criatura;
}

public class Skill
{
    int ID;
    string name;
    string description;
    Class clase;
    Skill_Type s_type;
}



