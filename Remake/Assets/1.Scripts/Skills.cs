using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Skills : MonoBehaviour {

    public static Skills Instance;

    void Awake()
    {
        Instance = this;
    }

    public Skill_Result SkillResolve(int ID_skill)
    {
        Skill_Result result = new Skill_Result();

        return result;
    }

}

