using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LimitsCalculator.Models
{
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
}
