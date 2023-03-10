using BigDataConverter;
using System.Data.SQLite;

internal class Program
{
    static BigDataReader dataReader;
    static AIDataReader aIDataReader;
    static ResultAnalyzer resultAnalyzer;
    static List<string> AnalyzedLines =new List<string>();
    private static void Main(string[] args)
    {
        Console.WriteLine("Loading file and preparing loader.");
        //if (args.Length > 0)
        {
            string File = Environment.CurrentDirectory + "\\South Korea.txt";
            //dataReader = new BigDataReader(args[0]);
            aIDataReader = new AIDataReader(File);// (args[0]);
            resultAnalyzer = new ResultAnalyzer(':');
            aIDataReader.Start();
            while (aIDataReader.ProcessWorking)
            {
                string line = aIDataReader.GetNextLine(out bool IsNewLine);
                if (IsNewLine)
                {
                    Console.WriteLine(line);
                }
                string value = resultAnalyzer.Feed(line);
                if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                    AnalyzedLines.Add(value);
            }
            if(AnalyzedLines.Count > 0)
                CreateDB(AnalyzedLines);
            /*do
            {
                Console.WriteLine(dataReader.GetLines().Current);
            } while (dataReader.GetLines().MoveNext());*/
        }
        Console.Read();
    }
    private static void CreateDB(List<string> Lines)
    {
        SQLiteConnection.CreateFile("MyDatabase.sqlite");
        string connectionString = "Data Source=MyDatabase.sqlite;Version=3;";
        SQLiteConnection m_dbConnection = new SQLiteConnection(connectionString);
        m_dbConnection.Open();
        string sql = "CREATE TABLE IF NOT EXISTS FaceBook (one varchar(50), two varchar(50), three varchar(50), four varchar(50), five varchar(50), six varchar(50), seven varchar(50), eight varchar(50), nine varchar(50), ten varchar(50), eleven varchar(50), twelve varchar(50))";
        SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
        command.ExecuteNonQuery();
        foreach(string line in Lines)
        {
            if(!string.IsNullOrEmpty(line) && !string.IsNullOrWhiteSpace(line) && line.Contains(','))
            {
                try
                {
                    sql = $"Insert into FaceBook (one, two, three, four, five, six, seven, eight, nine, ten, eleven, twelve) values ({line})";
                    command = new SQLiteCommand(sql, m_dbConnection);
                    command.ExecuteNonQuery();
                }
                catch
                {
                    Console.WriteLine($"Found Syntax Error in line {line}");
                    continue;
                }
            }
        }
        
        m_dbConnection.Close();
    }
}