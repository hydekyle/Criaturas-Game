using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lenguaje : MonoBehaviour {

    [HideInInspector]
    public static Lenguaje Instance;

    /// TEXTOS DEL JUEGO:
    [HideInInspector]
    public string play, nest, shop, exit, 
    health, attack, skill, luck;


    void Awake()
    {
        Instance = this;
        CheckAndSetLanguage();
    }

    void SetLanguage(bool spanish)
    {
        if (spanish)
        {
            play = "Empezar";
            nest = "Nido";
            shop = "Tienda";
            exit = "Salir";

            health = "Vida";
            attack = "Ataque";
            skill = "Habilidad";
            luck = "Suerte";
        }
        else
        {
            play = "Play";
            nest = "Nest";
            shop = "Shop";
            exit = "Exit";

            health = "Health";
            attack = "Attack";
            skill = "Skill";
            luck = "Luck";
        }
    }

    void CheckAndSetLanguage()
    {
        if (Application.systemLanguage == SystemLanguage.Spanish) SetLanguage(true); else SetLanguage(false);
    }

}
