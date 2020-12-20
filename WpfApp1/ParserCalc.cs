using System;

namespace Calc 
{
    public class ParserCalc
    {
        public static string calculate(string exp)
        {
            exp = RemoveBrackets(exp);
            int[] arrayIndexOp = FillArrayOfSymbols(exp, new char[] { '-', '+', '*', '/' });

            var arrValue = exp.Split(new char[] { '-', '+', '*', '/' });
            string result = arrValue[0];

            for (int i = 0; i < arrValue.Length; i++)
            {
                int num; double double_num;
                if (Int32.TryParse(arrValue[i], out num) == false &&
                    Double.TryParse(arrValue[i], out double_num) == false)
                    return "error";
            }
                

            result = SolveExp(arrayIndexOp, arrValue, exp);
            return result;
        }

        private static string RemoveBrackets(string exp)
        {
            int countStartBracket = exp.Split('(').Length - 1;
            int countEndBracket = exp.Split(')').Length - 1;
            if (countStartBracket != countEndBracket) return "error";
            if ((countStartBracket + countEndBracket) == 0) return exp;

            int[] arrIndexBrackets = FillArrayOfSymbols(exp, new char[] { '(', ')' });
            
            int queue = 0;          // queue - очередь скобок. Т.к. по умолчанию открыта одна скобка, то ожидается одна закрывающая (поэтому очередь пока состоит из одной закр. скобки)
            int startBracket = -1;
            for (int i = 0; i < arrIndexBrackets.Length ; i++)
            { 
                char bracket = exp[arrIndexBrackets[i]];
                if (queue < 0) return "error";
                if (bracket == '(' && queue == 0)
                    startBracket = arrIndexBrackets[i];


                if (bracket == ')' && queue == 1)
                {                                                                           // Добавить бы проверку на пустое содержимое скобок
                    int endBracket = arrIndexBrackets[i];
                    string subexp = exp.Substring(startBracket + 1, endBracket - startBracket - 1);
                    string result = calculate(subexp);
                    if (result == "error") return "error";
                    exp = exp.Remove(startBracket, endBracket - startBracket + 1);
                    exp = exp.Insert(startBracket, result);
                    arrIndexBrackets = FillArrayOfSymbols(exp, new char[] { '(', ')' });
                    i = -1;
                    queue = 0;
                }
                else if (bracket == ')')
                    queue--;
                else if (bracket == '(')
                    queue++;
            }

            if (queue != 0)
                return "error";

            return exp;
        }

        private static string SolveExp(int[] arrOp, string[] arrValue, string exp)
        {
            for (int i = 0; i < arrOp.Length; i++)
            {
                string value = "";
                if (exp[arrOp[i]] == '*')
                    value = Convert.ToString(Double.Parse(arrValue[i]) * Double.Parse(arrValue[i + 1]));
                else if (exp[arrOp[i]] == '/')
                {
                    double double_value = Double.Parse(arrValue[i]) / Double.Parse(arrValue[i + 1]);
                    value = Convert.ToString(Math.Round(double_value, 3));
                }
                if (value != "")                                                                                    // Присваиваю результат левому операнду в массиве, а правый удаляю
                {
                    arrValue[i] = value;

                    Array.Clear(arrValue, i + 1, 1);
                    for (int index = i + 1; index + 1 < arrValue.Length; index++)
                        arrValue[index] = arrValue[index + 1];
                    Array.Resize(ref arrValue, arrValue.Length - 1);

                    Array.Clear(arrOp, i, 1);
                    for (int index = i; index + 1 < arrOp.Length; index++)
                        arrOp[index] = arrOp[index + 1];
                    Array.Resize(ref arrOp, arrOp.Length - 1);
                    i--;
                }
            }

            string result = arrValue[0];
            for (int i = 0; i < arrOp.Length; i++)
            {
                if (exp[arrOp[i]] == '+')
                    result = Convert.ToString(Double.Parse(result) + Double.Parse(arrValue[i + 1]));
                if (exp[arrOp[i]] == '-')
                {
                    double value = Double.Parse(result) - Double.Parse(arrValue[i + 1]);
                    result = Convert.ToString(Math.Round(value, 3));
                }
            }
            return result;
        }
        private static int[] FillArrayOfSymbols( string exp, char[] symbols) // Заносит в массив индексы всех символов в выражении
        {
            int[] arrOfIndexSymbols = new int[100];
            int size = 0;

            for (int i = 0; i < symbols.Length; i++)
            {
                int indexSymbol = -1;
                char symbol = symbols[i];
                while (exp.Contains(symbol))
                {
                    indexSymbol = exp.IndexOf(symbol);
                    arrOfIndexSymbols[size++] = indexSymbol;
                    exp = exp.Remove(indexSymbol, 1);
                    exp = exp.Insert(indexSymbol, " ");
                }
            }


            Array.Resize(ref arrOfIndexSymbols, size);
            Array.Sort(arrOfIndexSymbols);
            return arrOfIndexSymbols;
        }

        public static bool isCorrect(string exp)
        {
            if (exp != "" &&
                "+-*/,".IndexOf(exp[exp.Length - 1]) == -1 &&
                isRightBrackets(exp))
                return true;
            return false;
        }

        private static bool isRightBrackets(string exp)
        {
            int queue = 0;          // queue - очередь скобок. Т.к. по умолчанию открыта одна скобка, то ожидается одна закрывающая (поэтому очередь пока состоит из одной закр. скобки)
            char isLastBracket = '-';
            for (int i = 0; i < exp.Length; i++)
            {
                if (exp[i] == '(')
                {
                    if (isLastBracket == ')') return false;
                    isLastBracket = '(';
                    queue++;
                }
                else if (exp[i] == ')')
                {
                    isLastBracket = ')';
                    queue--;
                }

                if (queue < 0) return false;

            }
            if (queue == 0)
                return true;
            else return false;
        }
    }
}


