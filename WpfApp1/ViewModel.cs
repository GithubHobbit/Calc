using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Controls;
using Calc.Interfaces;
using Calc.Models;
using Calc;

namespace Calculator
{
    class ViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static bool isComma = false;

        public IHistory History { get; }
        public IMemory Mem { get; }

        public ViewModel()
        {
            //History = new HistoryInFile();
            History = new HistoryDataBase();
            //Mem = new MemoryInRam();
            //Mem = new MemoryInFile();
            Mem = new MemoryDataBase();
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
                    Mem.Add(TextValue);
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
                Mem.Increase(Mem.Count - 1, TextValue);
            }, () => string.IsNullOrEmpty(TextValue) == false && Mem.Any());
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

                Mem.Decrease(Mem.Count - 1, TextValue);
            }, () => string.IsNullOrEmpty(TextValue) == false && Mem.Any());
        }

        private ICommand _removeFromMemory;
        public ICommand RemoveFromMemory
        {
            get => _removeFromMemory ?? new RelayCommand(() =>
            {
                Mem.RemoveAtIndex(Mem.Count - 1);
            }, () => Mem.Any());
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
                Mem.Remove(x.Text);
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
                    History.Add(new Calc.Models.Expression(textValue, result));
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

        private ICommand _clearAll;
        public ICommand ClearAll
        {
            get => _clearAll ?? new RelayCommand(() =>
            {
                Mem.Clear();
                History.Clear();
            }, () => Mem.Values.Count > 0 || History.Values.Count > 0);
        }

        public Dictionary<string, string> ErrorCollection { get; private set; } = new Dictionary<string, string>();
        public string Error => null;

        public string this[string name]
        {
            get
            {
                string messageError = null;
                if (name == "TextValue")
                        if (string.IsNullOrWhiteSpace(TextValue))
                            messageError = "Empty field";

                if (ErrorCollection.ContainsKey(name))
                    ErrorCollection[name] = messageError;
                else if (messageError != null)
                    ErrorCollection.Add(name, messageError);
                OnPropertyChanged(nameof(ErrorCollection));
                return messageError;
            }
        }
    }
}
