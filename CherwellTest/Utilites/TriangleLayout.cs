using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;

namespace CherwellTest.Utilites
{
    //public class Triangle
    //{
    //    private TriangleLayout _triangleLayout;
    //    private TriangleLocation _triangleLocation;
    //    private TriangleVerticies _triangleVerticies;
    //}

    public struct LayoutLocation
    {
        public string RowLetter;
        public int ColumnNumber;
    }

    public class TriangleLayout
    {
        private const int ROW_BASE = 0;
        private const int COL_BASE = 1;
        private const int TRIANGLE_WIDTH = 10;
        private const int TRIANGLE_HEIGHT = 10;

        private LayoutRange _layoutRange;

        public TriangleLayout(int totalRow, int totalColumns)
        {
            _layoutRange = new LayoutRange(totalRow, totalColumns);
        }

        private bool IsInRange(TriangleLocation triangleLocation)
        {
            return triangleLocation.RowIndex.IsBetween(ROW_BASE, _layoutRange.Rows + ROW_BASE, BetweenExtensions.CompareOptions.InclusiveStartExclusiveEnd) &&
                triangleLocation.ColumnIndex.IsBetween(COL_BASE, _layoutRange.Columns + COL_BASE, BetweenExtensions.CompareOptions.InclusiveStartExclusiveEnd);
        }

        private bool IsInRange(TriangleVerticies verticies)
        {
            return verticies.MinY.IsBetween(0, _layoutRange.Rows * TRIANGLE_HEIGHT) &&
                   verticies.MinX.IsBetween(0, _layoutRange.Columns / 2 * TRIANGLE_WIDTH) &&
                   verticies.MaxY.IsBetween(0, _layoutRange.Rows * TRIANGLE_HEIGHT) &&
                   verticies.MaxX.IsBetween(0, _layoutRange.Columns / 2 * TRIANGLE_WIDTH);
        }

        public TriangleVerticies GetVerticies(TriangleLocation location)
        {
            if (IsInRange(location) == false)
                throw new ArgumentOutOfRangeException(nameof(location), "Triangle location not in range.");

            var pt1 = new Point();
            var pt2 = new Point();
            var pt3 = new Point();

            if ((location.ColumnIndex - COL_BASE) % 2 == 0)
            {
                pt1.X = (location.ColumnIndex - COL_BASE) / 2 * TRIANGLE_WIDTH;
                pt1.Y = (location.RowIndex - ROW_BASE) * TRIANGLE_HEIGHT;
                pt2.X = (location.ColumnIndex - COL_BASE) / 2 * TRIANGLE_WIDTH;
                pt2.Y = (location.RowIndex - ROW_BASE + 1) * TRIANGLE_HEIGHT;
                pt3.X = ((location.ColumnIndex - COL_BASE) / 2 + 1) * TRIANGLE_WIDTH;
                pt3.Y = (location.RowIndex - ROW_BASE + 1) * TRIANGLE_HEIGHT;
            }
            else
            {
                pt1.X = (location.ColumnIndex - COL_BASE) / 2 * TRIANGLE_WIDTH;
                pt1.Y = (location.RowIndex - ROW_BASE) * TRIANGLE_HEIGHT;
                pt2.X = ((location.ColumnIndex - COL_BASE) / 2 + 1) * TRIANGLE_WIDTH;
                pt2.Y = (location.RowIndex - ROW_BASE) * TRIANGLE_HEIGHT;
                pt3.X = ((location.ColumnIndex - COL_BASE) / 2 + 1) * TRIANGLE_WIDTH;
                pt3.Y = (location.RowIndex - ROW_BASE + 1) * TRIANGLE_HEIGHT;
            }

            return new TriangleVerticies(pt1, pt2, pt3);

        }

        public TriangleLocation GetTriangleLocation(TriangleVerticies verticies)
        {
            if (IsInRange(verticies) == false)
                throw new ArgumentOutOfRangeException(nameof(verticies), "Triangle verticies not in range.");

            if (Math.Abs(verticies.MaxX - verticies.MinX) != TRIANGLE_WIDTH)
                throw new ArgumentException("Invalid verticies for layout (invalid wide).");

            if (Math.Abs(verticies.MaxY - verticies.MinY) != TRIANGLE_HEIGHT)
                throw new ArgumentException("Invalid verticies for layout (invalid height).");

            var hLineY = verticies.Verticies.GroupBy(p => p.Y).FirstOrDefault(g => g.Count() == 2);
            if (hLineY == null)
                throw new ArgumentException("Invalid verticies for layout (missing horizonal side)");
            var vLineX = verticies.Verticies.GroupBy(p => p.X).FirstOrDefault(g => g.Count() == 2);
            if (vLineX == null)
                throw new ArgumentException("Invalid verticies for layout (missing vertical side)");

            var vLineYs = verticies.Verticies.Where(p => p.X == vLineX.Key).Select(p => p.Y);
            if (vLineYs.Contains(hLineY.Key) == false)
                throw new ArgumentException("Invalid verticies for layout (vertical line diconnected)");
            var hLineXs = verticies.Verticies.Where(p => p.Y == hLineY.Key).Select(p => p.X);
            if (hLineXs.Contains(vLineX.Key) == false)
                throw new ArgumentException("Invalid verticies for layout (horizontal line disconnected)");

            var nonHorizonalPoint = verticies.Verticies.FirstOrDefault(p => p.Y != hLineY.Key);
            if (nonHorizonalPoint.X != (verticies.MinY == hLineY.Key ? verticies.MaxX : verticies.MinX))
                throw new ArgumentException("Invalid triangle for layout (non-horzontal vertex)");

            if (verticies.MinX % TRIANGLE_WIDTH != 0 || verticies.MaxX % TRIANGLE_WIDTH != 0)
                throw new ArgumentException("Invalid triangle location (horizontal location)");
            if (verticies.MinY % TRIANGLE_HEIGHT != 0 || verticies.MaxY % TRIANGLE_HEIGHT != 0)
                    throw new ArgumentException("Invalid verticies for layout (vertical location)");

            var row = (char)(verticies.MinY / TRIANGLE_HEIGHT + 'A');
            var column = (verticies.MinX / TRIANGLE_WIDTH + 1) * 2 - (verticies.MinY == hLineY.Key ? 0 : 1);

            return new TriangleLocation(row, column);
        }


