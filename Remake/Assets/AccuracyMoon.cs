using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccuracyMoon : MonoBehaviour {

    RectTransform t_pointer;
    public byte difficulty = 2;
    bool active = true;
    bool dir;

    void Start()
    {
        t_pointer = transform.Find("Pointer").GetComponent<RectTransform>();
        t_pointer.localPosition = new Vector3(0, 300, 0);
    }

    void Update()
    {
        if (active)
        {
            if (dir)
            {
                t_pointer.localPosition = Vector3.Lerp(t_pointer.localPosition, new Vector3(0, -400, 0), Time.deltaTime * difficulty);
            }
            else
            {
                t_pointer.localPosition = Vector3.Lerp(t_pointer.localPosition, new Vector3(0, 400, 0), Time.deltaTime * difficulty);
            }
            if (t_pointer.localPosition.y > 300) dir = !dir;
            if (t_pointer.localPosition.y < -300) dir = !dir;
        }
    }

    public void Clicked()
    {
        active = !active;
    }
}
