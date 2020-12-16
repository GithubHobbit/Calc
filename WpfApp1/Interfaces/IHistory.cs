using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Linq;
using Calc.Models;

namespace Calc.Interfaces
{
    interface IHistory
    {
        ObservableCollection<Expression> Values { get; }
        void Add(Expression expression);
        void Clear();
    }
}
