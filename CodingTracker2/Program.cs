using System;
using System.Data.SQLite;

namespace CodingTracker2
{
    class Program
    {
        static void Main(string[] args)
        {
            if (File.Exists("CodingLogDB.db"))
            {
                Console.WriteLine("Database exists");
            }
            else
            {
                Console.WriteLine("Database Doesnt Exist");
            }

           /* SQLiteConnection.CreateFile("CodingLogDB.db3"); //Create a Database
            SQLiteConnection conn = new SQLiteConnection(@"data source = CodingLog.db3"); //Establish a connection with our created DB
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand("create table table1 (date TEXT, hours INT)", conn);
            cmd.ExecuteNonQuery();
            Console.WriteLine("Enter Date");
            string date = Console.ReadLine();
            Console.WriteLine("Enter Hours Done");
            string hours = Console.ReadLine();
            SQLiteCommand cmd2 = new SQLiteCommand("insert into table1 (date, hours) values ('" + date + "', '" + hours + "')", conn);
            cmd2.ExecuteNonQuery();*/
        }
    }
}
