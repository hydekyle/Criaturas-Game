﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Enums;

public class OsuBall : MonoBehaviour {

    RectTransform circle_out;
    Image circle_in_image;
    Image circle_out_image;
    float velocity = 1.8f;
    float maxRangeOK = 1.1f;
    float minRangeOK = 0.6f;
    bool isActive = true;
    bool ok;
    public bool isLastBall;

    void OnEnable()
    {
        gameObject.SetActive(true);
        circle_in_image = GetComponent<Image>();
        circle_out = transform.GetChild(0).GetComponent<RectTransform>();
        circle_out_image = circle_out.GetComponent<Image>();
        circle_out.gameObject.SetActive(true);
    }

    public void SetSpeed(Speed speed)
    {
        switch (speed)
        {
            case Speed.Slow:   velocity = 1.8f; break;
            case Speed.Normal: velocity = 2.0f; break;
            case Speed.Fast:   velocity = 2.4f; break;
        }
    }

    void FixedUpdate()
    {
        if (isActive)
        {
            circle_out_image.color = Color.Lerp(circle_out_image.color, Color.white, Time.deltaTime * velocity);
            circle_in_image.color = Color.Lerp(circle_out_image.color, Color.white, Time.deltaTime * velocity * 3);
            if (circle_out.localScale.x > minRangeOK)
            {
                circle_out.localScale -= Vector3.one * velocity * Time.deltaTime;
            }
            else
            {
                Failed();
            }
        }else
        {
            if (ok) circle_in_image.color = Color.Lerp(circle_in_image.color, new Color(0, 1, 0, 0), Time.deltaTime * velocity * 2);
               else circle_in_image.color = Color.Lerp(circle_in_image.color, new Color(1, 0, 0, 0), Time.deltaTime * velocity * 2);

            if (circle_in_image.color.a < 0.01f) Destroy(gameObject);
        }
    }

    public void OnClick()
    {
        if (circle_out.localScale.x > minRangeOK && circle_out.localScale.x < maxRangeOK) Good(); else Failed();
    }

    void Good()
    {
        ok = true;
        isActive = false;
        circle_out.gameObject.SetActive(false);
    }

    void Failed()
    {
        BattleSystem.instance.minigameFails++;
        isActive = false;
        circle_out.gameObject.SetActive(false);
    }

    void OnDisable()
    {
        if(isLastBall) BattleSystem.instance.EndMinigame();
    }

}
