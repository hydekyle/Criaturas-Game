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

    public Skill GetSkillByID(string id)
    {
        Skill skillBuscada = new Skill();
        int lista = int.Parse(id.Substring(0, 1));
        int n = int.Parse(id.Substring(1, 2));
        switch (lista)
        {
            case 1: skillBuscada = skill_list_assassin[n - 1]; break;
            case 2: skillBuscada = skill_list_alpha[n - 1]; break;
            case 3: skillBuscada = skill_list_charming[n - 1]; break;
            case 4: skillBuscada = skill_list_pacifist[n - 1]; break;
        }
        return skillBuscada;
    }

    public Skill_Result SkillResolve(int ID_skill, Stats myStats, Stats enemyStats, int fails)
    {
        Skill_Result result = new Skill_Result();
        switch (ID_skill)
        {
            case 101: result = Skill_101(myStats, enemyStats, fails); break;
            case 102: result = Skill_102(myStats, enemyStats, fails); break;
            case 103: result = Skill_103(myStats, enemyStats, fails); break;
            case 104: result = Skill_104(myStats, enemyStats, fails); break;
            case 105: result = Skill_105(myStats, enemyStats, fails); break;
            case 106: result = Skill_106(myStats, enemyStats, fails); break;
            case 107: result = Skill_107(myStats, enemyStats, fails); break;
            case 108: result = Skill_108(myStats, enemyStats, fails); break;
            case 109: result = Skill_109(myStats, enemyStats, fails); break;
            case 110: result = Skill_110(myStats, enemyStats, fails); break;

            case 201: result = Skill_201(myStats, enemyStats, fails); break;
            case 202: result = Skill_202(myStats, enemyStats, fails); break;
            case 203: result = Skill_203(myStats, enemyStats, fails); break;
            case 204: result = Skill_204(myStats, enemyStats, fails); break;
            case 205: result = Skill_205(myStats, enemyStats, fails); break;
            case 206: result = Skill_206(myStats, enemyStats, fails); break;
            case 207: result = Skill_207(myStats, enemyStats, fails); break;
            case 208: result = Skill_208(myStats, enemyStats, fails); break;
            case 209: result = Skill_209(myStats, enemyStats, fails); break;
            case 210: result = Skill_210(myStats, enemyStats, fails); break;

            case 301: result = Skill_301(myStats, enemyStats, fails); break;
            case 302: result = Skill_302(myStats, enemyStats, fails); break;
            case 303: result = Skill_303(myStats, enemyStats, fails); break;
            case 304: result = Skill_304(myStats, enemyStats, fails); break;
            case 305: result = Skill_305(myStats, enemyStats, fails); break;
            case 306: result = Skill_306(myStats, enemyStats, fails); break;
            case 307: result = Skill_307(myStats, enemyStats, fails); break;
            case 308: result = Skill_308(myStats, enemyStats, fails); break;
            case 309: result = Skill_309(myStats, enemyStats, fails); break;
            case 310: result = Skill_310(myStats, enemyStats, fails); break;

            case 401: result = Skill_401(myStats, enemyStats, fails); break;
            case 402: result = Skill_402(myStats, enemyStats, fails); break;
            case 403: result = Skill_403(myStats, enemyStats, fails); break;
            case 404: result = Skill_404(myStats, enemyStats, fails); break;
            case 405: result = Skill_405(myStats, enemyStats, fails); break;
            case 406: result = Skill_406(myStats, enemyStats, fails); break;
            case 407: result = Skill_407(myStats, enemyStats, fails); break;
            case 408: result = Skill_408(myStats, enemyStats, fails); break;
            case 409: result = Skill_409(myStats, enemyStats, fails); break;
            case 410: result = Skill_410(myStats, enemyStats, fails); break;

        }
        result.value -= (0.1f * result.value) * fails;
        return result;
    }


    Skill_Result Skill_101(Stats stats, Stats enemyStats, int fails) //Acuchillar
    {
        int newBleed = 2 - fails;
        float dmg = 100 * (stats.damage_now * 0.1f) - (fails * 5);
        return new Skill_Result()
        {
            bleed = Mathf.Clamp(newBleed, 0, 2),
            value = Mathf.Clamp(dmg, 10, 200)
        };
    }

    Skill_Result Skill_102(Stats stats, Stats enemyStats, int fails) //Chupasangre
    {
        float vidaRobada = (enemyStats.bleed * 10 + stats.skill_now * 5) - (fails * 2) - (stats.poison * 5);
        float dmg = 70 * (stats.damage_now * 0.1f) - (fails * 5);
        return new Skill_Result()
        {
            recoverHP = Mathf.Clamp(vidaRobada, 10, 200),
            value = Mathf.Clamp(dmg, 10, 200)
        };
    }

    Skill_Result Skill_103(Stats stats, Stats enemyStats, int fails) //Hemorragia
    {
        int sangrado = stats.skill_now;
        int newBleed = Mathf.Clamp(sangrado, 3, 6) - fails;
        return new Skill_Result()
        {
            bleed = newBleed
        };
    }

    Skill_Result Skill_104(Stats stats, Stats enemyStats, int fails) //Apuñalar
    {
        float dmg = 160 * (stats.damage_now * 0.2f) - 15 * fails;
        int newBleed = Random.Range(stats.luck_now, 14) > 12 ? 3 : 0;  
        return new Skill_Result() {
            bleed = newBleed,
            value = Mathf.Clamp(dmg, 35, 300)
        };
    }

    Skill_Result Skill_105(Stats stats, Stats enemyStats, int fails) // Desangrar
    {
        float dmg = stats.skill_now * 10 + (stats.skill_now * enemyStats.bleed);
        return new Skill_Result()
        {
            value = Mathf.Clamp(dmg, 10, 250)
        };
    }

    Skill_Result Skill_106(Stats stats, Stats enemyStats, int fails) //Rematar
    {
        float dmg = enemyStats.health_now < enemyStats.health_base * 0.5f ? (stats.skill_now * 3) - (fails * 8) : (stats.skill_now * 20) - (fails * 7);
        return new Skill_Result()
        {
            value = Mathf.Clamp(dmg, 10, 300)
        };
    }

    Skill_Result Skill_107(Stats stats, Stats enemyStats, int fails) //Venganza
    {
        float dmg = ((100 - (stats.health_now * 100 / stats.health_base)) * (stats.damage_now * 0.2f)) - fails * 10;
        return new Skill_Result()
        {
            value = Mathf.Clamp(dmg, 20, 400)
        };
    }

    Skill_Result Skill_108(Stats stats, Stats enemyStats, int fails) //Doble filo
    {
        float dmg = 250 * (stats.damage_now * 0.15f);
        int autoBleed = Random.Range(0, stats.luck_now + 1) <= 1 ? 2 : 1;
        return new Skill_Result()
        {
            myself_bleed = autoBleed,
            value = Mathf.Clamp(dmg, 30, 350)
        };
    }

    Skill_Result Skill_109(Stats stats, Stats enemyStats, int fails) //Envenenar
    {
        int veneno = Random.Range(stats.skill_now, 13) >= 10 ? 4 : 2;
        return new Skill_Result()
        {
            poison = veneno
        };
    }

    Skill_Result Skill_110(Stats stats, Stats enemyStats, int fails) //Intoxicar
    {
        int veneno = Random.Range(stats.skill_now, 13) >= 10 ? 2 : 1;
        float dmg = 80 * (stats.skill_now * 0.1f);
        return new Skill_Result()
        {
            poison = veneno,
            value = Mathf.Clamp(dmg, 20, 180)
        };
    }

    Skill_Result Skill_201(Stats stats, Stats enemyStats, int fails) //Tortazo
    {
        float dmg = 80 * (stats.damage_now * 0.15f);
        int mareo = Random.Range(stats.luck_now, 13) >= 8 ? 2 : 1;
        return new Skill_Result()
        {
            dizziness = mareo,
            value = Mathf.Clamp(dmg, 20, 180)
        };
    }

    Skill_Result Skill_202(Stats stats, Stats enemyStats, int fails) //Puñetazo
    {
        float dmg = 150 * (stats.damage_now * 0.2f);
        return new Skill_Result()
        {
            value = Mathf.Clamp(dmg, 30, 280)
        };
    }

    Skill_Result Skill_203(Stats stats, Stats enemyStats, int fails) //Cabezazo
    {
        float dmg = 180 * (stats.damage_now * 0.25f);
        float dmg_myself = dmg * 0.2f;

        return new Skill_Result()
        {
            myself_damage = (int)dmg_myself,
            value = Mathf.Clamp(dmg, 40, 400)
        };
    }

    Skill_Result Skill_204(Stats stats, Stats enemyStats, int fails) //Apalizar
    {
        float dmg = BattleSystem.instance.lastSkill_ID == 204 ? (stats.damage_now * 0.2f) * 100 : (stats.damage_now * 0.2f) * 230;
        return new Skill_Result()
        {
            value = Mathf.Clamp(dmg, 20, 400)
        };
    }

    Skill_Result Skill_205(Stats stats, Stats enemyStats, int fails) //Sacudir
    {
        float dmg = (stats.damage_now * 0.15f) * 50 + (stats.skill_now * enemyStats.dizziness * 3);
        return new Skill_Result()
        {
            value = Mathf.Clamp(dmg, 20, 400)
        };
    }

    Skill_Result Skill_206(Stats stats, Stats enemyStats, int fails) //Marear
    {
        int mareo = Random.Range(stats.luck_now, 14) >= 10 ? 4 : 2;
        int newConfusion = Random.Range(stats.luck_now, 14) >= 6 ? Random.Range(1, 3) : 0; 
        return new Skill_Result()
        {
            dizziness = mareo,
            confusion = newConfusion
        };
    }

    Skill_Result Skill_207(Stats stats, Stats enemyStats, int fails) //Férreo
    {
        int newAttack, newSkill;
        if(fails == 0)
        {
            newAttack = Random.Range(stats.luck_now, 14) >= 7 ? 4 : 3;
            newSkill = Random.Range(stats.luck_now, 14) >= 7 ? 4 : 3;
        }else
        {
            newAttack = Random.Range(stats.luck_now, 14) >= 7 ? 3 : 1;
            newSkill = Random.Range(stats.luck_now, 14) >= 7 ? 3 : 1;
        }
        return new Skill_Result()
        {
            buff_attack = newAttack,
            buff_skill = newSkill
        };
    }

    Skill_Result Skill_208(Stats stats, Stats enemyStats, int fails) //Desorientar
    {
        float dmg = (stats.damage_now * 0.2f) * 150 - (fails * 12);
        int mareo = 0;
        if(Random.Range(stats.luck_now, 14) >= 5)
        {
            mareo = Random.Range(stats.luck_now, 14) >= 7 ? 3 : 2;
        }
        return new Skill_Result()
        {
            dizziness = mareo,
            value = Mathf.Clamp(dmg, 30, 320)
        };
    }
   
    Skill_Result Skill_209(Stats stats, Stats enemyStats, int fails) //Devastar
    {
        float dmg = (stats.damage_now * 0.15f) * 150 + (enemyStats.barrier + enemyStats.shield) * 10 - (fails * 7);
        return new Skill_Result()
        {
            value = Mathf.Clamp(dmg, 30, 500)
        };
    }

    Skill_Result Skill_210(Stats stats, Stats enemyStats, int fails) //Golpe Escudo
    {
        float dmg = (stats.damage_now * 0.15f) * 120 - (fails * 8);
        int escudo = Random.Range(stats.luck_now, 14) >= 6 ? 3 : 1;
        return new Skill_Result()
        {
            buff_shield = escudo,
            value = Mathf.Clamp(dmg, 20, 250)
        };
    }

    Skill_Result Skill_301(Stats stats, Stats enemyStats, int fails) //Empatía
    {
        int sangrado, veneno, mareo, newConfusion;
        if (fails <= 2)
        {
            sangrado = stats.bleed;
            veneno = stats.poison;
            mareo = stats.dizziness;
            newConfusion = stats.confusion;
        }else
        {
            sangrado = 0;
            veneno = 0;
            mareo = 0;
            newConfusion = 0;
        }

        return new Skill_Result()
        {
            bleed = sangrado,
            poison = veneno,
            dizziness = mareo,
            confusion = newConfusion
        };
    }

    Skill_Result Skill_302(Stats stats, Stats enemyStats, int fails) //Manipulación
    {
        Player enemy = BattleSystem.instance.YourEnemy();
        List<int> enemySkills = new List<int>();
        foreach (int sID in enemy.criatura.skills.head) enemySkills.Add(sID);
        foreach (int sID in enemy.criatura.skills.body) enemySkills.Add(sID);
        foreach (int sID in enemy.criatura.skills.arms) enemySkills.Add(sID);
        foreach (int sID in enemy.criatura.skills.legs) enemySkills.Add(sID);

        int randomSkill = enemySkills[Random.Range(1, enemySkills.Count - 1)];
        return SkillResolve(randomSkill, stats, enemyStats, fails);
    }

    Skill_Result Skill_303(Stats stats, Stats enemyStats, int fails) //Imitación
    {
        return SkillResolve(BattleSystem.instance.lastSkillOponent_ID, stats, enemyStats, fails);
    }

    Skill_Result Skill_304(Stats stats, Stats enemyStats, int fails) //Ajustar cuentas
    {
        if(stats.health_now > enemyStats.health_now)
        {
            float dmg = (stats.skill_now * 0.2f) * 120 - fails * 10;
            return new Skill_Result()
            {
                value = Mathf.Clamp(dmg, 20, 300)
            };
        }else
        {
            float heal = (stats.skill_now * 10) + 50 - fails * 10 - stats.poison * 10;
            return new Skill_Result()
            {
                recoverHP = Mathf.Clamp(heal, 20, 150)
            };
        }
    }

    Skill_Result Skill_305(Stats stats, Stats enemyStats, int fails) //Recuperación
    {
        float heal = ((100 - (stats.health_now * 100 / stats.health_base)) * 4) - fails * 10 - stats.poison * 10;
        return new Skill_Result()
        {
            recoverHP = Mathf.Clamp(heal, 20, 250)
        };
    }

    Skill_Result Skill_306(Stats stats, Stats enemyStats, int fails) //Rencor
    {
        float dmg = ((100 - (stats.health_now * 100 / stats.health_base)) * (stats.skill_now * 0.2f)) - fails * 10;
        return new Skill_Result()
        {
            value = dmg
        };
    }

    Skill_Result Skill_307(Stats stats, Stats enemyStats, int fails) //Traición
    {
        float dmg = (stats.damage_now * 0.25f) * 120 - fails * 10;
        int newConfusion = 0;
        if (Random.Range(stats.luck_now, 14) >= 8)
        {
            newConfusion = Random.Range(stats.luck_now, 14) >= 7 ? 3 : 2;
        }
        return new Skill_Result()
        {
            dizziness = newConfusion,
            value = Mathf.Clamp(dmg, 20, 200)
        };
    }

    Skill_Result Skill_308(Stats stats, Stats enemyStats, int fails) //Maltrato
    {
        float dmg = stats.damage_now * 0.2f * 100 - fails * 15;
        int newAttack = Random.Range(stats.luck_now, 14) >= 9 ? 2 : 1;
        return new Skill_Result()
        {
            buff_attack = newAttack,
            value = Mathf.Clamp(dmg, 20, 350)
        };
    }

    Skill_Result Skill_309(Stats stats, Stats enemyStats, int fails) //Desprecio
    {
        float dmg = stats.damage_now * 0.2f * 100 - fails * 15;
        int newSkill = Random.Range(stats.luck_now, 14) >= 9 ? 2 : 1;
        return new Skill_Result()
        {
            buff_skill = newSkill,
            value = Mathf.Clamp(dmg, 20, 250)
        };
    }

    Skill_Result Skill_310(Stats stats, Stats enemyStats, int fails) //Bribón
    {
        float dmg = stats.damage_now * 0.2f * 100 - fails * 15;
        int newLuck = Random.Range(stats.luck_now, 14) >= 7 ? 2 : 1;
        return new Skill_Result()
        {
            buff_luck = newLuck,
            value = Mathf.Clamp(dmg, 20, 220)
        };
    }

    Skill_Result Skill_401(Stats stats, Stats enemyStats, int fails) //Curación
    {
        float recover = stats.skill_now * 10 + 80 - fails * 7 - stats.poison * 10;
        return new Skill_Result()
        {
            recoverHP = Mathf.Clamp(recover, 20, 300)
        };
    }

    Skill_Result Skill_402(Stats stats, Stats enemyStats, int fails) //Vudú
    {
        float dmg = (stats.skill_now * 0.1f) * 150 - fails * 10;
        int newConfusion = Random.Range(stats.luck_now, 14) >= 8 ? 3 : 1;
        return new Skill_Result()
        {
            confusion = newConfusion,
            value = Mathf.Clamp(dmg, 20, 200)
        };
    }

    Skill_Result Skill_403(Stats stats, Stats enemyStats, int fails) //Contagiar
    {
        if(fails <= 2)
        {
            int veneno = stats.poison;
            int mareo = stats.dizziness;
            int sangrado = stats.bleed;
            int newConfusion = stats.confusion;

            return new Skill_Result()
            {
                bleed = sangrado,
                poison = veneno,
                dizziness = mareo,
                confusion = newConfusion
            };
        }
        else
        {
            Message.instance.NewMessage(Lenguaje.Instance.Text_HabilidadFallada());
        }
        return new Skill_Result();
    }

    Skill_Result Skill_404(Stats stats, Stats enemyStats, int fails) //Protección
    {
        int newBarrier, newShield;
        if (fails <= 1)
        {
            newBarrier = Random.Range(stats.luck_now, 14) >= 9 ? 4 : 2;
            newShield = Random.Range(stats.luck_now, 14) >= 9 ? 4 : 2;
        }
        else
        {
            newBarrier = Random.Range(stats.luck_now, 14) >= 9 ? 2 : 1;
            newShield = Random.Range(stats.luck_now, 14) >= 9 ? 2 : 1;
        }
        return new Skill_Result()
        {
            buff_shield = newShield,
            buff_barrier = newBarrier
        };
    }

    Skill_Result Skill_405(Stats stats, Stats enemyStats, int fails) //Limpiar
    {
        int nLimpiar = stats.skill_now + 1 - fails;
        float heal = stats.skill_now * 10 + 20 - fails * 10 - stats.poison * 10;
        return new Skill_Result()
        {
            cleans = Mathf.Clamp(nLimpiar, 1, 7),
            recoverHP = Mathf.Clamp(heal, 20, 150)
        };
    }

    Skill_Result Skill_406(Stats stats, Stats enemyStats, int fails) //Maldición
    {
        int newPoison = 0, newBleed = 0, newDizziness = 0, newConfusion = 0;

        int nEfectos = stats.skill_now - fails;
        nEfectos = Mathf.Clamp(nEfectos, 1, 8);
        for(var x = 0; x < nEfectos; x++)
        {
            switch(Random.Range(0, 4))
            {
                case 0: newPoison++; break;
                case 1: newBleed++; break;
                case 2: newDizziness++; break;
                case 3: newConfusion++; break;
            }
        }
        return new Skill_Result()
        {
            poison = newPoison,
            bleed = newBleed,
            dizziness = newDizziness,
            confusion = newConfusion
        };
    }

    Skill_Result Skill_407 (Stats stats, Stats enemyStats, int fails) //Ritual
    {
        int total = 0;
        total += enemyStats.bleed + enemyStats.confusion + enemyStats.dizziness + enemyStats.poison;
        float dmg = (stats.skill_now * 0.2f) * 100 + total * (8 - fails);
        return new Skill_Result()
        {
            value = Mathf.Clamp(dmg, 20, 400)
        };
    }

    Skill_Result Skill_408(Stats stats, Stats enemyStats, int fails) //Plagiar
    {
        if(fails <= 2)
        {
            return new Skill_Result()
            {
                buff_attack = enemyStats.buff_attack,
                buff_skill = enemyStats.buff_skill,
                buff_luck = enemyStats.buff_luck,
                buff_shield = enemyStats.shield,
                buff_barrier = enemyStats.barrier
            };
        }
        Message.instance.NewMessage(Lenguaje.Instance.Text_HabilidadFallada());
        return new Skill_Result();
    }

    Skill_Result Skill_409(Stats stats, Stats enemyStats, int fails) //Condenar
    {
        int newPoison = 0, newBleed = 0, newDizziness = 0, newConfusion = 0, cantidad = 0;
        float dmg = (stats.skill_now * 0.1f) * 120 - fails * 15;
        if(fails <= 2)
        {
            cantidad = Random.Range(stats.luck_now, 14) >= 11 ? 3 : 2;
            newPoison = newBleed = newDizziness = newConfusion = cantidad;
        }else
        {
            cantidad = 1;
        }
        
        return new Skill_Result()
        {
            poison = newPoison,
            bleed = newBleed,
            dizziness = newDizziness,
            confusion = newConfusion,
            value = Mathf.Clamp(dmg, 10, 200)
        };
    }

    Skill_Result Skill_410(Stats stats, Stats enemyStats, int fails) //Peste
    {
        int newPoison = stats.luck_now - fails;
        newPoison = Mathf.Clamp(newPoison, 1, 4);
        float dmg = (stats.skill_now * 0.2f) * 120 - fails * 10;
        return new Skill_Result()
        {
            poison = newPoison,
            value = Mathf.Clamp(dmg, 20, 200)
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

