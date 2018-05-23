using Enums;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace Enums {
    public enum Equip_Position { Head, Body, Arms, Legs, Back, None };
    public enum Quality { Common, Rare, Epic, Legendary };
    public enum Stat { Strenght, Health, Dextery, Luck};
    public enum Skill_Class { Assassin, Pacifist, Charming, Alpha};
    public enum Skill_Type { Attack, Spell, Buff, Heal };
    public enum Speed { Slow, Normal, Fast};
    public enum Buffs { Attack, Dextery, Luck, Shield, Barrier}
    public enum Debuffs { Attack, Dextery, Bleed, Poison, Dizziness, Confusion};
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
    public string ID_string;
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
    public int attack_att;
    public int defense_att;
    public int skill_att;
    public int luck_att;
}

[SerializeField]
public class Player
{
    public string ID;
    public string nombre;
    public Criatura criatura;
    public Stats status;
}

[Serializable]
public class Skill
{
    public int ID;
    public string ID_string;
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
    public int bleed;
    public int poison;
    public int dizziness;
    public int confusion;
    public int myself_bleed;
    public int myself_damage;
    public int buff_attack;
    public int buff_skill;
    public int buff_luck;
    public int buff_shield;
    public int buff_barrier;
    public float recoverHP;
    public int cleans;
}

public class DataTurn
{
    public byte turn_number;
    public int used_skill;
    public int power_skill;
    public byte minigame_fails;
}

public class Stats
{
    public int damage_base;
    public int health_base;
    public int skill_base;
    public int luck_base;

    public int damage_now;
    public int health_now;
    public int skill_now;
    public int luck_now;

    public int bleed;
    public int dizziness;
    public int confusion;
    public int poison;

    public int shield;
    public int barrier;

    public int buff_attack;
    public int buff_skill;
    public int buff_luck;
    public int buff_shield;
    public int buff_barrier;

}




