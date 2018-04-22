using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Skills : MonoBehaviour{

    public static Skills instance;

    public List<Skill> skill_list_assassin;
    public List<Skill> skill_list_alpha;
    public List<Skill> skill_list_charming;
    public List<Skill> skill_list_pacifist;

    List<Skill> skill_list = new List<Skill>();

    void Awake()
    {
        instance = this;
        Inicialize();
    }


    private void Inicialize()
    {
        foreach (Skill s in skill_list_assassin) skill_list.Add(s);
        foreach (Skill s in skill_list_alpha)    skill_list.Add(s);
        foreach (Skill s in skill_list_charming) skill_list.Add(s);
        foreach (Skill s in skill_list_pacifist) skill_list.Add(s);
    }

    public Skill_Result SkillResolve(int ID_skill, Stats myStats, Stats enemyStats, int fails)
    {
        Skill_Result result = new Skill_Result();
        switch (ID_skill)
        {
            case  0: result = Skill_0 (myStats); break;

            case 11: result = Skill_11(myStats); break;
            case 40: result = Skill_40(myStats); break;
        }
        result.value -= (0.1f * result.value) * fails;
        return result;
    }

    Skill_Result Skill_0(Stats stats) //Tortazo: Daño igual al 30% de tu ataque actual.
    {
        return new Skill_Result()
        {
            s_type = Skill_Type.Attack,
            value = stats.damage_now * 0.3f
        };
    }

    Skill_Result Skill_11(Stats stats)
    {
        return new Skill_Result()
        {
            s_type = Skill_Type.Attack,
            value = stats.damage_now * 0.4f
        };
    }

    Skill_Result Skill_40(Stats stats) //Curación: Restaura un 20% de salud + 1% por punto de habilidad.
    {
        return new Skill_Result()
        {
            s_type = Skill_Type.Heal,
            value = (stats.health_base * 0.2f) + (stats.skill_now * stats.health_base / 100)
        };
    }

    public Skill SkillByID(int ID)
    {
        return skill_list.Find(sk => sk.ID == ID);
    }

    public Skill_Class SkillClassByID(int ID_skill)
    {
        return SkillByID(ID_skill).s_class;
    }

}

