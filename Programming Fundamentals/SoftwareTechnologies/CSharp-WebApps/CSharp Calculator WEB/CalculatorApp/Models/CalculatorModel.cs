namespace CalculatorApp.Models
{
    public class CalculatorModel
    {
        public CalculatorModel()
        {
            this.Result = 0;
        }

        public decimal LeftOperand { get; set; }
        public string Operator { get; set; }
        public decimal RightOperand { get; set; }
        public decimal Result { get; set; }

        public void CalculateResult()
        {
            decimal result = 0;
            switch (this.Operator)
            {
                case "+":
                    result = this.LeftOperand + this.RightOperand;
                    break;
                case "-":
                    result = this.LeftOperand - this.RightOperand;
                    break;
                case "/":
                    result = this.LeftOperand / this.RightOperand;
                    break;
                case "*":
                    result = this.LeftOperand * this.RightOperand;
                    break;
            }
            this.Result = result;
        }
    }
}
