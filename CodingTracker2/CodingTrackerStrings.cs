using System;
using System.IO;
using System.Data.SQLite;
using System.Globalization;
using ConsoleTableExt;
using System.Collections.Generic;
using System.Configuration;


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

            var MenuTable = new List<List<object>>
                {
                    new List<object>{ "1. Log coding hours..."},
                    new List<object>{ "2. View coding log..."},
                    new List<object>{ "3. Edit coding log..."},
                    new List<object>{ "4. Delete from coding log..."},
                    new List<object>{ "0. Quit... "},
                };

            ConsoleTableBuilder
                .From(MenuTable)
                .WithTitle("MAIN MENU", ConsoleColor.Green, ConsoleColor.Black)
                .WithTextAlignment(new Dictionary<int, TextAligntment>
                    {
                            {2, TextAligntment.Center}
                    })
                .WithCharMapDefinition(new Dictionary<CharMapPositions, char> {
                    {CharMapPositions.BottomLeft, '=' },
                    {CharMapPositions.BottomRight, '=' },
                    {CharMapPositions.BorderTop, '=' },
                    {CharMapPositions.BorderBottom, '=' },
                    {CharMapPositions.BorderLeft, '|' },
                    {CharMapPositions.BorderRight, '|' },
                })
                .ExportAndWriteLine();

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
                Console.WriteLine("---------------------------------------------------------------");
                Console.WriteLine("|  Press any key to return to the main menu or press 0 to QUIT |");
                Console.WriteLine("---------------------------------------------------------------");
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
                Console.WriteLine("---------------------------------------------------------------");
                Console.WriteLine("|  Press any key to return to the main menu or press 0 to QUIT |");
                Console.WriteLine("---------------------------------------------------------------");
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
            if (WelcomeAnswer == "4")
            {
                DeleteRow();
                Console.WriteLine();
                Console.WriteLine("---------------------------------------------------------------");
                Console.WriteLine("|  Press any key to return to the main menu or press 0 to QUIT |");
                Console.WriteLine("---------------------------------------------------------------");
                string UserAnswer = (Console.ReadLine());
                if (UserAnswer != "0")
                {
                    WelcomeMessage();
                }
                else
                {
                    Console.WriteLine("-------------");
                    Console.WriteLine("| GoodBye!  |");
                    Console.WriteLine("-------------");
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
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("| Not a valid answer, please try again |");
                Console.WriteLine("----------------------------------------");
                WelcomeMessage();
            }
        }

        static void CreateTable()
        {
            string dbFile = "URI=FILE:CodingTrackerDB.db";
            SQLiteConnection connection = new SQLiteConnection(dbFile);
            connection.Open();
            string tbl = "CREATE TABLE IF NOT EXISTS codetracker (DATE text, HOURS text);";
            SQLiteCommand command = new SQLiteCommand(tbl, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }

        static void AddRow()
        {

            bool parseSuccess1 = false;
            bool parseSuccess2 = false;
            string DateString = "0", HoursString = "0";
            string dbFile = "URI=FILE:CodingTrackerDB.db";
            int count = 0;
            

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
                    SQLiteConnection connection2 = new SQLiteConnection(dbFile);
                    connection2.Open();
                    string CheckDate = $"SELECT count(*) FROM codetracker WHERE DATE= '{DateString}';";
                    SQLiteCommand command2 = new SQLiteCommand(CheckDate, connection2);
                    
                    count = Convert.ToInt32(command2.ExecuteScalar());
                   
                    connection2.Close();

                    if(count > 0)
                    {
                        Console.WriteLine("----------------------------------------------------------");
                        Console.WriteLine("| Date already exists, please update with EDIT menu item |");
                        Console.WriteLine("----------------------------------------------------------");
                        
                        WelcomeMessage();
                    }
                    else
                    {
                        parseSuccess1 = true;
                    }
                    

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


            SQLiteConnection connection = new SQLiteConnection(dbFile);
            connection.Open();
            string AddHours = $"INSERT INTO codetracker (DATE,HOURS) VALUES ('{DateString}','{HoursString}');";
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


        static void DeleteRow()
        {
            string DateString;
            bool parseSuccess1 = false;
            DateTime dDate;

            do
            {
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine("| What date would you like to delete? dd-MM-yyyy        |");
                Console.WriteLine("|  Or press '0' to go back to main menu...              |");
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

            string dbFile = "URI=FILE:CodingTrackerDB.db";
            SQLiteConnection connection = new SQLiteConnection(dbFile);
            connection.Open();
            string DeleteHours = $"DELETE FROM codetracker WHERE DATE= '{DateString}';";
            SQLiteCommand command = new SQLiteCommand(DeleteHours, connection);
            command.ExecuteNonQuery();
            connection.Close();

            Console.WriteLine("----------------------------");
            Console.WriteLine("|  Row deleted succesfuly  |");
            Console.WriteLine("----------------------------");
        }
        

        static void EditRow()
        {
            int  InputDate;
            string DateString, HoursString;
            bool parseSuccess1 = false;
            bool parseSuccess2 = false;
            int count = 0;
            DateTime dDate;
            string dbFile = "URI=FILE:CodingTrackerDB.db";

            do
            {
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine("| What date would you like to edit? dd-MM-yyyy          |");
                Console.WriteLine("|Or press '0' to go back to main menu...                |");
                Console.WriteLine("---------------------------------------------------------");

                DateString = (Console.ReadLine());


                if (DateTime.TryParseExact(DateString, "dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out dDate))
                {
                    SQLiteConnection connection2 = new SQLiteConnection(dbFile);
                    connection2.Open();
                    string CheckDate = $"SELECT count(*) FROM codetracker WHERE DATE= '{DateString}';";
                    SQLiteCommand command2 = new SQLiteCommand(CheckDate, connection2);

                    count = Convert.ToInt32(command2.ExecuteScalar());

                    connection2.Close();

                    if (count > 0)
                    {
                        parseSuccess1 = true;
                    }
                    else
                    {
                        Console.WriteLine("----------------------------------------------------------");
                        Console.WriteLine("|          Date does not exist, please try again          |");
                        Console.WriteLine("----------------------------------------------------------");

                        EditRow();
                        
                    }
                    
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

                Console.WriteLine("----------------------------------------------");
                Console.WriteLine("| How many hours would you like to log? HH:mm |");
                Console.WriteLine("| Or press '0' to go back to main menu...     |");
                Console.WriteLine("----------------------------------------------");
                
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
                    SQLiteConnection connection = new SQLiteConnection(dbFile);
                    connection.Open();
                    string EditHours = $"UPDATE codetracker SET HOURS='{HoursString}' WHERE DATE= '{DateString}';";
                    SQLiteCommand command = new SQLiteCommand(EditHours, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.WriteLine("|                    Row edited succesfuly                     |"); 
                    Console.WriteLine("|                                                              |");
                    Console.WriteLine("| Press any key to return to the main menu or press 0 to QUIT  |");
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
            } while (!parseSuccess2);
                {
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("|    Not a valid answer, please try again   |");
                Console.WriteLine("--------------------------------------------");

                HoursString = default(string);
                parseSuccess2 = false;

                
                HoursString = (Console.ReadLine());

                if(HoursString == "0")
                {
                    WelcomeMessage();
                }

                    parseSuccess2 = int.TryParse(DateString, out InputDate);
                }

            
        }


        static void ReadTable()
        {
            string dbFile = "URI=FILE:CodingTrackerDB.db";
            var connection = new SQLiteConnection(dbFile);

            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = "SELECT * FROM codetracker";

            List<CodeTracker> tableData = new List<CodeTracker>();

            SQLiteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                        new CodeTracker
                        {
                            Date = new string(reader.GetString(0)),
                            Hours = new string(reader.GetString(1))
                        });  
                    }
                }
                else
                {
                    Console.WriteLine("No data found.");
                }
                reader.Close();

            ConsoleTableBuilder
                .From(tableData)
                .WithTitle("Coding Tracker ", ConsoleColor.Green, ConsoleColor.Black)
                .WithTextAlignment(new Dictionary<int, TextAligntment>
                    {
                            {2, TextAligntment.Center}
                    })
                  .WithCharMapDefinition(new Dictionary<CharMapPositions, char> {
                        {CharMapPositions.BottomLeft, '=' },
                        {CharMapPositions.BottomCenter, '=' },
                        {CharMapPositions.BottomRight, '=' },
                        {CharMapPositions.BorderTop, '=' },
                        {CharMapPositions.BorderBottom, '=' },
                        {CharMapPositions.BorderLeft, '|' },
                        {CharMapPositions.BorderRight, '|' },
                        {CharMapPositions.DividerY, '|' },
                        {CharMapPositions.DividerX, '-' },
                    })

                .ExportAndWriteLine();
                
            Console.WriteLine("");
            WelcomeMessage();
        }
    }
}

