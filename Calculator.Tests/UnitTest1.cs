using System;
using Xunit;
using Calc;

namespace Calculator.Tests
{
    public class CalcTests
    {
        [Fact]
        public void Sum_2Plus5_Returned()
        {
            var res = ParserCalc.calculate("2 + 5");
            Assert.Equal("7", res);
        }
        [Fact]

        public void Div_7Div3_Returned()
        {
            var res = ParserCalc.calculate("7 / 3");
            Assert.Equal(Convert.ToString(7.0 / 3), res);
        }
 
        [Fact]
        public void Mul_10MulUnderfined_Returned()
        {
            var res = ParserCalc.calculate("10*");
            Assert.Equal("error", res);
        }
        [Fact]
        public void Div_UnderfinedDiv6_Returned()
        {
            var res = ParserCalc.calculate("/ 6");
            Assert.Equal("error", res);
        }

        [Fact]
        public void Exp_8Plus2Mul10_Returned()
        {
            var res = ParserCalc.calculate("8+2*10+3");
            Assert.Equal("31", res);
        }

        [Fact]
        public void Exp_10Div3Mul6Plus3_Returned()
        {
            var res = ParserCalc.calculate("10/3*6+3");
            Assert.Equal("23", res);
        }
        [Fact]
        public void Exp_1Div2Mul3PlusUnderfined_Returned()
        {
            var res = ParserCalc.calculate("1/2*3+");
            Assert.Equal("error", res);
        }
        [Fact]
        public void Exp_5Div2point5_Returned()
        {
            var res = ParserCalc.calculate("5/2,5");
            Assert.Equal("2", res);
        }
        [Fact]
        public void Exp_10Plus20Mul2Div4Minus10Mul2point5_Returned()
        {
            var res = ParserCalc.calculate("10+20*2/4-10*2,5");
            Assert.Equal("-5", res);
        }
        [Fact]
        public void Exp_4PlusBracket2Plus3BracketMul4DivBracket10Minus5Bracket_Returned8()
        {
            var res = ParserCalc.calculate("4+(2+3)* 4 / ( 10 - 5)");
            Assert.Equal("8", res);
        }
        [Fact]
        public void Exp_BrBrBr10Mul5BrBrBrPlusBr2Br_Return52()
        {
            var res = ParserCalc.calculate("(((10*5))) + (2)");
            Assert.Equal("52", res);
        }
        [Fact]
        public void BigExpressionTest()
        {
            var res = ParserCalc.calculate("((4))+(2*3)-(1+2)*3-(10/((7-2)-1))");
            Assert.Equal("-1,5", res);
        }

        [Fact]
        public void Exp_Br10Plus5_ReturnError()
        {
            var res = ParserCalc.calculate("(10+5");
            Assert.Equal("error", res);
        }
        [Fact]
        public void Exp_10Plus5Br_ReturnError()
        {
            var res = ParserCalc.calculate("10+5)");
            Assert.Equal("error", res);
        }
    }
}
