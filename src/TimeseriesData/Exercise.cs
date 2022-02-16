using System;
using System.Linq;

namespace TimeseriesData;

public class Exercise
{
    public static Point[] Fix(Point[] points, int from, int to, int interval)
	{
        return points
            .MapToValidFormat(from, to, interval)
            .IsNotMultipleItShouldBeAdjusted(from, to, interval)
            .ToArray();
    }
}