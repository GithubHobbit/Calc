using System;
using System.Collections.ObjectModel;
using System.IO;
using Calc.Interfaces;
using Newtonsoft.Json;

namespace Calc.Models
{
    public class HistoryInFile : IHistory
    {
        public ObservableCollection<Expression> Values { get; }
        private string file;

        public HistoryInFile(string jsonPath = "historyJson.json")
        {
            file = jsonPath;
            if (File.Exists(file) == false)
            {
                FileStream stream = File.Create(file);
                stream.Close();
                Values = new ObservableCollection<Expression>();
                return;
            }
            var fileText = File.ReadAllText(file);
            Values = JsonConvert.DeserializeObject<ObservableCollection<Expression>>(fileText);
            Values = Values ?? new ObservableCollection<Expression>();
        }
        public void Add(Expression expression)
        {
            Values.Add(expression);
            SaveToJson();
        }

        public void Clear()
        {
            if (Values.Count > 0)
                Values.Clear();
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
    }
}
