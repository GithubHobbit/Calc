using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Calc.Interfaces;
using System.Linq;


namespace Calc.Models
{
    public class MemoryInRam : IMemory
    {
        public MemoryInRam()
        {
            Values = new ObservableCollection<string>();
        }
        public ObservableCollection<string> Values { get; }

        public void Add(string value)
        {
            Values.Add(value);
        }

        public void Remove(string item)
        {
            Values.Remove(item);
        }
        public void RemoveAtIndex(int index)
        {
            Values.RemoveAt(index);
        }

        public void Increase(int index, string value)
        {
            Values[index] = Convert.ToString(Convert.ToDouble(Values[index]) + Convert.ToDouble(value));
        }

        public void Decrease(int index, string value)
        {
            Values[index] = Convert.ToString(Convert.ToDouble(Values[index]) - Convert.ToDouble(value));
        }

        public void Clear()
        {
            Values.Clear();
        }
        public int Count
        {
            get => Values.Count;
        }
        public bool Any()
        {
            return Values.Any();
        }
    }
}