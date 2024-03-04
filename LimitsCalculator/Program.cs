using System;
using System.Collections.Generic;

public enum TokenType
{
    Number,
    Variable,
    Plus,
    Minus,
    Multiply,
    Divide,
    Power,
    LParen,
    RParen
}

public struct Token
{
    public TokenType Type { get; set; }
    public string Value { get; set; }
}

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

// 2*x^2 - 5*x + 3
//3*x - 3

public class LimitSolver
{
    public double SolveLimit(Func<double, double> f, Func<double, double> g, double x)
    {
        // Рассчитываем предел f(x) / g(x) при x стремящемся к заданному значению
        double fLimit = f(x);
        double gLimit = g(x);

        // Вместо проверки на знаменатель равный нулю, просто применяем правило Лопиталя
        // если оба предела равны 0 или бесконечности, это будет обработано в методе SolveLimitLHopital
        return SolveLimitLHopital(f, g, x);
    }


    public double SolveLimitLHopital(Func<double, double> f, Func<double, double> g, double x)
    {
        // Рассчитываем предел f(x) / g(x) при x стремящемся к заданному значению
        double fLimit = f(x);
        double gLimit = g(x);

        // Проверяем условия применимости правила Лопиталя
        if (Math.Abs(fLimit) < double.Epsilon && Math.Abs(gLimit) < double.Epsilon)
        {
            // Оба предела равны 0, применяем правило Лопиталя
            double fDerivative = Derivative(f, x);
            double gDerivative = Derivative(g, x);
            return fDerivative / gDerivative;
        }
        else if (double.IsInfinity(fLimit) && double.IsInfinity(gLimit))
        {
            // Оба предела стремятся к бесконечности, применяем правило Лопиталя
            double fDerivative = Derivative(f, x);
            double gDerivative = Derivative(g, x);
            return fDerivative / gDerivative;
        }
        else
        {
            // Предел не является неопределенностью 0/0 или бесконечность/бесконечность,
            // поэтому нельзя применить правило Лопиталя
            throw new InvalidOperationException("Предел не является неопределенностью 0/0 или бесконечность/бесконечность");
        }
    }


    private double Derivative(Func<double, double> func, double x)
    {
        const double delta = 1e-6; // Определяем шаг для численного дифференцирования
        return (func(x + delta) - func(x)) / delta;
    }
}

public class Fraction
{
    public int Numerator { get; }
    public int Denominator { get; }

    public Fraction(int numerator, int denominator)
    {
        int gcd = GreatestCommonDivisor(numerator, denominator);
        Numerator = numerator / gcd;
        Denominator = denominator / gcd;
    }

    public override string ToString()
    {
        return Denominator == 1 ? Numerator.ToString() : $"{Numerator}/{Denominator}";
    }

    private int GreatestCommonDivisor(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }
}



class Program
{
    static void Main()
    {
        Console.WriteLine("Введите выражение для f(x) с использованием символа 'x' для обозначения переменной:");
        string expressionF = Console.ReadLine();

        Console.WriteLine("Введите выражение для g(x) с использованием символа 'x' для обозначения переменной:");
        string expressionG = Console.ReadLine();

        Console.WriteLine("Введите значение x, к которому стремится предел (для бесконечности введите 'inf'):");
        string xInput = Console.ReadLine();

        double x;
        if (xInput == "inf")
        {
            x = double.PositiveInfinity; // Или double.NegativeInfinity, в зависимости от направления стремления
        }
        else
        {
            x = Convert.ToDouble(xInput);
        }

        Parser parserF = new Parser(expressionF);
        List<Token> tokensF = parserF.Parse();

        Parser parserG = new Parser(expressionG);
        List<Token> tokensG = parserG.Parse();

        // Создаем функции для f(x) и g(x) на основе введенных пользователем выражений
        Func<double, double> f = (val) => EvaluateExpression(tokensF, val);
        Func<double, double> g = (val) => EvaluateExpression(tokensG, val);

        // Создаем экземпляр класса для решения пределов
        LimitSolver limitSolver = new LimitSolver();

        try
        {
            double limitLHopital = limitSolver.SolveLimitLHopital(f, g, x);
            Fraction fractionResult = ConvertToFraction(limitLHopital);
            Console.WriteLine($"Предел f(x) / g(x) с применением правила Лопиталя при x -> {xInput} равен:");
            Console.WriteLine($"Десятичная дробь: {limitLHopital}");
            Console.WriteLine($"Обычная дробь: {fractionResult}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка при вычислении предела с применением правила Лопиталя: {ex.Message}");
        }



    }
    static Fraction ConvertToFraction(double decimalValue)
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



    static double EvaluateExpression(List<Token> tokens, double x)
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

    static bool IsOperator(TokenType type)
    {
        return type == TokenType.Plus || type == TokenType.Minus || type == TokenType.Multiply ||
               type == TokenType.Divide || type == TokenType.Power;
    }

    static int Precedence(TokenType type)
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

    static double ApplyOperation(TokenType op, double b, double a)
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
