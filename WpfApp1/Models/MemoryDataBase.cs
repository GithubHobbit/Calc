using System;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using Calc.Interfaces;
using System.Data.SQLite;
using System.Data.Common;

namespace Calc.Models
{
    class MemoryDataBase : IMemory
    {
        public ObservableCollection<string> Values { get; }
        private string _nameDB;

        public MemoryDataBase(string nameDB = "calc.db")
        {
            _nameDB = nameDB;
            Values = Values ?? new ObservableCollection<string>();

            using (SQLiteConnection Connect = new SQLiteConnection($"Data Source={_nameDB}; Version=3;"))
            {
                string commandText = "CREATE TABLE IF NOT EXISTS [MemoryValues] ([value] VARCHAR NOT NULL)";
                SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
                Connect.Open();
                Command.ExecuteNonQuery();
                
                SQLiteCommand command = new SQLiteCommand("SELECT * FROM [MemoryValues]", Connect);
                SQLiteDataReader reader = command.ExecuteReader();
                foreach (DbDataRecord record in reader)
                    Values.Add(record["value"].ToString());
                Connect.Close();
            }
        }
        
        public void Add(string value)
        {
            using (SQLiteConnection Connect = new SQLiteConnection($"Data Source={_nameDB}; Version=3;"))
            {
                Connect.Open();
                SQLiteCommand command = new SQLiteCommand(Connect);
                command.CommandText = "INSERT INTO [MemoryValues] VALUES (@value)";
                command.Parameters.AddWithValue("@value", value);
                command.ExecuteNonQuery();
            }
            Values.Add(value);
        }

        public void Remove(string value)
        {
            using (SQLiteConnection Connect = new SQLiteConnection($"Data Source={_nameDB}; Version=3;"))
            {
                Connect.Open();
                SQLiteCommand command = new SQLiteCommand(Connect);
                command.CommandText = $"DELETE FROM [MemoryValues] WHERE value=@value";
                command.Parameters.AddWithValue("@value", value);
                command.ExecuteNonQuery();
            }
            Values.Remove(value);
        }

        public void RemoveAtIndex(int index)
        {
            using (SQLiteConnection Connect = new SQLiteConnection($"Data Source={_nameDB}; Version=3;"))
            {
                Connect.Open();
                SQLiteCommand command = new SQLiteCommand(Connect);
                command.CommandText = $"DELETE FROM [MemoryValues] WHERE rowid={index + 1}";
                command.ExecuteNonQuery();
            }
            Values.RemoveAt(index);
        }

        public void Increase(int index, string value)
        {
            double newValue = Convert.ToDouble(Values[index]) + Convert.ToDouble(value);
            using (SQLiteConnection Connect = new SQLiteConnection($"Data Source={_nameDB}; Version=3;"))
            {
                Connect.Open();
                SQLiteCommand command = new SQLiteCommand(Connect);
                command.CommandText = $"UPDATE [MemoryValues] SET value=@value WHERE rowid={index + 1}";
                command.Parameters.AddWithValue("@value", newValue);
                command.ExecuteNonQuery();
            }
            Values[index] = newValue.ToString();
        }

        public void Decrease(int index, string value)
        {
            double newValue = Convert.ToDouble(Values[index]) - Convert.ToDouble(value);
            using (SQLiteConnection Connect = new SQLiteConnection($"Data Source={_nameDB}; Version=3;"))
            {
                Connect.Open();
                SQLiteCommand command = new SQLiteCommand(Connect);
                command.CommandText = $"UPDATE [MemoryValues] SET value=@newValue WHERE rowid = {index + 1}";
                command.Parameters.AddWithValue("@newValue", newValue);
                command.ExecuteNonQuery();
            }
            Values[index] = newValue.ToString();
        }

        public void Clear()
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source = " + _nameDB))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = "DELETE FROM [MemoryValues]";
                command.ExecuteNonQuery();
            }
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