
using ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Sudoku
{
    class SudokuPuzzleValidator
    {
        static void Main(string[] args)
        {
            int[][] goodSudoku1 = {
                new int[] {7,8,4,  1,5,9,  3,2,6},
                new int[] {5,3,9,  6,7,2,  8,4,1},
                new int[] {6,1,2,  4,3,8,  7,5,9},

                new int[] {9,2,8,  7,1,5,  4,6,3},
                new int[] {3,5,7,  8,4,6,  1,9,2},
                new int[] {4,6,1,  9,2,3,  5,8,7},

                new int[] {8,7,6,  3,9,4,  2,1,5},
                new int[] {2,4,3,  5,6,1,  9,7,8},
                new int[] {1,9,5,  2,8,7,  6,3,4}
            };


            int[][] goodSudoku2 = {
                new int[] {1,4, 2,3},
                new int[] {3,2, 4,1},

                new int[] {4,1, 3,2},
                new int[] {2,3, 1,4}
            };

            int[][] badSudoku1 =  {
                new int[] {1,2,3, 4,5,6, 7,8,9},
                new int[] {1,2,3, 4,5,6, 7,8,9},
                new int[] {1,2,3, 4,5,6, 7,8,9},

                new int[] {1,2,3, 4,5,6, 7,8,9},
                new int[] {1,2,3, 4,5,6, 7,8,9},
                new int[] {1,2,3, 4,5,6, 7,8,9},

                new int[] {1,2,3, 4,5,6, 7,8,9},
                new int[] {1,2,3, 4,5,6, 7,8,9},
                new int[] {1,2,3, 4,5,6, 7,8,9}
            };

            int[][] badSudoku2 = {
                new int[] {1,3,2,4,5},
                new int[] {2,4,3,1},
                new int[] {3,2,3,4},
                new int[] {4}
            };

            Debug.Assert(ValidateSudoku(goodSudoku1), "This is supposed to validate! It's a good sudoku!");
            Debug.Assert(ValidateSudoku(goodSudoku2), "This is supposed to validate! It's a good sudoku!");
            Debug.Assert(!ValidateSudoku(badSudoku1), "This isn't supposed to validate! It's a bad sudoku!");
            Debug.Assert(!ValidateSudoku(badSudoku2), "This isn't supposed to validate! It's a bad sudoku!");
        }

        static bool ValidateSudoku(int[][] puzzle)
        {
            return new SudokuExaminer(puzzle).IsValid();
        }
    }

    public class SudokuExaminer
    {
        private int[][] _sudokuBoard;

        private int _sudokuBoardSize;
        private int _miniMeSize;

        private bool _miniBoard;
        public SudokuExaminer(int[][] sudoku, bool miniBoard = false)
        {
            _sudokuBoard = sudoku;
            _sudokuBoardSize = sudoku == null ? 0 : sudoku.Length;

            int sqrt = (int)Math.Sqrt(_sudokuBoardSize);
            _miniMeSize = Math.Pow(sqrt, 2) == _sudokuBoardSize ? sqrt : _sudokuBoardSize;

            _miniBoard = miniBoard;
        }

        public bool IsValid()
        {
            try
            {
                if (_sudokuBoardSize == 0)
                    return false;

                if (_miniBoard && IsSquareValid() == false)
                    return false;

                for (int i = 0; i < _sudokuBoardSize; i++)
                {
                    if ((IsRowValid(i) && IsColumnValid(i)) == false)
                        return false;

                    if (IsMiniMeValid(i) == false)
                        return false;
                }

                return true;
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Jagged Array");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private bool IsMiniMeValid(int pos)
        {
            if (_miniMeSize != _sudokuBoardSize && pos % _miniMeSize == 0)
            {
                if (!new SudokuExaminer(_sudokuBoard.Slice(pos, pos, _miniMeSize), true).IsValid())
                    return false;

                for (int k = pos + _miniMeSize; k < _sudokuBoardSize; k += _miniMeSize)
                {
                    if (!new SudokuExaminer(_sudokuBoard.Slice(pos, k, _miniMeSize), true).IsValid())
                        return false;


                    if (!new SudokuExaminer(_sudokuBoard.Slice(k, pos, _miniMeSize), true).IsValid())
                        return false;
                }
            }

            return true;
        }

        private bool IsRowValid(int pos) => IsSudokuRowColumnValid(GetRow(pos));
        private bool IsColumnValid(int pos) => IsSudokuRowColumnValid(GetColumn(pos));
        private bool IsSquareValid() => IsSudokuSquareValid(GetSquare());

        private bool IsSudokuRowColumnValid(IEnumerable<int> enumerable) => enumerable.Count() == _sudokuBoardSize && new HashSet<int>(enumerable).Count == _sudokuBoardSize;
        private bool IsSudokuSquareValid(IEnumerable<int> enumerable) => Math.Pow(_sudokuBoardSize, 2) == new HashSet<int>(enumerable).Count;

        private IEnumerable<int> GetRow(int pos) => _sudokuBoard[pos];
        private IEnumerable<int> GetColumn(int pos) => Enumerable.Range(0, _sudokuBoardSize).Select(it => _sudokuBoard[it][pos]);
        private IEnumerable<int> GetSquare() => Enumerable.Range(0, _sudokuBoardSize).Select(it => _sudokuBoard[it]).SelectMany(i => i);
    }
}