using UnityEngine;
using System.Collections;

public class Figure : MonoBehaviour, IClickable
{
    public FigureConf config;

    private bool Selected = false;

    private Vector3 originPos;
    private Quaternion originRot;
    void Start()
    {

    }

    public GameObject OnClick()
    {
        Selected = !Selected;

        if (Selected)
        {
            originPos = transform.position;
            originRot = transform.rotation;
            transform.position += new Vector3(0, 1, 0);
        }
        else
        {
            transform.position = originPos;
        }

        return transform.gameObject;
    }

    public void UnSelectUnit()
    {
        if (Selected)
        {
            Selected = false;
            transform.position = originPos;
            transform.rotation = originRot;
        }
    }

    public void RotateClockwise()
    {
        if (Selected && config.IsRotateable) {
            transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
        }
    }

    public void RotateCounterClockwise()
    {
        if (Selected && config.IsRotateable)
        {
            transform.Rotate(0.0f, -90.0f, 0.0f, Space.Self);
        }
    }
}