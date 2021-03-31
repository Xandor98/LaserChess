using UnityEngine;
using System.Collections;

public abstract class APlayer : MonoBehaviour
{
    public Camera cam;
    public AudioListener audioListener;

    public bool IsTurn { get; set; } = false;
    public TurnManager.TeamColor MyColor;

    private GameObject selected = null;
    private bool canMakeMove = true;
    private float x_tile_gap;
    private float z_tile_gap;

    private Move ret = null;

    void Start()
    {
        if (cam == null)
        {
            cam = GetComponent<Camera>();
        }

        if (audioListener == null)
        {
            audioListener = GetComponent<AudioListener>();
        }

        x_tile_gap = MapManager.Instance.GetXGap();
        z_tile_gap = MapManager.Instance.GetZGap();

        EventManager.EndOfRoundHandler += EndOfRoundListener;
    }

    void Update()
    {
        if (IsTurn)
        {
            cam.enabled = true;
            audioListener.enabled = true;

            ret = ProcessMove();
        }
        else
        {
            cam.enabled = false;
            audioListener.enabled = false;
        }
    }

    public void EndOfRoundListener()
    {
        IsTurn = !IsTurn;
        canMakeMove = true;

        selected = null;
    }

    public Move MakeMove()
    {
        return GetMove();
    }

    /// <summary>
    /// Returns The Move of The Player
    /// 
    /// If Move Null No Figure has been Moved and Round Not End
    /// If Move isnt Null and Contains a Figure a Destiny and a Origin needs to be set
    /// And If No Figure is Set the Round ends without Moving a Figure
    /// 
    /// </summary>
    /// <returns>If A Move has been Done</returns>
    protected virtual Move GetMove()
    {
        return ret;
    }

    private Move ProcessMove()
    {

        if (Input.GetMouseButtonDown(0) && canMakeMove)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("MouseLayer")))
            {
                if (hit.transform.gameObject.TryGetComponent(out IClickable tmp))
                {
                    Debug.Log("Clicked on: " + hit.transform.name);
                    if (selected == null && hit.transform.tag == MyColor.ToString().ToLower())
                    {
                        FigureConf conf = hit.transform.gameObject.GetComponent<Figure>().config;
                        if (conf.IsMoveable || conf.IsRotateable)
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

                        if (conf.IsMoveable)
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
            return new Move();
        }

        else if (selected != null && Input.GetButtonDown("Submit"))
        {
            canMakeMove = false;

            Move move = new Move();
            move.Figure_To_Move = selected.GetComponent<Figure>();
            move.Origin = move.Figure_To_Move.GetOriginPos();
            move.Destination = selected.transform.position -= new Vector3(0, 1.0f, 0);

            selected = null;

            return move;
        }

        return null;
    }

    private void CheckRotation()
    {
        if (selected != null)
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
