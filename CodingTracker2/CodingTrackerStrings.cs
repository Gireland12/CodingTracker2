using System;
using System.IO;
using System.Data.SQLite;
using System.Globalization;

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
            string WelcomeAnswer;

            Console.WriteLine("----------------------------------------");
            Console.WriteLine("|        Main Menu                      |");
            Console.WriteLine("|        1. Log coding hours...         |");
            Console.WriteLine("|        2. View coding log...          |");
            Console.WriteLine("|        3. Edit coding log...          |");
            Console.WriteLine("|        0. Quit...                     |");
            Console.WriteLine("----------------------------------------");

            WelcomeAnswer = (Console.ReadLine());

            if (WelcomeAnswer == "1")
            {
                AddRow();
            }

            if (WelcomeAnswer == "2")
            {
                Console.WriteLine();
                ReadTable();
                Console.WriteLine();
                Console.WriteLine("Press any key to return to the main menu or press 0 to QUIT");
                string UserAnswer = (Console.ReadLine());
                if (UserAnswer != "0")
                {
                    WelcomeMessage();
                }
                else
                {
                    Console.WriteLine("GoodBye!");
                    Environment.Exit(0);
                }
            }

            if (WelcomeAnswer == "3")
            {
                EditRow();
                Console.WriteLine();
                Console.WriteLine("Press any key to return to the main menu or press 0 to QUIT");
                string UserAnswer = (Console.ReadLine());
                if (UserAnswer != "0")
                {
                    WelcomeMessage();
                }
                else
                {
                    Console.WriteLine("GoodBye!");
                    Environment.Exit(0);
                }
            }

            if (WelcomeAnswer == "0")
            {
                Console.WriteLine("-------------");
                Console.WriteLine("| GoodBye!  |");
                Console.WriteLine("-------------");
                Environment.Exit(0);
            }

            if (WelcomeAnswer != "0" && WelcomeAnswer != "1" && WelcomeAnswer != "2" && WelcomeAnswer != "3")
            {
                Console.WriteLine("Not a valid answer, please try again");
                WelcomeMessage();
            }
        }

        static void CreateTable()
        {
            string dbFile = "URI=file:CodingTrackerDB.db";
            SQLiteConnection connection = new SQLiteConnection(dbFile);
            connection.Open();
            string tbl = "create table if not exists CodeTracker (DATE text, HOURS text);";
            SQLiteCommand command = new SQLiteCommand(tbl, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }

        static void AddRow()
        {

            bool parseSuccess1 = false;
            bool parseSuccess2 = false;
            string DateString = "0", HoursString = "0";

            do
            {
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("|  What Date is your entry for? dd-MM-yyyy  |");
                Console.WriteLine("|  Or press '0' to go back to main menu...  |");
                Console.WriteLine("--------------------------------------------");

                DateString = Console.ReadLine();

                DateTime dDate;

                if (DateTime.TryParseExact(DateString, "dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out dDate))
                {
                    parseSuccess1 = true;
                }

                if (DateString == "0")
                {
                    WelcomeMessage();
                }

                if (!parseSuccess1)
                {
                    Console.WriteLine("--------------------------------------------");
                    Console.WriteLine("|               Invalid date                |");
                    Console.WriteLine("--------------------------------------------");
                }


            } while (!parseSuccess1);
            do
            {
                Console.WriteLine("-----------------------------------------------");
                Console.WriteLine("| How many hours would you like to log? HH:mm  |");
                Console.WriteLine("|Or press '0' to go back to main menu...       |");
                Console.WriteLine("-----------------------------------------------");

                HoursString = Console.ReadLine();
                DateTime dTime;


                if (DateTime.TryParse(HoursString, out dTime))
                {
                    String.Format("{0:HH:mm}", dTime);
                    parseSuccess2 = true;
                }

                if (HoursString == "0")
                {
                    WelcomeMessage();
                }
                if (!parseSuccess2)
                {
                    Console.WriteLine("--------------------------------------------");
                    Console.WriteLine("|               Invalid time                |");
                    Console.WriteLine("--------------------------------------------");
                }


            } while (!parseSuccess2);

           HoursString = HoursString.PadLeft(5, '0');


            string dbFile = "URI=file:CodingTrackerDB.db";
            SQLiteConnection connection = new SQLiteConnection(dbFile);
            connection.Open();
            string AddHours = $"insert into CodeTracker (DATE,HOURS) values ('{DateString}','{HoursString}');";
            SQLiteCommand command = new SQLiteCommand(AddHours, connection);
            command.ExecuteNonQuery();
            connection.Close();

            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("|                   Log added succesfuly                        |");
            Console.WriteLine("|                                                               |");
            Console.WriteLine("|Press any key to return to the main menu or press 0 to QUIT... |");
            Console.WriteLine("|                                                               |");
            Console.WriteLine("----------------------------------------------------------------");

            string UserAnswer = (Console.ReadLine());

            if (UserAnswer != "0")
            {
                WelcomeMessage();
            }
            else
            {
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("|               Goodbye!                   |");
                Console.WriteLine("--------------------------------------------");
                Environment.Exit(0);
            }


        }


        //static void DeleteRow()
        /*{
            string dbFile = "URI=file:CodingTrackerDB.db";
            SQLiteConnection connection = new SQLiteConnection(dbFile);
            connection.Open();
            string DeleteHours = "Delete from CodeTracker where date=15112021;";
            SQLiteCommand command = new SQLiteCommand(DeleteHours, connection);
            command.ExecuteNonQuery();
            connection.Close();
            Console.WriteLine("Row deleted succesfuly");
            Console.ReadLine();
        }*/

        static void EditRow()
        {
            int  InputDate;
            string DateString, HoursString;
            bool parseSuccess1 = false;
            bool parseSuccess2 = false;
            DateTime dDate;

            do
            {
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine("| What date would you like to edit? dd-MM-yyyy          |");
                Console.WriteLine("|Or press '0' to go back to main menu...                |");
                Console.WriteLine("---------------------------------------------------------");

                DateString = (Console.ReadLine());


                if (DateTime.TryParseExact(DateString, "dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out dDate))
                {
                    parseSuccess1 = true;
                }

                if (DateString == "0")
                {
                    WelcomeMessage();
                }

                if (!parseSuccess1)
                {
                    Console.WriteLine("-----------------");
                    Console.WriteLine("|  Invalid date  |");
                    Console.WriteLine("-----------------");
                }
            } while (!parseSuccess1);

            do
            {

                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("|How many hours would you like to log?     |");
                Console.WriteLine("|Or press '0' to go back to main menu...   |");
                Console.WriteLine("--------------------------------------------");
                
                HoursString = (Console.ReadLine());
               

                if (DateTime.TryParseExact(HoursString, "HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out dDate))
                {
                    parseSuccess2 = true;
                }

                if (HoursString == "0")
                {
                    WelcomeMessage();
                }
                if (!parseSuccess2)
                {
                    Console.WriteLine("-----------------");
                    Console.WriteLine("|  Invalid time |");
                    Console.WriteLine("-----------------");
                }
                
                HoursString = HoursString.PadLeft(5, '0');

                while (parseSuccess2)
                {
                    string dbFile = "URI=file:CodingTrackerDB.db";
                    SQLiteConnection connection = new SQLiteConnection(dbFile);
                    connection.Open();
                    string EditHours = $"update CodeTracker set HOURS='{HoursString}' where DATE= '{DateString}';";
                    SQLiteCommand command = new SQLiteCommand(EditHours, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    Console.WriteLine("Row edited succesfuly");
                    Console.WriteLine();
                    Console.WriteLine("Press any key to return to the main menu or press 0 to QUIT");

                    string UserAnswer = (Console.ReadLine());
                    if (UserAnswer != "0")
                    {
                        WelcomeMessage();
                    }
                    else
                    {
                        Console.WriteLine("GoodBye!");
                        Environment.Exit(0);
                    }
                }
            } while (!parseSuccess2);
                {
                    Console.WriteLine("Not a valid answer, please try again");

                    HoursString = default(string);
                    parseSuccess2 = false;

                    Console.WriteLine("How many hours would you like to log?");
                    HoursString = (Console.ReadLine());

                    parseSuccess2 = int.TryParse(DateString, out InputDate);
                }

            
        }

        static void ReadTable()
        {
            string dbFile = "URI=file:CodingTrackerDB.db";
            SQLiteConnection connection = new SQLiteConnection(dbFile);
            connection.Open();

            string stm = "SELECT * FROM CodeTracker LIMIT 5";

            using var command = new SQLiteCommand(stm, connection);
            using SQLiteDataReader rdr = command.ExecuteReader();

            Console.WriteLine("----------------------------------");
            Console.WriteLine("DD/MM/YYYY                  HOURS");

            while (rdr.Read())
            {

                Console.WriteLine($"{rdr.GetString(0)}                 {rdr.GetString(1)}");

            }
            Console.WriteLine("----------------------------------");
        }
    }
}
