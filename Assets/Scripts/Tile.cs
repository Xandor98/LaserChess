using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour, IClickable
{
    
    void Update()
    {

    }

    public GameObject OnClick()
    {
        return transform.gameObject;
    }
}
