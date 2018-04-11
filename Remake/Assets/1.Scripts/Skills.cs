using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Skills : MonoBehaviour{

    public Skill_Result SkillResolve(int ID_skill, Stats myStats, Stats enemyStats)
    {
        Skill_Result result = new Skill_Result();
        switch (ID_skill)
        {
            case 0: result = Skill_0(myStats); break;

            case 40: result = Skill_40(myStats); break;
        }
        print("Habilidad " + Lenguaje.Instance.SkillNameByID(ID_skill) + " lanzada.");
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

    Skill_Result Skill_1(Stats stats)
    {
        return new Skill_Result()
        {

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

}

