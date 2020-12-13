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
using System.Windows.Controls;

using Newtonsoft.Json;
using System.IO;

using Calc;

namespace WpfApp1
{
    class ViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static bool isComma = false;

        public ViewModel()
        {
            _expression = new ObservableCollection<Expression>();
            _memory = new ObservableCollection<string>();
        }

        private string textValue = "";
        public string TextValue
        {
            get => textValue;
            set
            {
                textValue = value;
                OnPropertyChanged(nameof(TextValue));
            }
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

        private ICommand _sumMem;
        public ICommand SumMem
        {
            get => _sumMem ?? new RelayCommand(() =>
            {
                string result = ParserCalc.calculate(TextValue);
                if (result == "error")
                    result = "";
                TextValue = result;
                Memory[Memory.Count - 1] = Convert.ToString(Convert.ToDouble(Memory[Memory.Count - 1]) + Convert.ToDouble(result));
            }, () => string.IsNullOrEmpty(TextValue) == false && Memory.Any()); //??????????????????//
        }

        private ICommand _subMem;
        public ICommand SubMem
        {
            get => _subMem ?? new RelayCommand(() =>
            {
                string result = ParserCalc.calculate(TextValue);
                if (result == "error")
                    result = "";
                TextValue = result;

                Memory[Memory.Count - 1] = Convert.ToString(Convert.ToDouble(Memory[Memory.Count - 1]) - Convert.ToDouble(result));
            }, () => string.IsNullOrEmpty(TextValue) == false && Memory.Any());
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

        private ICommand _takeMemory;
        public ICommand TakeMemory
        {
            get => _takeMemory ?? new RelayCommand<TextBox>((x) =>
            {
                TextValue = x.Text;
            }, x => true);
        }

        private ICommand _delMemory;
        public ICommand DelMemory
        {
            get => _delMemory ?? new RelayCommand<TextBox>((x) =>
            {
                Memory.Remove(x.Text);
            }, x => true);
        }

        private ICommand addNumber;
        public ICommand AddNumber
        {
            get => addNumber ?? new RelayCommand<string>(x =>
            {
                TextValue += x;
            }, x => true);
        }

        private ICommand calculate;
        public ICommand Calculate
        {
            get => calculate ?? new RelayCommand<string>(x =>
            {
                if (TextValue == "") return;

                char checkOp = TextValue[TextValue.Length - 1];
                int indexOp = "+-*/".IndexOf(checkOp);
                if (indexOp != -1)
                    TextValue = TextValue.Remove(TextValue.Length - 1);

                if (x == "=")
                {
                    string result = ParserCalc.calculate(TextValue);
                    Expressions.Add(new Expression(textValue, result));
                    if (result == "error")
                        result = "";
                    TextValue = result;
                    if (result.IndexOf(',') != -1)
                        isComma = true;
                }
                if ("+-*/".IndexOf(x) != -1)
                {
                    TextValue += x;
                    isComma = false;
                }
                if (x == "," && isComma == false)
                {
                    TextValue += x;
                    isComma = true;
                }
            }, x => true);
        }

        ICommand _brackets;
        public ICommand Brackets
        {
            get => _brackets ?? new RelayCommand<string>(x =>
            {
                TextValue += x;
            }, x => true);
        }

        ICommand _back;
        public ICommand Back
        {
            get => _back ?? new RelayCommand(() =>
            {
                if (TextValue == "") return;
                TextValue = TextValue.Remove(TextValue.Length - 1);
            }, () => true);
        }

        private ICommand _clear;
        public ICommand Clear
        {
            get => _clear ?? new RelayCommand(() =>
            {
                TextValue = "";
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
        
        public Dictionary<string, string> CollectionError { get; private set; } = new Dictionary<string, string>();
        public string Error => null;

        public string this[string name]
        {
            get
            {
                string messageError = null;
                if (name == "TextValue")
                        if (string.IsNullOrWhiteSpace(TextValue))
                            messageError = "Empty field";

                if (CollectionError.ContainsKey(name))
                    CollectionError[name] = messageError;
                else if (messageError != null)
                    CollectionError.Add(name, messageError);
                OnPropertyChanged(nameof(CollectionError));
                return messageError;
            }
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
