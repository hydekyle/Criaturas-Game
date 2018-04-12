using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OsuSystem : MonoBehaviour {

    public GameObject osu_ball;
    int x = 0;

    void PutBall(Vector3 position)
    {
        GameObject go = Instantiate(osu_ball);
        RectTransform rectT = go.GetComponent<RectTransform>();
        go.transform.SetParent(transform);
        rectT.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        rectT.localPosition = position;
        go.gameObject.name = x.ToString();
        go.gameObject.SetActive(true);
        x++;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3)) StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        PutBall(Vector3.zero);
        yield return new WaitForSeconds(0.4f);
        PutBall(Vector3.zero + Vector3.right * 200);
        yield return new WaitForSeconds(0.3f);
        PutBall(Vector3.zero + Vector3.left * 200);
        yield return null;
    }
}
