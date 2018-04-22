using Enums;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace Enums {
    public enum Equip_Position { Head, Body, Arms, Legs, Back, None };
    public enum Quality { Common, Rare, Epic, Legendary };
    public enum Stat { Strenght, Health, Skill, Luck};
    public enum Skill_Class { Assassin, Pacifist, Charming, Alpha};
    public enum Skill_Type { Attack, Spell, Buff, Heal };
    public enum Speed { Slow, Normal, Fast};
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

public class FixedScale
{
    public Vector3 customScale;
    public Vector3 customPosition;
    public Vector3 customPosition_right;
    public Vector3 customRotation_right = Vector3.zero;
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
public class SkillsButtons
{
    public SkillButton head_button;
    public SkillButton body_button;
    public SkillButton arms_button;
    public SkillButton legs_button;
    public int head_activable_skill_ID;
    public int body_activable_skill_ID;
    public int arms_activable_skill_ID;
    public int legs_activable_skill_ID;
}

[Serializable]
public class SkillButton
{
    public Image myImage;
    public Text myText;
}

[Serializable]
public struct Equipable_Item
{
    public int ID;
    public string nombre;
    public Quality quality;
    public List<GiveStat> addStat;
    public List<int> skills_ID;
}

[SerializeField]
public class Criatura
{
    public string nombre;
    public Equipment equipment;
    public MySkylls skills;
}

[SerializeField]
public class Player
{
    public string ID;
    public string nombre;
    public Criatura criatura;
}

[Serializable]
public class Skill
{
    public int ID;
    public string name_spanish;
    public string name_english;
    public string description_spanish;
    public string description_english;
    public Skill_Class s_class;
    public Skill_Type s_type;
}

public class Skill_Result : Skill
{
    public float value;
}

public class DataTurn
{
    byte turn_number;
    int used_skill;
    int power_skill;
    Skill_Type s_type;
}

public class Stats
{
    public float damage_base;
    public float health_base;
    public float skill_base;
    public float luck_base;

    public float damage_now;
    public float health_now;
    public float skill_now;
    public float luck_now;

    public List<int> buffs_ID;
    public List<int> debuffs_ID;
}

public struct Osu_Ball
{
    
}



