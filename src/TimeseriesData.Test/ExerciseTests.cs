using Xunit;
using TimeseriesData;
using FluentAssertions;
using System;

namespace TimeseriesData.Test;

public class ExerciseTests
{
    [Fact]
    public void TestPassThrough()
    {
        var input = new Point[] {
            new Point(0, 100),
            new Point(8.4, 110),
            new Point(0.999, 120),
        };

        var got = Exercise.Fix(input, 100, 130, 10);

        var exp = new Point[] {
            new Point(0, 100),
            new Point(8.4, 110),
            new Point(0.999, 120),
        };

        exp.Should().BeEquivalentTo(got);
    }

    [Fact]
    public void TestApplyFromTo()
    {
        var input = new Point[] {
          new Point(0.5, 100),
          new Point(8.4, 110),
          new Point(32.5, 120),
          new Point(0.999, 130),
          };

        var got = Exercise.Fix(input, 110, 130, 10);

        var exp = new Point[] {
          new Point(8.4, 110),
          new Point(32.5, 120),
        };

        exp.Should().BeEquivalentTo(got);
    }

    [Fact]
    public void TestApplyFromToNaNs()
    {
        var input = new Point[] {
              new Point(0.5, 100),
              new Point(41.3, 110),
          };

        var got = Exercise.Fix(input, 90, 140, 10);

        var exp = new Point[] {
            new Point(double.NaN, 90),
            new Point(0.5, 100),
            new Point(41.3, 110),
            new Point(double.NaN, 120),
            new Point(double.NaN, 130),
        };

        exp.Should().BeEquivalentTo(got);
    }

    [Fact]
    public void TestApplyIrregularFromToNaNs()
    {
        var input = new Point[] {
            new Point(0.5, 100),
            new Point(41.3, 110),
          };

        var got = Exercise.Fix(input, 89, 131, 10);

        var exp = new Point[] {
            new Point(double.NaN, 90),
            new Point(0.5, 100),
            new Point(41.3, 110),
            new Point(double.NaN, 120),
            new Point(double.NaN, 130),
        };

        exp.Should().BeEquivalentTo(got);
    }

    [Fact]
    public void TestAdjustIrregularTimestamps()
    {
        var input = new Point[] {
          new Point(0.5, 100),
          new Point(8.4, 110),
          new Point(32.5, 118),
          new Point(0.999, 130),
          new Point(41.3, 139),
          new Point(41.9, 141),
        };

        var got = Exercise.Fix(input, 100, 160, 10);

        var exp = new Point[] {
          new Point(0.5, 100),
          new Point(8.4, 110),
          new Point(32.5, 120),
          new Point(0.999, 130),
          new Point(41.3, 140),
          new Point(41.9, 150),
        };

        exp.Should().BeEquivalentTo(got);
    }

    [Fact]
    public void TestInsertNaNs()
    {
        var input = new Point[] {
          new Point(0.5, 100),
          new Point(32.5, 120),
          new Point(0.999, 130),
          new Point(41.3, 150),
        };

        var got = Exercise.Fix(input, 100, 160, 10);

        var exp = new Point[] {
          new Point(0.5, 100),
          new Point(double.NaN, 110),
          new Point(32.5, 120),
          new Point(0.999, 130),
          new Point(double.NaN, 140),
          new Point(41.3, 150),
        };

        exp.Should().BeEquivalentTo(got);
    }

    [Fact]
    public void TestFilterDuplicates()
    {
        var input = new Point[] {
          new Point(0.5, 100),
          new Point(0.7, 100),
          new Point(32.5, 110),
          new Point(0.9, 120),
          new Point(0.8, 125),
          new Point(41.3, 130),
          new Point(41.3, 140),
          new Point(41.2, 141),
          new Point(41.3, 142),
          new Point(41.4, 149),
        };
        var got = Exercise.Fix(input, 100, 160, 10);
        var exp = new Point[] {
          new Point(0.5, 100),
          new Point(32.5, 110),
          new Point(0.9, 120),
          new Point(0.8, 130),
          new Point(41.3, 140),
          new Point(41.2, 150),
        };

        exp.Should().BeEquivalentTo(got);
    }

    [Fact]
    public void TestIrregularFromToIrregularTimestamps()
    {
        var input = new Point[] {
          new Point(0.5, 105),
          new Point(1, 120),
          new Point(1.5, 130),
          new Point(2, 135),
          new Point(3, 139),
        };
        var got = Exercise.Fix(input, 108, 138, 10);
        var exp = new Point[] {
          new Point(0.5, 110),
          new Point(1, 120),
          new Point(1.5, 130),
        };
        exp.Should().BeEquivalentTo(got);
    }

    [Fact]
    public void TestAllAtOnce()
    {
        var input = new Point[] {
          new Point(0.5, 105),
          new Point(41.3, 111),
          new Point(0, 130),
          new Point(1, 150),
          new Point(2, 151),
          new Point(3, 152),
          new Point(4, 159),
          new Point(4, 160),
          new Point(5, 160),
          new Point(4, 174),
          new Point(5, 175),
          new Point(6, 185),
        };
        var got = Exercise.Fix(input, 81, 186, 10);

        var exp = new Point[] {
          new Point(double.NaN, 90),
          new Point(double.NaN, 100),
          new Point(0.5, 110),
          new Point(41.3, 120),
          new Point(0, 130),
          new Point(double.NaN, 140),
          new Point(1, 150),
          new Point(2, 160),
          new Point(double.NaN, 170),
          new Point(4, 180),
        };
        exp.Should().BeEquivalentTo(got);
    }
}
