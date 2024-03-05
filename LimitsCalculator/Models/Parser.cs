using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LimitsCalculator.Models
{
    public class Parser
    {
        private readonly string expression;
        private int position;
        private readonly List<Token> tokens;

        public Parser(string expression)
        {
            this.expression = expression.Replace(" ", ""); // Удаляем пробелы из выражения
            this.position = 0;
            this.tokens = new List<Token>();
        }

        public List<Token> Parse()
        {
            while (position < expression.Length)
            {
                char currentChar = expression[position];
                if (char.IsDigit(currentChar))
                {
                    tokens.Add(ScanNumber());
                }
                else if (char.IsLetter(currentChar))
                {
                    tokens.Add(ScanVariable());
                }
                else if (currentChar == '+')
                {
                    tokens.Add(new Token { Type = TokenType.Plus, Value = "+" });
                    position++;
                }
                else if (currentChar == '-')
                {
                    tokens.Add(new Token { Type = TokenType.Minus, Value = "-" });
                    position++;
                }
                else if (currentChar == '*')
                {
                    tokens.Add(new Token { Type = TokenType.Multiply, Value = "*" });
                    position++;
                }
                else if (currentChar == '/')
                {
                    tokens.Add(new Token { Type = TokenType.Divide, Value = "/" });
                    position++;
                }
                else if (currentChar == '^')
                {
                    tokens.Add(new Token { Type = TokenType.Power, Value = "^" });
                    position++;
                }
                else if (currentChar == '(')
                {
                    tokens.Add(new Token { Type = TokenType.LParen, Value = "(" });
                    position++;
                }
                else if (currentChar == ')')
                {
                    tokens.Add(new Token { Type = TokenType.RParen, Value = ")" });
                    position++;
                }
                else
                {
                    throw new Exception($"Unexpected character: {currentChar}");
                }
            }

            return tokens;
        }

        public double ParseX(string xInput)
        {
            if (xInput.Equals("inf", StringComparison.OrdinalIgnoreCase))
            {
                return double.PositiveInfinity; // или double.NegativeInfinity, в зависимости от вашей логики
            }
            else
            {
                return Convert.ToDouble(xInput);
            }
        }


        private Token ScanNumber()
        {
            int startPos = position;
            while (position < expression.Length && (char.IsDigit(expression[position]) || expression[position] == '.'))
            {
                position++;
            }
            return new Token { Type = TokenType.Number, Value = expression.Substring(startPos, position - startPos) };
        }

        private Token ScanVariable()
        {
            int startPos = position;
            while (position < expression.Length && char.IsLetter(expression[position]))
            {
                position++;
            }
            return new Token { Type = TokenType.Variable, Value = expression.Substring(startPos, position - startPos) };
        }
    }
}
