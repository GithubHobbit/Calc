using System;
using System.Collections.ObjectModel;
using System.IO;
using Calc.Interfaces;
using Newtonsoft.Json;

namespace Calc.Models
{
    public class MemoryInFile : IMemory
    {
        private string file;
        public ObservableCollection<string> Values { get; }
        public MemoryInFile(string jsonPath = "memJson.json")
        {
            file = jsonPath;
            if (File.Exists(file) == false)
            {
                FileStream stream = File.Create(file);
                stream.Close();
                Values = new ObservableCollection<string>();
                return;
            }
            var fileText = File.ReadAllText(file);
            Values = JsonConvert.DeserializeObject<ObservableCollection<string>>(fileText);
            Values = Values ?? new ObservableCollection<string>();
        }

        public void Add(string value)
        {
            Values.Add(value);
            SaveToJson();
        }

        public void Remove(string item)
        {
            Values.Remove(item);
        }

        public void RemoveAtIndex(int index)
        {
            Values.RemoveAt(index);
            SaveToJson();
        }

        public void SaveToJson()
        {
            if (File.Exists(file) == false)
            {
                File.Create(file);
            }
            var data = JsonConvert.SerializeObject(Values);
            File.WriteAllText(file, data);
        }

        public void Increase(int index, string value)
        {
            Values[index] = Convert.ToString(Convert.ToDouble(value) + Convert.ToDouble(Values[index]));
            SaveToJson();
        }

        public void Decrease(int index, string value)
        {
            Values[index] = Convert.ToString(Convert.ToDouble(Values[index]) - Convert.ToDouble(value));
            SaveToJson();
        }

        public void Clear()
        {
            Values.Clear();
            SaveToJson();
        }
        
        public int Count
        {
            get => Values.Count;
        }
        
        public bool Any()
        {
            return Values.Count > 0;
        }
    }
}
