using System;
using System.Collections.Generic;
using LimitsCalculator.Models;

// 2*x^2 - 5*x + 3
//3*x - 3

// 2*x^2 - 6*x^3 - 4
// 3*x - 9

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

        Parser parserF = new Parser(expressionF);
        List<Token> tokensF = parserF.Parse();

        Parser parserG = new Parser(expressionG);
        List<Token> tokensG = parserG.Parse();

        double x = parserF.ParseX(xInput);

        // Создаем функции для f(x) и g(x) на основе введенных пользователем выражений
        Func<double, double> f = (val) => ExpressionHelper.EvaluateExpression(tokensF, val);
        Func<double, double> g = (val) => ExpressionHelper.EvaluateExpression(tokensG, val);

        // Создаем экземпляр класса для решения пределов
        LimitSolver limitSolver = new LimitSolver();

        try
        {
            double limitLHopital = limitSolver.SolveLimitLHopital(f, g, x);
            Fraction fractionResult = ExpressionHelper.ConvertToFraction(limitLHopital);
            Console.WriteLine($"Предел f(x) / g(x) с применением правила Лопиталя при x -> {xInput} равен:");
            Console.WriteLine($"Десятичная дробь: {limitLHopital}");
            Console.WriteLine($"Обычная дробь: {fractionResult}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка при вычислении предела с применением правила Лопиталя: {ex.Message}");
        }



    }
}
