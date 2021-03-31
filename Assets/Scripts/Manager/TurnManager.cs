using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private static TurnManager _instance;

    public static TurnManager Instance { get { return _instance; } }

    public APlayer[] playerList;

    private TeamColor CurrentPlayer = TeamColor.BLUE;

    public enum TeamColor
    {
        BLUE, RED
    }

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

        GameObject[] _gameObjects = GameObject.FindGameObjectsWithTag("Player");
        if(_gameObjects.Length != 2)
        {
            Debug.LogError("Not Enougth Player");
            //TODO: End game
        }

        playerList = new APlayer[_gameObjects.Length];

        playerList[0] = _gameObjects[0].GetComponent<APlayer>();
        playerList[1] = _gameObjects[1].GetComponent<APlayer>();

        playerList[0].MyColor = TeamColor.BLUE;
        playerList[0].IsTurn = true;

        playerList[1].MyColor = TeamColor.RED;
        playerList[1].IsTurn = false;
    }

    private void EndOFRoundListener()
    {
        MapManager.Instance.FireLaser(CurrentPlayer);

        switch (CurrentPlayer)
        {
            case TeamColor.BLUE:
                CurrentPlayer = TeamColor.RED;
                break;
            case TeamColor.RED:
                CurrentPlayer = TeamColor.BLUE;
                break;
        }
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
        Move current_Move = null;
        foreach (APlayer player in playerList)
        {
            if (player.MyColor == CurrentPlayer)
            {
                current_Move = player.MakeMove();
            }
        }

       //TODO: Verify Move

        if(current_Move != null)
        {
            if (current_Move.HasFigure)
            {
                current_Move.Figure_To_Move.transform.position = current_Move.Destination;
            }

            EventManager.EndOfRound();
        }
    }
}
