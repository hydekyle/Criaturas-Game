using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Skills : MonoBehaviour{

    public Skill_Result SkillResolve(int ID_skill, Stats myStats, Stats enemyStats, int fails)
    {
        Skill_Result result = new Skill_Result();
        switch (ID_skill)
        {
            case  0: result = Skill_0(myStats); break;

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

    public Skill_Class SkillClassByID(int ID_skill)
    {
        if (ID_skill < 10) return Skill_Class.Assassin;
        else if (ID_skill < 20) return Skill_Class.Alpha;
        else if (ID_skill < 30) return Skill_Class.Charming;
        else if (ID_skill < 40) return Skill_Class.Pacifist;

        return Skill_Class.Assassin;

    }


}

