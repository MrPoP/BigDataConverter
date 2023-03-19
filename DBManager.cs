using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace BigDataConverter
{
    interface IManager
    {
        void Insert(string line);
    } 
    internal class DBManager : IManager
    {
        private string DBFile = "MyDatabase.db";
        private string ConnectionString = "";
        private SQLiteConnection m_dbConnection;
        private bool CreatedNow = false;
        public DBManager()
        {
            if(!File.Exist(Path.Combin(Environment.CurrentDirectory, DBFile)))
            {
                SQLiteConnection.CreateFile(DBFile);
                CreatedNow = true;
            }
            ConnectionString = $"Data Source={DBFile};Version=3;";
            if(CreatedNow)
            {
                m_dbConnection = new SQLiteConnection(ConnectionString);
                m_dbConnection.Open();
                string sql = "CREATE TABLE IF NOT EXISTS FaceBook (one varchar(50), two varchar(50), three varchar(50), four varchar(50), five varchar(50), six varchar(50), seven varchar(50), eight varchar(50), nine varchar(50), ten varchar(50), eleven varchar(50), twelve varchar(50))";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                m_dbConnection.Close();
            }
        }
        public void Insert(string line)
        {
            if(m_dbConnection == null)
            {
                m_dbConnection = new SQLiteConnection(ConnectionString);
            }
            if(!string.IsNullOrEmpty(line) && !string.IsNullOrWhiteSpace(line) && line.Contains(','))
            {
                try
                {
                    m_dbConnection.Open();
                    sql = $"Insert into FaceBook (one, two, three, four, five, six, seven, eight, nine, ten, eleven, twelve) values ({line})";
                    SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                    command.ExecuteNonQuery();
                }
                catch(Exception es)
                {
                    Console.WriteLine($"Found Syntax Error in line {line}");
                }
                finally
                {
                    m_dbConnection.Close();
                }
            }
        }
    }
}
