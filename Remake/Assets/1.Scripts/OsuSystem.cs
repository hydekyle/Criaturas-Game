using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class OsuSystem : MonoBehaviour {

    public GameObject osu_ball;
    public static OsuSystem instance;
    Vector3 escala = new Vector3(0.5f, 0.5f, 0.5f);

    void Awake()
    {
        instance = this;
    }


    void PutBall(Vector3 position, Speed speed)
    {
        GameObject go = Instantiate(osu_ball);
        go.GetComponent<OsuBall>().SetSpeed(speed);
        RectTransform rectT = go.GetComponent<RectTransform>();
        go.transform.SetParent(transform);
        rectT.localScale = escala;
        rectT.localPosition = position;
        go.gameObject.SetActive(true);
    }

    void PutLastBall(Vector3 position, Speed speed)
    {
        GameObject go = Instantiate(osu_ball);
        OsuBall ball = go.GetComponent<OsuBall>();
        ball.SetSpeed(speed);
        ball.isLastBall = true;
        RectTransform rectT = go.GetComponent<RectTransform>();
        go.transform.SetParent(transform);
        rectT.localScale = escala;
        rectT.localPosition = position;
        go.gameObject.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3)) StartCoroutine(PatronRandomHorizontal_3());
    }

    public void Bolas()
    {
        StartCoroutine(PatronRandomHorizontal_3());
    }

    IEnumerator PatronRandomHorizontal_3()
    {
        float y = Random.Range(-200f, 200f);
        int distanceX = Random.Range(100, 220);
        int dir = Random.Range(0, 2) == 1 ? 1 : -1;
        Vector3 puntoInicial = new Vector3(-200 * dir, y, 1);
        PutBall(puntoInicial, SpeedByInt(Random.Range(0,3)));
        yield return new WaitForSeconds(Random.Range(0.5f, 0.8f));
        PutBall(puntoInicial + Vector3.right * distanceX * dir, SpeedByInt(Random.Range(0, 3)));
        yield return new WaitForSeconds(Random.Range(0.5f, 0.8f));
        PutLastBall(puntoInicial + Vector3.right * distanceX * 2 * dir, SpeedByInt(Random.Range(0, 3)));
    }

    Speed SpeedByInt(int i)
    {
        Speed speed = Speed.Normal;
        switch (i)
        {
            case 0: speed = Speed.Slow;   break;
            case 1: speed = Speed.Normal; break;
            case 2: speed = Speed.Fast;   break;
        }
        return speed;
    }


}
