using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private static TurnManager _instance;

    public static TurnManager Instance { get { return _instance; } }

    public float x_tile_gap = 1.3f;
    public float z_tile_gap = 1.3f;

    public enum TeamColor
    {
        BLUE, RED
    }

    private GameObject selected = null;
    private bool canMakeMove = true;
    private TeamColor yourColor = TeamColor.BLUE;
    private bool YourMove = true;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        EventManager.EndOfRoundHandler += EndOFRoundListener;
        EventManager.UnitKilledHandler += UnitKilledListener;

        YourMove = yourColor == TeamColor.BLUE;
    }

    private void EndOFRoundListener()
    {
        YourMove = !YourMove;
        canMakeMove = YourMove;

        Debug.Log("Your Move: " + YourMove);
    }

    private void UnitKilledListener(Figure figure)
    {
        if (figure.gameObject.name.Contains("king"))
        {
            if(figure.tag == TeamColor.BLUE.ToString().ToLower())
            {
                EventManager.EndGameTrigger(TeamColor.BLUE);
            }
            else
            {
                EventManager.EndGameTrigger(TeamColor.RED);
            }
        }
        Debug.Log("Unit has been Killed");
    }

    // Update is called once per frame
    void Update()
    {
        if (YourMove)
        {
            if (Input.GetMouseButtonDown(0) && canMakeMove)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("MouseLayer")))
                {
                    if (hit.transform.gameObject.TryGetComponent(out IClickable tmp))
                    {
                        if (selected == null && hit.transform.tag == yourColor.ToString().ToLower())
                        {
                            FigureConf conf = hit.transform.gameObject.GetComponent<Figure>().config;
                            if(conf.IsMoveable || conf.IsRotateable)
                                selected = tmp.OnClick();
                        }
                        else if (selected != null && hit.transform.gameObject.TryGetComponent(out Tile tmp_tile))
                        {
                            FigureConf conf = selected.GetComponent<Figure>().config;
                            //if Selected and click on tile

                            int x_t_val = (int)(hit.transform.position.x / x_tile_gap);
                            int z_t_val = (int)(hit.transform.position.z / z_tile_gap);

                            int x_f_val = (int)(selected.transform.position.x / x_tile_gap);
                            int z_f_val = (int)(selected.transform.position.z / z_tile_gap);

                            int x_diff = Mathf.Abs(x_t_val - x_f_val);
                            int z_diff = Mathf.Abs(z_t_val - z_f_val);
                            
                            if (conf.IsMoveable && (x_diff > 0 && z_diff > 0))
                            {
                                if ((x_diff == 0 && z_diff <= conf.Max_Possible_Moves) ||
                                (x_diff <= conf.Max_Possible_Moves && z_diff == 0) ||
                                (x_diff <= conf.Max_Possible_Moves && z_diff <= conf.Max_Possible_Moves))
                                {
                                    selected.transform.position = hit.transform.position + new Vector3(0, 1.1f, 0);
                                    canMakeMove = false;
                                }
                            }
                        }
                    }
                }
            }

            if (selected != null && Input.GetButtonDown("Cancel"))
            {
                selected.GetComponent<Figure>().UnSelectUnit();
                selected = null;
                canMakeMove = true;
            }

            CheckRotation();

            if (selected == null && Input.GetButtonDown("Submit"))
            {
                MapManager.Instance.FireLaser(yourColor);
                EventManager.EndOfRound();
            }
            else if (selected != null && Input.GetButtonDown("Submit"))
            {
                selected.transform.position -= new Vector3(0, 1.0f, 0);
                selected = null;
            }
        }
    }

    void CheckRotation()
    {
        if(selected != null)
        {
            bool can_rotate = selected.GetComponent<Figure>().config.IsRotateable;
            if (can_rotate && Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                selected.GetComponent<Figure>().RotateClockwise();
            }

            if (can_rotate && Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                selected.GetComponent<Figure>().RotateCounterClockwise();
            }
        }
        
    }
}
