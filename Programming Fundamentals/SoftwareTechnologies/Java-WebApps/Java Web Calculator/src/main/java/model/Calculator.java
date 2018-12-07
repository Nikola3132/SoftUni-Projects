package model;

public class Calculator {
    private double leftOperand;
    private String operator;
    private double rightOperand;

    public Calculator(double leftOperand, String operator, double rightOperand) {
        this.leftOperand = leftOperand;
        this.operator = operator;
        this.rightOperand = rightOperand;
    }

    public double getLeftOperand() {
        return leftOperand;
    }

    public void setLeftOperand(double leftOperand) {
        this.leftOperand = leftOperand;
    }

    public String getOperator() {
        return operator;
    }

    public void setOperator(String operator) {
        this.operator = operator;
    }

    public double getRightOperand() {
        return rightOperand;
    }

    public void setRightOperand(double rightOperand) {
        this.rightOperand = rightOperand;
    }

    public double calculateResult(){
        double result = 0.0;
        String operator = this.operator;

        if (operator.equals("+")){
            result = this.leftOperand + this.rightOperand;
        }
        if (operator.equals("-")){
            result = this.leftOperand - this.rightOperand;
        }
        if (operator.equals("/")){
            result = this.leftOperand / this.rightOperand;
        }
        if (operator.equals("*")){
            result = this.leftOperand * this.rightOperand;
        }
        if (operator.equals("^")){
            result = Math.pow(this.leftOperand,this.rightOperand);
        }

        return result;
    }
}
