using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LimitsCalculator.Models
{
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
            // Проверяем условия применимости правила Лопиталя
            if (double.IsInfinity(x))
            {
                // x стремится к бесконечности, применяем правило Лопиталя
                double fDerivative = Derivative(f, x);
                double gDerivative = Derivative(g, x);
                return fDerivative / gDerivative;
            }
            else
            {
                // Рассчитываем предел f(x) / g(x) при x стремящемся к заданному значению
                double fLimit = f(x);
                double gLimit = g(x);

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
        }



        private double Derivative(Func<double, double> func, double x)
        {
            const double h = 1e-6; // Шаг для численного дифференцирования

            // Вычисляем значение функции в точках (x - h) и (x + h)
            double fx_minus_h = func(x - h);
            double fx_plus_h = func(x + h);

            // Вычисляем производную как приближенное значение предела разности функций
            double derivative = (fx_plus_h - fx_minus_h) / (2 * h);

            return derivative;
        }

    }
}
