﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapGame : MonoBehaviour {

    Image fillBar;
    RectTransform t_clockhand;
    public byte difficult_level = 1;
    float minValue = 0.2f;
    float maxValue = 0.8f;

    float value;
    float timeUp;

    bool active;

    void OnEnable()
    {
        Inicialize();
    }

    void Inicialize()
    {
        active = false;
        value = minValue;
        fillBar = transform.Find("Fill_Bar").GetComponent<Image>();
        t_clockhand = transform.Find("Clock_Hand").GetComponent<RectTransform>();
        t_clockhand.localRotation = Quaternion.Euler(0, 0, 90);
        fillBar.fillAmount = minValue;
        
    }

    void Update()
    {
        if (active)
        {
            if (Time.time < timeUp)
            {
                value -= Time.deltaTime * 0.3f * difficult_level;
                value = Mathf.Clamp(value, minValue, maxValue);
                ApuntarHand(value);
                fillBar.fillAmount = Mathf.Lerp(fillBar.fillAmount, value, Time.deltaTime * difficult_level);
            }
            else
            {
                float f = 10 - (10 * Mathf.Clamp((fillBar.fillAmount - minValue / maxValue) * 2, 0f, 1f));
                BattleSystem.instance.minigameFails = (int)f;
                BattleSystem.instance.EndMinigame();
                transform.parent.gameObject.SetActive(false);
            }
        }
    }

    void ApuntarHand(float val)
    {
        float formula = Mathf.Clamp((value - minValue / maxValue) * 2, 0f, 1f);
        float z = 90 - 180 * formula;
        t_clockhand.localRotation = Quaternion.Lerp(t_clockhand.localRotation, Quaternion.Euler(0, 0, z), Time.deltaTime * difficult_level * 2);
    }

    public void Clicked()
    {
        if (!active) Activar();
        value += 0.25f;
    }

    void Activar()
    {
        timeUp = Time.time + 2f;
        active = true;
    }
}
