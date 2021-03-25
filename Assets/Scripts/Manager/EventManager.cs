using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour
{
    public delegate void EndRound();
    public static event EndRound EndOfRoundHandler;

    public delegate void UnitKilled(Figure figure);
    public static event UnitKilled UnitKilledHandler;

    public delegate void EndGame(TurnManager.TeamColor winningColor);
    public static event EndGame EndGameHandler;

    public static void EndOfRound()
    {
        EndOfRoundHandler?.Invoke();
    }

    public static void UnitKilledTrigger(Figure figure)
    {
        UnitKilledHandler?.Invoke(figure);
    }

    public static void EndGameTrigger(TurnManager.TeamColor winningColor)
    {
        EndGameHandler?.Invoke(winningColor);
    }
}
