namespace TheTankGame.Entities.Parts
{
    using System;
    using Contracts;
    using TheTankGame.Utils;

    public abstract class BasePart : IPart
    {
        private string model;
        private double weight;
        private decimal price;

        protected BasePart(string model, double weight, decimal price)
        {
            this.Model = model;
            this.Weight = weight;
            this.Price = price;
        }

        public string Model
        {
            get
            {
                return this.model;
            }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(GlobalConstants.ModelCannotBeNull);
                }

                this.model = value;
            }
        }

        public double Weight
        {
            get
            {
                return this.weight;
            }
            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException(GlobalConstants.WeightCannotBeLessThanZero);
                }

                this.weight = value;
            }
        }

        public decimal Price
        {
            get
            {
                return this.price;
            }
            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException(GlobalConstants.PriceCannotBeLessThanZero);
                }

                this.price = value;
            }
        }

        public override string ToString()
        {
            string partName = this.GetType().Name.Replace("Part", "");

            return $"{partName} Part - {this.Model}";
        }
    }
}