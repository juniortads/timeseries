using System;
namespace TimeseriesData;

public static class PointServiceExtensions
{
    public static Point[] ToArray(this IList<Point> points) => points
            .GroupBy(a => a.Ts)
            .Select(a => a.First())
            .OrderBy(a => a.Ts)
            .ToArray();

    public static IList<Point> IsNotMultipleItShouldBeAdjusted(this IList<Point> points, int from, int to, int interval)
    {
        for (int i = from; i < to; i++)
        {
            if (!IsMultiple(interval, i) || points.Any(o => o.Ts.Equals(i)))
                continue;

            AddPoint(points, new Point(double.NaN, i), from, to);
        }
        return points;
    }

    public static IList<Point> MapToValidFormat(this Point[] points, int from, int to, int interval)
    {
        var fixedPoints = new List<Point>();
        foreach (var item in points)
            SearchMultipleAndAdds(fixedPoints, item, from, to, interval);

        return fixedPoints;
    }

    private static void SearchMultipleAndAdds(List<Point> points, Point item, int from, int to, int interval)
    {
        if (!IsValid(item, from, to))
        {
            AddPoint(points, GetMultipleNumberAndAdds(item, interval, from, to), from, to);
        }
        else
        {
            AddsIfNumberIsMultipleAndValid(points, item, interval, from, to);
        }
    }

    private static void AddsIfNumberIsMultipleAndValid(List<Point> points, Point point, int interval, int from, int to)
    {
        if (IsMultiple(point.Ts, interval))
            AddPoint(points, point, from, to);
        else
            AddPoint(points, GetMultipleNumberAndAdds(point, interval, from, to), from, to);
    }

    private static Point? GetMultipleNumberAndAdds(Point point, int interval, int from, int to)
    {
        point.Ts = SearchNumberIsMultiple(interval, point.Ts, Direction.After);

        if (IsValid(point, from, to))
        {
            return point;
        }
        else
        {
            point.Ts = SearchNumberIsMultiple(interval, point.Ts, Direction.Before);

            if (IsValid(point, from, to))
            {
                return point;
            }
        }
        return null;
    }

    private static void AddPoint(IList<Point> points, Point? point, int from, int to)
    {
        if (IsValid(point, from, to) && point != null)
        {
            points.Add(point);
        }
    }

    private static bool IsValid(Point? item, int from, int to)
    {
        return item != null && item.Ts >= from && item.Ts < to;
    }

    private static bool IsMultiple(int multiple, int number)
    {
        return number % multiple == 0;
    }

    private static int SearchNumberIsMultiple(int multiple, int number, Direction direction)
    {
        if (IsMultiple(multiple, number))
            return number;
        else
        {
            return direction switch
            {
                Direction.After => SearchNumberIsMultiple(multiple, number + 1, direction),
                Direction.Before => SearchNumberIsMultiple(multiple, number - 1, direction),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), $"Not expected direction value: {direction}"),
            };
        }
    }
}