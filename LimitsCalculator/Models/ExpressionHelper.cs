using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LimitsCalculator.Models
{
    public static class ExpressionHelper
    {
        public static Fraction ConvertToFraction(double decimalValue)
        {
            const double tolerance = 0.0001;
            int wholePart = (int)decimalValue;
            double fractionPart = decimalValue - wholePart;
            int maxDenominator = 10000;

            for (int i = 2; i < maxDenominator; i++)
            {
                if (Math.Abs(fractionPart * i - Math.Round(fractionPart * i)) < tolerance)
                {
                    int numerator = (int)Math.Round(fractionPart * i);
                    return new Fraction(wholePart * i + numerator, i);
                }
            }

            return new Fraction(wholePart, 1);
        }



        public static double EvaluateExpression(List<Token> tokens, double x)
        {
            Stack<double> values = new Stack<double>();
            Stack<TokenType> operators = new Stack<TokenType>();

            foreach (var token in tokens)
            {
                if (token.Type == TokenType.Number)
                {
                    values.Push(double.Parse(token.Value));
                }
                else if (token.Type == TokenType.Variable)
                {
                    values.Push(x);
                }
                else if (IsOperator(token.Type))
                {
                    while (operators.Count > 0 && Precedence(token.Type) <= Precedence(operators.Peek()))
                    {
                        values.Push(ApplyOperation(operators.Pop(), values.Pop(), values.Pop()));
                    }
                    operators.Push(token.Type);
                }
                else if (token.Type == TokenType.LParen)
                {
                    operators.Push(token.Type);
                }
                else if (token.Type == TokenType.RParen)
                {
                    while (operators.Peek() != TokenType.LParen)
                    {
                        values.Push(ApplyOperation(operators.Pop(), values.Pop(), values.Pop()));
                    }
                    operators.Pop(); // Discard the left parenthesis
                }
            }

            while (operators.Count > 0)
            {
                values.Push(ApplyOperation(operators.Pop(), values.Pop(), values.Pop()));
            }

            return values.Pop();
        }

        public static bool IsOperator(TokenType type)
        {
            return type == TokenType.Plus || type == TokenType.Minus || type == TokenType.Multiply ||
                   type == TokenType.Divide || type == TokenType.Power;
        }

        public static int Precedence(TokenType type)
        {
            switch (type)
            {
                case TokenType.Power:
                    return 3;
                case TokenType.Multiply:
                case TokenType.Divide:
                    return 2;
                case TokenType.Plus:
                case TokenType.Minus:
                    return 1;
                default:
                    return 0;
            }
        }

        public static double ApplyOperation(TokenType op, double b, double a)
        {
            switch (op)
            {
                case TokenType.Plus:
                    return a + b;
                case TokenType.Minus:
                    return a - b;
                case TokenType.Multiply:
                    return a * b;
                case TokenType.Divide:
                    if (Math.Abs(b) < double.Epsilon)
                        throw new DivideByZeroException("Division by zero");
                    return a / b;
                case TokenType.Power:
                    return Math.Pow(a, b);
                default:
                    throw new ArgumentException("Invalid operator");
            }
        }
    }
}
