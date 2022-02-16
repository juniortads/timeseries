using System;
namespace TimeseriesData;

public class Point
{
    public int Ts { get; set; }
    public double Val { get; set; }

    public Point(double val, int ts)
    {
        Ts = ts;
        Val = val;
    }
}

public enum Direction
{
    After = 0,
    Before = 1
}