        public class LayoutRange
        {
            public int Rows { get; set; }
            public int Columns { get; set; }

            public LayoutRange(int totalRows, int totalColumns)
            {
                SetRange(totalRows, totalColumns);
            }

            public void SetRange(int rows, int columns)
            {
                Rows = rows;
                Columns = columns;
            }

            public bool IsValid()
            {
                return Rows > 0 && Columns > 0;
            }

        }

    }

    public class TriangleLocation
    {
        public char Row { get; }
        public int Column { get; }
        public int RowIndex => Row - 'A';
        public int ColumnIndex => Column;

        public TriangleLocation(char row, int column)
        {
            Row = char.ToUpper(row);
            Column = column;
        }

        public static implicit operator LayoutLocation(TriangleLocation triangleLocation)
        {
            LayoutLocation location;
            location.RowLetter = $"{triangleLocation.Row}";
            location.ColumnNumber = triangleLocation.Column;
            return location;
        }

    }

    public class TriangleVerticies
    {
        public Point V1 { get; }
        public Point V2 { get; }
        public Point V3 { get; }

        public int MinX => Math.Min(V1.X, Math.Min(V2.X, V3.X));
        public int MinY => Math.Min(V1.Y, Math.Min(V2.Y, V3.Y));
        public int MaxX => Math.Max(V1.X, Math.Max(V2.X, V3.X));
        public int MaxY => Math.Max(V1.Y, Math.Max(V2.Y, V3.Y));

        public List<Point> Verticies => new[] {V1, V2, V3}.ToList();

        public TriangleVerticies(Point v1, Point v2, Point v3)
        {
            V1 = v1;
            V2 = v2;
            V3 = v3;
        }
    }

    /// <summary>
    /// An extension class for the between operation
    /// name pattern IsBetweenXX where X = I -> Inclusive, X = E -> Exclusive
    /// <a href="https://stackoverflow.com/a/13470099/37055"></a>
    /// </summary>
    public static class BetweenExtensions
    {
        public enum CompareOptions
        {
            InclusiveStartInclusiveEnd,
            ExclusiveStartExclusiveEnd,
            InclusiveStartExclusiveEnd,
            ExclusiveStartInclusiveEnd
        }

        /// <summary>
        /// Between check <![CDATA[min <= value <= max]]> 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">the value to check</param>
        /// <param name="min">Inclusive minimum border</param>
        /// <param name="max">Inclusive maximum border</param>
        /// <returns>return true if the value is between the min & max else false</returns>
        public static bool IsBetween<T>(this T value, T min, T max, CompareOptions options = CompareOptions.InclusiveStartInclusiveEnd) where T : IComparable<T>
        {
            switch (options)
            {
                case CompareOptions.InclusiveStartInclusiveEnd:
                    return IsBetweenII(value, min, max);
                case CompareOptions.ExclusiveStartInclusiveEnd:
                    return IsBetweenEI(value, min, max);
                case CompareOptions.InclusiveStartExclusiveEnd:
                    return IsBetweenIE(value, min, max);
                case CompareOptions.ExclusiveStartExclusiveEnd:
                    return IsBetweenEE(value, min, max);
                default:
                    return IsBetweenII(value, min, max);

            }
        }

        /// <summary>
        /// Between check <![CDATA[min <= value <= max]]> 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">the value to check</param>
        /// <param name="min">Inclusive minimum border</param>
        /// <param name="max">Inclusive maximum border</param>
        /// <returns>return true if the value is between the min & max else false</returns>
        private static bool IsBetweenII<T>(this T value, T min, T max) where T : IComparable<T>
        {
            return (min.CompareTo(value) <= 0) && (value.CompareTo(max) <= 0);
        }

        /// <summary>
        /// Between check <![CDATA[min < value <= max]]>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">the value to check</param>
        /// <param name="min">Exclusive minimum border</param>
        /// <param name="max">Inclusive maximum border</param>
        /// <returns>return true if the value is between the min & max else false</returns>
        private static bool IsBetweenEI<T>(this T value, T min, T max) where T : IComparable<T>
        {
            return (min.CompareTo(value) < 0) && (value.CompareTo(max) <= 0);
        }

        /// <summary>
        /// between check <![CDATA[min <= value < max]]>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">the value to check</param>
        /// <param name="min">Inclusive minimum border</param>
        /// <param name="max">Exclusive maximum border</param>
        /// <returns>return true if the value is between the min & max else false</returns>
        private static bool IsBetweenIE<T>(this T value, T min, T max) where T : IComparable<T>
        {
            return (min.CompareTo(value) <= 0) && (value.CompareTo(max) < 0);
        }

        /// <summary>
        /// between check <![CDATA[min < value < max]]>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">the value to check</param>
        /// <param name="min">Exclusive minimum border</param>
        /// <param name="max">Exclusive maximum border</param>
        /// <returns>return true if the value is between the min & max else false</returns>

        private static bool IsBetweenEE<T>(this T value, T min, T max) where T : IComparable<T>
        {
            return (min.CompareTo(value) < 0) && (value.CompareTo(max) < 0);
        }
    }
}