﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleSnake.GameObjects
{
    public abstract class Food : Point
    {
        private Wall wall;
        private char foodSymbol;
        private Random random;

        public Food(Wall wall, char foodSymbol, int points)
            : base(wall.LeftX, wall.TopY)
        {
            this.wall = wall;
            this.foodSymbol = foodSymbol;
            this.FoodPoints = points;
            this.random = new Random();
        }

        public int FoodPoints { get; private set; }

        public void SetRandomPosition(Queue<Point> snakeElements)
        {
            this.LeftX = this.random.Next(2, this.wall.LeftX - 2);
            this.TopY = this.random.Next(2, this.wall.TopY - 2);

            bool isPointOfSnake = snakeElements.Any(x => x.TopY == this.TopY && x.LeftX == this.LeftX);

            while (isPointOfSnake)
            {
                this.LeftX = this.random.Next(2, this.wall.LeftX - 2);
                this.TopY = this.random.Next(2, this.wall.TopY - 2);

                isPointOfSnake = snakeElements.Any(x => x.TopY == this.TopY && x.LeftX == this.LeftX);
            }
            if (foodSymbol == '#')
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                this.Draw(foodSymbol);
                Console.BackgroundColor = ConsoleColor.White;
            }
            else if (foodSymbol == '$')
            {
                Console.BackgroundColor = ConsoleColor.Green;
                this.Draw(foodSymbol);
                Console.BackgroundColor = ConsoleColor.White;
            }
            else if (foodSymbol == '*')
            {
                Console.BackgroundColor = ConsoleColor.Red;
                this.Draw(foodSymbol);
                Console.BackgroundColor = ConsoleColor.White;
            }
        }

        public bool IsFoodPoint(Point snake)
        {
            return this.LeftX == snake.LeftX && this.TopY == snake.TopY;
        }
    }
}
