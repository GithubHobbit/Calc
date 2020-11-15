using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Dynamic;

using Calc;

namespace WpfApp1
{
    class ViewModel : INotifyPropertyChanged
    {
        private static string leftValue = "0";
        private static string rightValue = null;
        private static string op = null;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ViewModel()
        {
            _expression = new ObservableCollection<Expression>();
        }

        private string textValue = "0";
        public string TextValue
        {
            get => textValue;
            set
            {
                textValue = value;
                OnPropertyChanged(nameof(TextValue));
            }
        }

        public void PrintValue() {
            TextValue = leftValue + " " + op + " " + rightValue;
        }


        private ICommand addNumber;
        public ICommand AddNumber
        {
            get => addNumber ?? new RelayCommand<string>(x =>
            {
                if (op == null)
                {
                    if (leftValue == "0")
                    {
                        leftValue = x;
                    }
                    else leftValue += x;
                }
                else rightValue += x;

                PrintValue();
            }, x => true);
        }

        private ICommand calculate;
        public ICommand Calculate
        {
            get => calculate ?? new RelayCommand<string>(x =>
            {
                /*if (rightValue == null)
                {
                    if (x == "=") return;
                    op = x;
                }
                if (op != "=")
                {
                    op = x;
                }
                else if (x == "=")
                {
                    string exp = leftValue + op + rightValue;
                    leftValue = ParserCalc.calculate(exp);
                    
                    op = null;
                    rightValue = null;
                }*/



                if (rightValue == null)
                {
                    if (x == "=")
                        Expressions.Add(new Expression(textValue, leftValue));

                    else op = x;
                }
                else if (rightValue != null)
                {
                    if (x == "=")
                    {
                        string exp = leftValue + op + rightValue;
                        leftValue = ParserCalc.calculate(exp);
                        Expressions.Add(new Expression(textValue, leftValue)); // &&&&&
                        op = null;
                    }
                    else
                    {
                        leftValue = leftValue + op + rightValue;
                        op = x;
                    }
                    
                    rightValue = null;
                       
                }

                PrintValue();

            }, x => true);
        }

        private ICommand _clear;
        public ICommand Clear
        {
            get => _clear ?? new RelayCommand(() =>
            {
                leftValue = "0";
                TextValue = leftValue;
                rightValue = null;
                op = null;
            }, () => true);
        }

        private ObservableCollection<Expression> _expression;
        public ObservableCollection<Expression> Expressions
        {
            get => _expression;
        }

        public class Expression
        {
            public Expression(string exp, string answer)
            {
                Exp = exp;
                Value = answer;
            }
            public string Exp { get; }
            public string Value { get; }
        }
    }
}
