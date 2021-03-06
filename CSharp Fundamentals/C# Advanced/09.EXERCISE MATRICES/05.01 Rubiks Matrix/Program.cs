﻿using System;
using System.Linq;
    public class RubiksMatrix
    {
        public static void Main() // 100/100
        {
            int[][] matrix = GetMatrix();
            int[][] modifiedMatrix = RotateMatrix(matrix);
            SwapModifiedMatrixToOriginal(modifiedMatrix, matrix);
            //PrintMatrix(modifiedMatrix);
        }

        private static void SwapModifiedMatrixToOriginal(int[][] modifiedMatrix, int[][] matrix)
        {
            int rows = matrix.Length;
            int cols = matrix[0].Length;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    if (modifiedMatrix[row][col] == matrix[row][col])
                    {
                        Console.WriteLine("No swap required");
                        continue;
                    }
                    // swap elements
                    for (int searchRow = row; searchRow < rows; searchRow++)
                    {
                        bool isMatch = false;
                        for (int searchCol = 0; searchCol < cols; searchCol++)
                        {
                            if (modifiedMatrix[searchRow][searchCol] == matrix[row][col])
                            {
                                modifiedMatrix[searchRow][searchCol] = modifiedMatrix[row][col];
                                modifiedMatrix[row][col] = matrix[row][col];
                                Console.WriteLine($"Swap ({row}, {col}) with ({searchRow}, {searchCol})");
                                isMatch = true;
                                break;
                            }
                        }
                        if (isMatch) break;
                    }
                }
            }
        }

        private static int[][] CopyMatrix(int[][] matrix)
        {
            int rows = matrix.Length;
          
            int[][] matrixCopy = new int[rows][];
            for (int row = 0; row < rows; row++)
            {
                matrixCopy[row] = matrix[row].ToArray();
            }
            return matrixCopy;
        }

        private static int[][] RotateMatrix(int[][] matrix)
        {
            int commandsCount = int.Parse(Console.ReadLine());
            int[][] modifiedMatrix = CopyMatrix(matrix);

            for (int command = 0; command < commandsCount; command++)
            {
                string[] args = Console.ReadLine()
                          .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                          .ToArray();
                int rotatedRowCol = int.Parse(args[0]);
                string direction = args[1];
                int movesCount = int.Parse(args[2]);

                if (direction == "up" || direction == "down")
                {
                    modifiedMatrix = RotateMatrixCol(modifiedMatrix, rotatedRowCol, direction, movesCount);
                }
                else if (direction == "left" || direction == "right")
                {
                    modifiedMatrix = RotateMatrixRow(modifiedMatrix, rotatedRowCol, direction, movesCount);
                }
            }
            return modifiedMatrix;
        }

        private static int[][] RotateMatrixRow(int[][] matrix, int rotatedRow, string direction, int movesCount)
        {
            int[][] modifiedMatrix = CopyMatrix(matrix);
            int rows = matrix.Length;
            int cols = matrix[0].Length;

            // direction RIGHT => increase COL index, direction LEFT => decrease COL index
            // keep ROW index upchanged
            movesCount %= rows;
            if (direction == "left")
            {
                movesCount = -movesCount;
            }

            for (int col = 0; col < rows; col++)
            {
                int modifiedCol = (col + movesCount) % cols;
                while (modifiedCol < 0)
                {
                    modifiedCol += cols;
                }
                modifiedMatrix[rotatedRow][modifiedCol] = matrix[rotatedRow][col];
            }
            return modifiedMatrix;
        }

        private static int[][] RotateMatrixCol(int[][] matrix, int rotatedCol, string direction, int movesCount)
        {
            int[][] modifiedMatrix = CopyMatrix(matrix);
            int rows = matrix.Length;

            // direction DOWN => increase ROW index, direction UP => decrease ROW index
            // keep COL index unchanged
            movesCount %= rows;
            if (direction == "up")
            {
                movesCount = -movesCount;
            }

            for (int row = 0; row < rows; row++)
            {
                int modifiedRow = (row + movesCount) % rows;
                while (modifiedRow < 0)
                {
                    modifiedRow += rows;
                }
                modifiedMatrix[modifiedRow][rotatedCol] = matrix[row][rotatedCol];
            }
            return modifiedMatrix;
        }

        public static void PrintMatrix(int[][] matrix)
        {
            foreach (int[] row in matrix)
            {
                Console.WriteLine(string.Join(" ", row));
            }
        }

        private static int[][] GetMatrix()
        {
            int[] size = Console.ReadLine().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                      .Select(int.Parse).ToArray();

            int rows = size[0];
            int cols = size[1];
            int[][] matrix = new int[rows][];
            int number = 1;
            for (int row = 0; row < rows; row++)
            {
                matrix[row] = new int[cols];
                for (int col = 0; col < cols; col++)
                {
                    matrix[row][col] = number++;
                }
            }
            return matrix;
        }
    }