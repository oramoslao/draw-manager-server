using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace DrawManager.ImportTool
{
    class Program
    {
        private const string FILE_PATH = @"Data\Import\data.txt";

        private const string FILE_PATH_SQL_PRIZES = @"Data\Sql\populate-prizes.sql";
        private const string FILE_PATH_SQL_DRAWS = @"Data\Sql\populate-draws.sql";
        private const string FILE_PATH_SQL_ENTRANTS = @"Data\Sql\populate-entrants.sql";
        private const string FILE_PATH_SQL_DRAW_ENTRIES = @"Data\Sql\populate-draw-entries.sql";
        private const string FILE_PATH_SQL_CREATE_TABLE_DATA = @"Data\Sql\create-table-data.sql";

        private const string CONNECTION_STRING = "data source=Data\\Database\\draw_manager_docker_db.sqlite";
        private const string TABLE_NAME_DATA = "Data";
        private const string TABLE_NAME_DRAWS = "Draws";
        private const string TABLE_NAME_PRIZES = "Prizes";
        private const string TABLE_NAME_ENTRANTS = "Entrants";
        private const string TABLE_NAME_DRAW_ENTRIES = "DrawEntries";

        private const string MESSAGE_ELAPSED_TIME = "Elapsed time: {0}:{1}:{2}";
        private const string MESSAGE_POPULATE_DATABASE_BEGIN = "Populate database starting.";
        private const string MESSAGE_POPULATE_DATABASE_END = "Populate database done.";
        private const string MESSAGE_POPULATE_TABLE_BEGIN = "Populate table '{0}' starting.";
        private const string MESSAGE_POPULATE_TABLE_END = "Populate table '{0}' done.";
        private const string MESSAGE_DELETE_TABLE_BEGIN = "Delete table '{0}' starting.";
        private const string MESSAGE_DELETE_TABLE_END = "Delete table '{0}' done.";
        private const string MESSAGE_CREATE_TABLE_BEGIN = "Create table '{0}' starting.";
        private const string MESSAGE_CREATE_TABLE_END = "Create table '{0}' done.";

        private const string CMD_DROP_TABLE = "DROP TABLE IF EXISTS {0}";
        private const string CMD_EMPTY_TABLE = "DELETE FROM {0}";
        private const string CMD_INSERT_INTO_TABLE_DATA = "INSERT INTO Data (Identificacion, Nombre, Provincia) VALUES (\"{0}\", \"{1}\", \"{2}\");";

        static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine(MESSAGE_POPULATE_DATABASE_BEGIN);

            CreateTableData();
            PopulateTableData();
            PopulateTableEntrans();
            PopulateTableDraws();
            PopulateTablePrizes();
            PopulateTableDrawEntries();
            DeleteTableData();

            stopwatch.Stop();
            WriteMessageWithTime(stopwatch, string.Format("{0} {1}", MESSAGE_POPULATE_DATABASE_END, MESSAGE_ELAPSED_TIME));

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }

        private static void WriteMessageWithTime(Stopwatch stopwatch, string message)
        {
            var hours = stopwatch.Elapsed.Hours < 10 ? string.Format("0{0}", stopwatch.Elapsed.Hours) : stopwatch.Elapsed.Hours.ToString();
            var minutes = stopwatch.Elapsed.Minutes < 10 ? string.Format("0{0}", stopwatch.Elapsed.Minutes) : stopwatch.Elapsed.Minutes.ToString();
            var seconds = stopwatch.Elapsed.Seconds < 10 ? string.Format("0{0}", stopwatch.Elapsed.Seconds) : stopwatch.Elapsed.Seconds.ToString();

            Console.WriteLine(message, hours, minutes, seconds);
        }

        private static void DeleteTableData()
        {
            Console.WriteLine(MESSAGE_DELETE_TABLE_BEGIN, TABLE_NAME_DATA);
            var commandText = string.Format(CMD_DROP_TABLE, TABLE_NAME_DATA);
            ExecuteQuery(commandText);
            Console.WriteLine(MESSAGE_DELETE_TABLE_END, TABLE_NAME_DATA);
        }

        private static void PopulateTableDrawEntries()
        {
            Console.WriteLine(MESSAGE_POPULATE_TABLE_BEGIN, TABLE_NAME_DRAW_ENTRIES);
            ExecuteFileQuery(FILE_PATH_SQL_DRAW_ENTRIES);
            Console.WriteLine(MESSAGE_POPULATE_TABLE_END, TABLE_NAME_DRAW_ENTRIES);
        }

        private static void PopulateTablePrizes()
        {
            Console.WriteLine(MESSAGE_POPULATE_TABLE_BEGIN, TABLE_NAME_PRIZES);
            ExecuteFileQuery(FILE_PATH_SQL_PRIZES);
            Console.WriteLine(MESSAGE_POPULATE_TABLE_END, TABLE_NAME_PRIZES);
        }

        private static void PopulateTableDraws()
        {
            Console.WriteLine(MESSAGE_POPULATE_TABLE_BEGIN, TABLE_NAME_DRAWS);
            ExecuteFileQuery(FILE_PATH_SQL_DRAWS);
            Console.WriteLine(MESSAGE_POPULATE_TABLE_END, TABLE_NAME_DRAWS);
        }

        private static void PopulateTableEntrans()
        {
            Console.WriteLine(MESSAGE_POPULATE_TABLE_BEGIN, TABLE_NAME_ENTRANTS);
            ExecuteFileQuery(FILE_PATH_SQL_ENTRANTS);
            Console.WriteLine(MESSAGE_POPULATE_TABLE_END, TABLE_NAME_ENTRANTS);
        }

        private static void PopulateTableData()
        {
            Console.WriteLine(MESSAGE_POPULATE_TABLE_BEGIN, TABLE_NAME_DATA);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            char[] delimiterChars = { '\t' };
            var allLines = File.ReadAllLines(FILE_PATH, Encoding.UTF8);

            using (SQLiteConnection conn = new SQLiteConnection(CONNECTION_STRING))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand(conn))
                {
                    using (var dbTran = conn.BeginTransaction())
                    {
                        cmd.CommandText = string.Format(CMD_EMPTY_TABLE, TABLE_NAME_DATA);
                        cmd.ExecuteNonQuery();

                        var currentCursorLeft = Console.CursorLeft;
                        var currentCursorTop = Console.CursorTop;

                        for (int i = 1; i < allLines.Length; i++)

                        {
                            var line = allLines[i];

                            if (string.IsNullOrWhiteSpace(allLines[i])) return;

                            var items = line.Split(delimiterChars);

                            cmd.CommandText = string.Format(CMD_INSERT_INTO_TABLE_DATA, items[0].Trim(), items[1].Trim(), items[2].Trim());

                            try
                            {
                                cmd.ExecuteNonQuery();

                            }
                            catch (Exception e)
                            {
                                conn.Close();
                                throw;
                            }

                            Console.SetCursorPosition(currentCursorLeft, currentCursorTop);
                            Console.Write("Proccessing line: {0} of {1}", i, allLines.Length - 1);

                        }

                        dbTran.Commit();

                    }
                }

                conn.Close();
            }

            stopwatch.Stop();
            Console.WriteLine();
            WriteMessageWithTime(stopwatch, string.Format("{0} {1}", MESSAGE_POPULATE_DATABASE_END, MESSAGE_ELAPSED_TIME));

        }

        private static void CreateTableData()
        {
            Console.WriteLine(MESSAGE_CREATE_TABLE_BEGIN, TABLE_NAME_DATA);
            ExecuteFileQuery(FILE_PATH_SQL_CREATE_TABLE_DATA);
            Console.WriteLine(MESSAGE_CREATE_TABLE_END, TABLE_NAME_DATA);
        }

        private static void ExecuteFileQuery(string filePath)
        {
            var commandText = File.ReadAllText(filePath);
            ExecuteQuery(commandText);
        }
        private static void ExecuteQuery(string commandText)
        {
            using (SQLiteConnection conn = new SQLiteConnection(CONNECTION_STRING))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = commandText;
                    cmd.ExecuteNonQuery();
                }

                conn.Close();
            }
        }
    }
}
