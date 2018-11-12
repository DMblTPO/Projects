using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace MyUnitTests.CodeWars
{
    public class SolutionTest
    {

        [Test]
        public void TestBasic()
        {

            string a = ".W.\n" +
                       ".W.\n" +
                       "...",

                b = ".W.\n" +
                    ".W.\n" +
                    "W..",

                c = "......\n" +
                    "......\n" +
                    "......\n" +
                    "......\n" +
                    "......\n" +
                    "......",

                d = "......\n" +
                    "......\n" +
                    "......\n" +
                    "......\n" +
                    ".....W\n" +
                    "....W.";

            Assert.AreEqual(4, Finder.PathFinder(a));
            Assert.AreEqual(-1, Finder.PathFinder(b));
            Assert.AreEqual(10, Finder.PathFinder(c));
            Assert.AreEqual(-1, Finder.PathFinder(d));
        }
    }

    public class Finder
    {
        public static int PathFinder(string mazeTxt)
        {
            var maze = new Maze(mazeTxt);

            while (maze.Current == maze.Finish)
            {
                var pos = maze.GoEast() ?? maze.GoSouth() ?? maze.GoWest() ?? maze.GoWest();

            }

            return 4;
        }

    }

    public class Maze
    {
        private int _edge;
        private string[] _mazeMap;
        private readonly Position _start;
        private readonly Position _finish;
        private Position _cur;

        public Maze(string maze)
        {
            _mazeMap = maze.Split(new[] {'\n'});
            _edge = _mazeMap.Length - 1;
            _start = new Position(0, 0);
            _cur = new Position(0, 0);
            _finish = new Position(_edge, _edge);
        }

        public Position Start => _start;
        public Position Finish => _finish;
        public Position Current => _cur;

        public Position GoNorth()
            => Validate(_cur.X-1, _cur.Y) ? _cur = new Position(_cur.X-1, _cur.Y) : null;

        public Position GoSouth()
            => Validate(_cur.X+1, _cur.Y) ? _cur = new Position(_cur.X+1, _cur.Y) : null;

        public Position GoWest()
            => Validate(_cur.X, _cur.Y-1) ? _cur = new Position(_cur.X, _cur.Y-1) : null;

        public Position GoEast()
            => Validate(_cur.X, _cur.Y+1) ? _cur = new Position(_cur.X, _cur.Y+1) : null;

        private bool Validate(int x, int y) => x >= 0 && y >= 0 && x <= _edge && y <= _edge;

        public class Position
        {
            public Position(int x, int y)
            {
                X = x;
                Y = y;
            }
            public int X { get; }
            public int Y { get; }
            public (int x, int y) XY => (X, Y);

            public override bool Equals(object obj)
            {
                if (obj is Position p)
                {
                    return p == this;
                }
                return base.Equals(obj);
            }

            public static bool operator ==(Position rp, Position lp) => rp != null && lp != null && rp.Y == lp.Y && rp.X == lp.X;
            public static bool operator !=(Position rp, Position lp) => rp != null && lp != null && (rp.Y != lp.Y || rp.X != lp.X);
        }

        public enum Direction
        {
            North, East, South, West
        }
    }

}