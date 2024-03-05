using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LimitsCalculator.Models
{
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
}
