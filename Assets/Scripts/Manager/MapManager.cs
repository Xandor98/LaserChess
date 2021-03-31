using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    #region Singelton Header
    private static MapManager _instance;

    public static MapManager Instance { get { return _instance; } }

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
    #endregion

    private Figure[] figures;
    private Tile[] tiles;

    private float x_gap = 1.3f;
    private float z_gap = 1.3f;

    // Start is called before the first frame update
    void Start()
    {
        figures = GetComponentsInChildren<Figure>();
        tiles = GetComponentsInChildren<Tile>();
    }

    public Laser GetLaser(TurnManager.TeamColor color)
    {
        foreach(Figure f in figures)
        {
            if(f.tag == color.ToString().ToLower())
            {
                if (f.gameObject.TryGetComponent<Laser>(out Laser l))
                {
                    return l;
                }
            }
        }

        return null;
    }

    public void FireLaser(TurnManager.TeamColor color)
    {
        Laser l;
        if ((l = GetLaser(color)) != null)
        {
            l.fire = true;
        }
    }

    public float GetXGap()
    {
        return x_gap;
    }

    public float GetZGap()
    {
        return z_gap;
    }
    
}
