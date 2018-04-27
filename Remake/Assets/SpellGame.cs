using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellGame : MonoBehaviour {

    Transform circle;
    RectTransform rotor;
    RectTransform firstOne;

    void OnEnable()
    {
        circle = circle ?? transform.Find("Circle");
        rotor = rotor ?? circle.Find("Rotor").GetComponent<RectTransform>();
        firstOne = firstOne ?? rotor.GetChild(0).GetComponent<RectTransform>();
    }

    void SetCopy()
    {
        GameObject go = Instantiate(firstOne.gameObject);
        RectTransform rectGO = go.GetComponent<RectTransform>();
        go.name = "1";
        go.transform.SetParent(rotor.transform);
        rectGO.localScale = firstOne.localScale;
        rectGO.localPosition = firstOne.localPosition;
        go.transform.SetParent(circle);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) SetCopy();
    }
}
