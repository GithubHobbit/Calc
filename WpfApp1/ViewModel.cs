﻿using System;
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

using Newtonsoft.Json;
using System.IO;

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
            _memory = new ObservableCollection<string>();
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

        private ICommand _toMemory;
        public ICommand AddToMemory
        {
            get => _toMemory ?? new RelayCommand(
                () =>
                {
                    Memory.Add(TextValue);
                }, () => TextValue.Length > 0);
        }

        private ICommand _removeFromMemory;
        public ICommand RemoveFromMemory
        {
            get => _removeFromMemory ?? new RelayCommand(
                () =>
                {
                    Memory.RemoveAt(Memory.Count() - 1);
                }, () => Memory.Any());
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
                        Expressions.Add(new Expression(textValue, leftValue));
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
            public string Exp { get; }
            public string Value { get; }
            public Expression(string expression, string result)
            {
                Exp = expression;
                Value = result;
            }
        }

        private ObservableCollection<string> _memory;
        public ObservableCollection<string> Memory
        {
            get => _memory;
        }

        /*
        var serializeObject = JsonConvert.SerializeObject(student, Base64Formatting.Indented);
        File.WriteAllText("my-json.json", serializeObject);
            var result = FileStyleUriParser.ReadAllText("my-json.json");
        File.Exist();

        Var otherStudent = JsonConvert.DeserializedObject<Group>(serializeObject);
        
        public static void expr(Expression exp)
        {
            var serializeObject = JsonConvert.SerializeObject(exp, Formatting.Indented);
            File.WriteAllText("my-json.json", serializeObject);
            var result = File.ReadAllText("my-json.json");
        } */
    }
}
