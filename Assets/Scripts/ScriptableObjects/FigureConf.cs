using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FigureConf", menuName = "FigureConf")]
public class FigureConf : ScriptableObject
{
    public bool IsRotateable;
    public bool IsMoveable;

    public int Max_Possible_Moves;
}
