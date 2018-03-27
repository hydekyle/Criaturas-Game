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

}

public class Skill
{
    string name;
    string description;
    Class clase;
    Skill_Type s_type;
    int damage;
    int actionNumber;
}
