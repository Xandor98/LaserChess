using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("MouseLayer")))
            {
                Debug.Log(hit.transform.name);
                if (hit.transform.gameObject.TryGetComponent(out IClickable tmp))
                {
                    tmp.OnClick();
                }
            }
        }
    }
}
