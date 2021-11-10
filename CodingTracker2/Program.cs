using System;
using System.IO;
using System.Data.SQLite;

namespace CodingTracker2
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateTable();
            WelcomeMessage();

        }

        static void WelcomeMessage()
        {
            int WelcomeAnswer;
            string WelcomeAnswerAsString;
            bool parseSuccess;

            Console.WriteLine("Hello, what would you like to do?");
            Console.WriteLine("1. Log coding hours");
            Console.WriteLine("2. View coding log");
            Console.WriteLine("3. Edit coding log");
            Console.WriteLine("0. To Exit");

            WelcomeAnswerAsString = (Console.ReadLine());

            parseSuccess = int.TryParse(WelcomeAnswerAsString, out WelcomeAnswer);

            while (parseSuccess)
            {
                while (WelcomeAnswer != 0)
                {
                    if (WelcomeAnswer == 1)
                    {
                        Console.WriteLine("You've chosen 1");
                        AddRow();
                        break;
                    }

                    if (WelcomeAnswer == 2)
                    {
                        Console.WriteLine("You've chosen 2");
                        break;
                    }

                    if (WelcomeAnswer == 3)
                    {
                        Console.WriteLine("You've chosen 3");
                        EditRow();
                        break;
                    }

                    if (WelcomeAnswer != 0 && WelcomeAnswer != 1 && WelcomeAnswer != 2 && WelcomeAnswer != 3)
                    {
                        Console.WriteLine("Not a valid answer, please try again");
                        WelcomeAnswer = default(int);
                        Console.WriteLine("Hello, what would yu like to do?");
                        Console.WriteLine("1. Add coding to coding log");
                        Console.WriteLine("2. View coding log");
                        Console.WriteLine("3. Edit coding log");
                        Console.WriteLine("0. To Exit");

                        WelcomeAnswerAsString = (Console.ReadLine());

                        parseSuccess = int.TryParse(WelcomeAnswerAsString, out WelcomeAnswer);
                    }
                }

            } while (!parseSuccess)
                {
                    Console.WriteLine("Not a valid answer, please try again");

                    WelcomeAnswer = default(int);
                    WelcomeAnswerAsString = default(string);
                    parseSuccess = false;

                    Console.WriteLine("Hello, what would yu like to do?");
                    Console.WriteLine("1. Add coding to coding log");
                    Console.WriteLine("2. View coding log");
                    Console.WriteLine("3. Edit coding log");
                    Console.WriteLine("0. To Exit");

                    WelcomeAnswerAsString = (Console.ReadLine());

                    parseSuccess = int.TryParse(WelcomeAnswerAsString, out WelcomeAnswer);
                }
        }

        static void CreateTable()
        {
            string dbFile = "URI=file:CodingTrackerDB.db";
            SQLiteConnection connection = new SQLiteConnection(dbFile);
            connection.Open();
            string tbl = "create table if not exists CodeTracker (DATE integer primary key, HOURS integer);";
            SQLiteCommand command = new SQLiteCommand(tbl, connection);
            command.ExecuteNonQuery();
            connection.Close();

        }

        static void AddRow()
        {
            int Hours, InputDate, InputHours;
            string DateString, HoursString;
            bool parseSuccess1, parseSuccess2;


            Console.WriteLine("What Date is your entry for? DDMMYYYY");
            DateString = (Console.ReadLine());
            parseSuccess1 = int.TryParse(DateString, out InputDate);
            
            while (parseSuccess1)
            {
                Console.WriteLine("How many hours would you like to log? H");
                HoursString = (Console.ReadLine());
                parseSuccess2 = int.TryParse(HoursString, out InputHours);
                while (parseSuccess2)
                {
                    string dbFile = "URI=file:CodingTrackerDB.db";
                    SQLiteConnection connection = new SQLiteConnection(dbFile);
                    connection.Open();
                    string AddHours = $"insert into CodeTracker (DATE,HOURS) values ({InputDate},{InputHours});";
                    SQLiteCommand command = new SQLiteCommand(AddHours, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    Console.WriteLine("Row added succesfuly");
                    Console.ReadLine();
                } while (!parseSuccess2)
                    {
                        Console.WriteLine("Not a valid answer, please try again");

                        InputHours = default(int);
                        HoursString = default(string);
                        parseSuccess2 = false;

                        Console.WriteLine("How many hours would you like to log?");
                        HoursString = (Console.ReadLine());

                        parseSuccess2 = int.TryParse(DateString, out InputDate);

                    }


            } while (!parseSuccess1)
            {
                    Console.WriteLine("Not a valid answer, please try again");

                    InputDate = default(int);
                    DateString = default(string);
                    parseSuccess1 = false;

                    Console.WriteLine("What Date is your entry for?");
                    DateString = (Console.ReadLine());

                    parseSuccess1 = int.TryParse(DateString, out InputDate);

            }
            
        }

        static void DeleteRow()
        {
            string dbFile = "URI=file:CodingTrackerDB.db";
            SQLiteConnection connection = new SQLiteConnection(dbFile);
            connection.Open();
            string DeleteHours = "Delete from CodeTracker where date=15112021;";
            SQLiteCommand command = new SQLiteCommand(DeleteHours, connection);
            command.ExecuteNonQuery();
            connection.Close();
            Console.WriteLine("Row deleted succesfuly");
            Console.ReadLine();
        }

        static void EditRow()
        {
            int Hours, InputDate, InputHours;
            string DateString, HoursString;
            bool parseSuccess1, parseSuccess2;


            Console.WriteLine("What date would you like to edit? Type Below DDMMYYYY");

            DateString = (Console.ReadLine());
            parseSuccess1 = int.TryParse(DateString, out InputDate);

            while (parseSuccess1)
            {
                Console.WriteLine("How many hours would you like to log? H");
                HoursString = (Console.ReadLine());
                parseSuccess2 = int.TryParse(HoursString, out InputHours);
                while (parseSuccess2)
                {
                    string dbFile = "URI=file:CodingTrackerDB.db";
                    SQLiteConnection connection = new SQLiteConnection(dbFile);
                    connection.Open();
                    string EditHours = $"update CodeTracker set HOURS={InputHours} where DATE= {InputDate};";
                    SQLiteCommand command = new SQLiteCommand(EditHours, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    Console.WriteLine("Row edited succesfuly");
                    Console.ReadLine();
                } while (!parseSuccess2)
                    {
                        Console.WriteLine("Not a valid answer, please try again");

                        InputHours = default(int);
                        HoursString = default(string);
                        parseSuccess2 = false;

                        Console.WriteLine("How many hours would you like to log?");
                        HoursString = (Console.ReadLine());

                        parseSuccess2 = int.TryParse(DateString, out InputDate);
                    }

            } while (!parseSuccess1)
                {
                    Console.WriteLine("Not a valid answer, please try again");

                    InputDate = default(int);
                    DateString = default(string);
                    parseSuccess1 = false;

                    Console.WriteLine("What Date is your entry for?");
                    DateString = (Console.ReadLine());

                    parseSuccess1 = int.TryParse(DateString, out InputDate);
                }
        }
    }
}
