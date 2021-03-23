using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour, IClickable
{
    public Transform laser_start_pos;
    public float max_reach = 5000;
    public int max_reflections = 5;

    public bool fire = false;
    public float fire_duartion = 1;
    
    private LineRenderer lr;

    private bool deaktivate_started = false;
    private bool destroying_started = false;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fire)
        {
            lr.enabled = true;
            lr.positionCount = 1;
            lr.SetPosition(0, laser_start_pos.position);

            PerformLaserShooting(laser_start_pos.position, laser_start_pos.forward, 1, 0);
        }
        else
        {
            lr.enabled = false;
            deaktivate_started = false;
            destroying_started = false;
        }
    }

    private void PerformLaserShooting(Vector3 startPoint, Vector3 direction, int currentPos, int currentReflections)
    {
        lr.positionCount += 1;

        RaycastHit hit;
        if (Physics.Raycast(startPoint, direction, out hit))
        {
            if (hit.collider)
            {
                if (hit.transform.tag == "Mirror" && currentReflections < max_reflections)
                {
                    lr.SetPosition(currentPos, hit.point);
                    PerformLaserShooting(hit.point, Vector3.Reflect(direction, hit.normal), currentPos + 1, currentReflections + 1);
                }
                else if (hit.transform.tag == "Destroyer")
                {
                    lr.SetPosition(currentPos, hit.point);
                    if (!destroying_started)
                    {
                        StartCoroutine(DestroyTimer(hit.transform.parent.gameObject));
                    }
                }
                else
                {
                    lr.SetPosition(currentPos, hit.point);
                    if (!deaktivate_started)
                    {
                        StartCoroutine(DeaktivateTimer());
                    }
                }
            }
        }
        else
        {
            lr.SetPosition(currentPos, startPoint + direction * max_reach);
            if (!deaktivate_started)
            {
                StartCoroutine(DeaktivateTimer());
            }
        }
    }

    IEnumerator DeaktivateTimer()
    {
        deaktivate_started = true;
        yield return new WaitForSecondsRealtime(fire_duartion);
        fire = false;
    }

    IEnumerator DestroyTimer(GameObject to_destroy)
    {
        destroying_started = true;
        yield return new WaitForSecondsRealtime(1);
        Destroy(to_destroy);
        fire = false;
    }

    public void OnClick()
    {
        if(!fire)
            fire = true;
    }
}
