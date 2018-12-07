using SimpleSnake.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SimpleSnake.GameObjects
{
    public class Engine
    {
        public Engine(Wall wall, Snake snake)
        {
            this.wall = wall;
            this.snake = snake;
            this.sleepTime = 100;
            this.pointsOfDirections = new Point[4];
        }

        private Point[] pointsOfDirections; 
        private Direction direction;
        Snake snake;
        Wall wall;
        private double sleepTime;
        private void CreateDirections()
        {
            this.pointsOfDirections[0] = new Point(1, 0);
            this.pointsOfDirections[1] = new Point(-1, 0);
            this.pointsOfDirections[2] = new Point(0, 1);
            this.pointsOfDirections[3] = new Point(0, -1);
        }

        private void GetNextDirection()
        {
            ConsoleKeyInfo userInput = Console.ReadKey();

            if (userInput.Key == ConsoleKey.LeftArrow)
            {
                if (direction != Direction.Right)
                {
                    direction = Direction.Left;
                }
            }
            else if (userInput.Key == ConsoleKey.RightArrow)
            {
                if (direction != Direction.Left)
                {
                    direction = Direction.Right;
                }
            }
            else if (userInput.Key == ConsoleKey.DownArrow)
            {
                if (direction != Direction.Up)
                {
                    direction = Direction.Down;
                }
            }
            else if (userInput.Key == ConsoleKey.UpArrow)
            {
                if (direction != Direction.Down)
                {
                    direction = Direction.Up;
                }
            }

            Console.CursorVisible = false;

            bool isMoving = snake.IsMoving(this.pointsOfDirections[(int)direction]);
        }

        private void AskUserForRestart()
        {
            int leftX = this.wall.LeftX + 1;
            int topY = 3;

            Console.SetCursorPosition(leftX, topY);
            Console.Write("Would you like to continue? y/n.");
            string input = Console.ReadLine();

            if (input == "y")
            {
                Console.Clear();
                StartUp.Main();
            }
            else 
            {
                StopGame();
            }
        } 
        
        private void StopGame()
        {
            Console.SetCursorPosition(20, 10);
            Console.Write("Game Over!");
            Environment.Exit(0);
        }

        public void Run()
        {
            this.CreateDirections();

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    GetNextDirection();
                }

                bool isMoving = snake.IsMoving(this.pointsOfDirections[(int)direction]);

                if (!isMoving)
                {
                    AskUserForRestart();
                }
                this.sleepTime -= 0.01;

                Thread.Sleep((int)sleepTime);
                
            }
        }
    }
}
