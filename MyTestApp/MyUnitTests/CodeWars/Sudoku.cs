﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace MyUnitTests.CodeWars
{
    using System.Linq;

    public class Sudoku
    {
        public static bool ValidateSolution(int[][] board)
        {
            int CalcKey(int i) => i <= 2 ? 1 : i > 2 && i < 6 ? 2 : 3;

            bool CheckSudoku(int[][] b)
            {
                foreach (var row in b)
                {
                    if (row.Where(x => x >= 1 && x <= 9).Sum() != 45)
                    {
                        return false;
                    }

                    if (row.GroupBy(x => x).Select(x => new {x.Key, Qty = x.Count()}).Any(x => x.Qty > 1))
                    {
                        return false;
                    }
                }
                return true;
            }

            var bb = board.Select(x => new int[9]).ToArray();

            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    bb[i][j] = board[j][i];
                }
            }

            return CheckSudoku(board) && CheckSudoku(bb);
        }
    }
}

namespace MyUnitTests.CodeWars 
{
    using NUnit.Framework;

    [TestFixture]
    public class Sudoku_Sample_Tests
    {
        private static object[] testCases = new object[]
        {
            new object[]
            {
                true,
                new []
                {
                    new [] {5, 3, 4, 6, 7, 8, 9, 1, 2}, 
                    new [] {6, 7, 2, 1, 9, 5, 3, 4, 8},
                    new [] {1, 9, 8, 3, 4, 2, 5, 6, 7},
                    new [] {8, 5, 9, 7, 6, 1, 4, 2, 3},
                    new [] {4, 2, 6, 8, 5, 3, 7, 9, 1},
                    new [] {7, 1, 3, 9, 2, 4, 8, 5, 6},
                    new [] {9, 6, 1, 5, 3, 7, 2, 8, 4},
                    new [] {2, 8, 7, 4, 1, 9, 6, 3, 5},
                    new [] {3, 4, 5, 2, 8, 6, 1, 7, 9},
                },
            },
            new object[]
            {
                false,
                new []
                {
                    new[] {5, 3, 4, 6, 7, 8, 9, 1, 2}, 
                    new[] {6, 7, 2, 1, 9, 5, 3, 4, 8},
                    new[] {1, 9, 8, 3, 0, 2, 5, 6, 7},
                    new[] {8, 5, 0, 7, 6, 1, 4, 2, 3},
                    new[] {4, 2, 6, 8, 5, 3, 7, 9, 1},
                    new[] {7, 0, 3, 9, 2, 4, 8, 5, 6},
                    new[] {9, 6, 1, 5, 3, 7, 2, 8, 4},
                    new[] {2, 8, 7, 4, 1, 9, 6, 3, 5},
                    new[] {3, 0, 0, 2, 8, 6, 1, 7, 9},
                },
            },
            new object[]
            {
                false,
                new []
                {
                    new [] {1, 2, 3, 4, 5, 6, 7, 8, 9}, 
                    new [] {2, 3, 1, 5, 6, 4, 8, 9, 7},
                    new [] {3, 1, 2, 6, 4, 5, 9, 7, 8},
                    new [] {4, 5, 6, 7, 8, 9, 1, 2, 3},
                    new [] {5, 6, 4, 8, 9, 7, 2, 3, 1},
                    new [] {6, 4, 5, 9, 7, 8, 3, 1, 2},
                    new [] {7, 8, 9, 1, 2, 3, 4, 5, 6},
                    new [] {8, 9, 7, 2, 3, 1, 5, 6, 4},
                    new [] {9, 7, 8, 3, 1, 2, 6, 4, 5},
                },
            },
        };
  
        [Test, TestCaseSource("testCases")]
        public void Test(bool expected, int[][] board) => Assert.AreEqual(expected, Sudoku.ValidateSolution(board));
    }
}