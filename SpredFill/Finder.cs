using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace SpredFill
{
    internal static class Finder
    {
        static int SizeX, SizeY;
        static int AcceptedLevel = 1;

        static int[,] space;
        static bool[,] tested;
        static bool[,] scaned_points;


        public static List<Point> center = new List<Point>();
        public static List<List<Point>> blop_points = new List<List<Point>>();



        public static void FindAllBlops(int[,] _space, int _SizeX, int _SizeY, int _AcceptedLevel)
        {
            SizeX = _SizeX;
            SizeY = _SizeY;
            tested = new bool[SizeX, SizeY];
            scaned_points = new bool[SizeX, SizeY];

            space = _space;
            AcceptedLevel = _AcceptedLevel;


            //Scan the space to find interesting pixel with value equal or higher than AcceptedLevel
            for (int i = 1; i < SizeX; ++i)
            {
                for (int j = 1; j < SizeY; ++j)
                {
                    var p = new Point(i, j);
                    if (TestPoint(p))
                        center.Add(FindBlop(p));
                }
            }
        }

        //Find all points in blop
        static Point FindBlop(Point point)
        {
            var blop_points = new List<Point>
            {
                point //The first found point
            };

            //find the rest of points in the blop (the heart of the algorithm) Count increases in the loop:)
            for (int i = 0; i < blop_points.Count; i++)
                if (scaned_points[blop_points[i].X, blop_points[i].Y] == false)
                    blop_points.AddRange(ScanNeighburst(blop_points[i]));
                

            //All blops with they points
            Finder.blop_points.Add(blop_points);

            //Return center of the blop
            return Average(blop_points);
        }

        static Point Average(List<Point> blop_points)
        {
            var ave = new Point();

            if (blop_points.Count > 0)
            {
                for (int i = 0; i < blop_points.Count; ++i)
                    ave += blop_points[i];

                ave.X /= blop_points.Count;
                ave.Y /= blop_points.Count;
            }
            return ave;
        }


        //Scan all neighbours of the found point
        static List<Point> ScanNeighburst(Point point)
        {
            Point neighbour;

            //Set flags for the investigated point
            tested[point.X, point.Y] = scaned_points[point.X, point.Y] = true;

            List<Point> surrounded_points = new List<Point>();

            //Test all surrounded neighbour points
            for (int x = -1; x < 2; x++)
                for (int y = -1; y < 2; y++)
                {
                    neighbour = point + new Point(x, y);
                    if (TestPoint(neighbour))
                        surrounded_points.Add(neighbour);
                }

            return surrounded_points;
        }

        static bool TestPoint(Point point)
        {
            //if (point.X > 0 && point.Y > 0) 
            if (point.X < SizeX && point.Y < SizeY)
                if (tested[point.X, point.Y] == false && space[point.X, point.Y] >= AcceptedLevel)
                {
                    return true;
                }
            return false;
        }

    }
}
