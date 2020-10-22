using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

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
                if (rightValue == null)
                {
                    if (x == "=") return;
                    op = x;
                }
                else if (x == "=")
                {
                    switch (op)
                    {
                        case "+": leftValue = Convert.ToString(Double.Parse(leftValue) + Double.Parse(rightValue)); break;
                        case "-": leftValue = Convert.ToString(Double.Parse(leftValue) - Double.Parse(rightValue)); break;
                        case "*": leftValue = Convert.ToString(Double.Parse(leftValue) * Double.Parse(rightValue)); break;
                        case "/": leftValue = Convert.ToString(Double.Parse(leftValue) / Double.Parse(rightValue)); break;
                        default: break;
                    }

                    op = null;
                    rightValue = null;
                }

                PrintValue();

            }, x => true);
        }

        /*private ICommand _comma;
        public ICommand Comma
        {
            get => _comma ?? new RelayCommand<string>(x =>
            {
                TextValue += x;
            }, x => !TextValue.Contains(x));
        }
        */

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
    }
}
