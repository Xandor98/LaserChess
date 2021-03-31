using System;
using UnityEngine;

public class Move
{
    public bool HasFigure { get; set; } = false;

    public Vector3 Origin { get; set; }
    public Vector3 Destination { get; set; }

    private Figure _figure;

    public Figure Figure_To_Move
    {
        get => _figure;

        set
        {
            if (value != null)
            {
                HasFigure = true;
            }
            else
            {
                HasFigure = false;
            }

            _figure = value;
        }
    }
}