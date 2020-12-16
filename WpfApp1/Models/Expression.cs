using System;
using System.Collections.Generic;
using System.Text;

namespace Calc.Models
{
    public class Expression
    {
        public string Exp { get; set; }
        public string Value { get; set;  }
        public Expression(string expression, string result)
        {
            Exp = expression;
            Value = result;
        }
    }
}
