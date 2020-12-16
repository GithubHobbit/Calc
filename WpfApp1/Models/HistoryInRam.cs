using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Calc.Interfaces;

namespace Calc.Models
{
    public class HistoryRAM : IHistory
    {
        public ObservableCollection<Expression> Values { get; }
        public HistoryRAM()
        {
            Values = new ObservableCollection<Expression>();
        }
        public void Add(Expression expression)
        {
            Values.Add(expression);
        }

        public void Clear()
        {
            if (Values.Count > 0)
                Values.Clear();
        }
    }
}
