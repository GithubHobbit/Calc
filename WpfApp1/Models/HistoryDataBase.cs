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
    class HistoryDataBase : IHistory
    {
        public ObservableCollection<Expression> Values { get; }
        private string _nameDB;

        public HistoryDataBase(string nameDB = "calc.db")
        {
            _nameDB = nameDB;
            Values = Values ?? new ObservableCollection<Expression>();

            using (SQLiteConnection Connect = new SQLiteConnection($"Data Source={_nameDB}; Version=3;"))
            {
                string commandText = "CREATE TABLE IF NOT EXISTS [HistoryValues] ([exp] VARCHAR NOT NULL, [value] VARCHAR NOT NULL)";
                SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
                Connect.Open();
                Command.ExecuteNonQuery();

                SQLiteCommand command = new SQLiteCommand("SELECT * FROM [HistoryValues]", Connect);
                SQLiteDataReader reader = command.ExecuteReader();
                foreach (DbDataRecord record in reader)
                    Values.Add(new Expression(record["exp"].ToString(), record["value"].ToString()));
                Connect.Close();
            }
        }

        public void Add(Expression exp)
        {
            using (SQLiteConnection Connect = new SQLiteConnection($"Data Source={_nameDB}; Version=3;"))
            {
                Connect.Open();
                SQLiteCommand command = new SQLiteCommand(Connect);
                command.CommandText = "INSERT INTO [HistoryValues] VALUES (@expression, @value)";
                command.Parameters.AddWithValue("@expression", exp.Exp);
                command.Parameters.AddWithValue("@value", exp.Value);

                command.ExecuteNonQuery();
            }
            Values.Add(exp);
        }

        public void Clear()
        {
            using (SQLiteConnection Connect = new SQLiteConnection("Data Source = " + _nameDB))
            {
                Connect.Open();
                SQLiteCommand command = new SQLiteCommand(Connect);
                command.CommandText = "DELETE FROM [HistoryValues]";
                command.ExecuteNonQuery();
            }
            Values.Clear();
        }
    }
}
