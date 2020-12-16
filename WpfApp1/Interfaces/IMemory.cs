using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace Calc.Interfaces
{
    interface IMemory
    {
        ObservableCollection<string> Values { get; }
        void Add(string value);
        void Remove(string item);
        void RemoveAtIndex(int index);
        void Increase(int index, string value);
        void Decrease(int index, string value);
        void Clear();
        int Count { get; }
        bool Any();
    }
}
