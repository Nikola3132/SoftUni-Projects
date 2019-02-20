using System;
using System.Data.SqlClient;

namespace AddMinion
{
    class Program
    {
        const string connectionString =
          @"Server=DESKTOP-JU304LN\SQLEXPRESS;initial catalog=MinionsDB;Integrated Security=true";

        static void Main(string[] args)
        {
            string[] minionInformation = Console.ReadLine()
                  .Split(new char[] {' ',':' }, StringSplitOptions.RemoveEmptyEntries);

            string townName = string.Empty;
            string minionName = string.Empty;
            int minionAge = 0;
            try
            {
                townName = minionInformation[3];
                minionName = minionInformation[1];
                minionAge = int.Parse(minionInformation[2]);
            }
            catch (Exception)
            {
                Console.WriteLine("There was an error in the minion input");
                return;
            }


            string[] villianInformation = Console.ReadLine()
                .Split(new char[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries);

            string villianName = string.Empty;

            try
            {
                villianName = villianInformation[1];
            }
            catch (Exception)
            {
                Console.WriteLine("There was an error in the villian input");
                return;
            }

            string villianQuery = 
                $"SELECT DISTINCT Name FROM Villains WHERE Name = '{villianName}'";

            string townQuery = 
                $"SELECT DISTINCT Name FROM Towns WHERE Name = '{townName}'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                if (CheckerIfExists(townQuery, connection) == false)
                {
                    string townQueryInsert = 
                        $"INSERT INTO Towns (Name) VALUES('{townName}')";

                    AddingIntoDatabase(townQueryInsert, connection);
                    Console.WriteLine($"Town {townName} was added to the database.");
                } 

                if (CheckerIfExists(villianQuery,connection) == false)
                {
                    string villianQueryInsert = 
                        $"INSERT INTO Villains (Name,EvilnessFactorId) VALUES('{villianName}',4)";

                    AddingIntoDatabase(villianQueryInsert, connection);

                    Console.WriteLine($"Villain {villianName} was added to the database.");
             
                }

                string minionTownIdQuery = $"SELECT Id FROM Towns WHERE Name = '{townName}'";
                int townId = 0;
                using (SqlCommand command = new SqlCommand(minionTownIdQuery,connection))
                {
                    townId = (int)command.ExecuteScalar();
                }

                string addingNewMinionQuery = 
                    $"INSERT INTO Minions (Name,TownId) VALUES ('{minionName}',{townId})";

                using (SqlCommand command = new SqlCommand(addingNewMinionQuery,connection))
                {
                    SqlTransaction transaction= command.Transaction;
                    
                    if (command.ExecuteNonQuery() != 1)
                    {
                        Console.WriteLine("Something wrong with the minion adding!");
                        transaction.Rollback("transaction");
                        return;
                    }

                    int minionId = 0;
                    int villainId = 0;

                    using (SqlCommand secondCommand = new SqlCommand($"SELECT Id FROM Minions WHERE Name = '{minionName}'", connection))
                    {
                        minionId = (int)secondCommand.ExecuteScalar();
                    }

                    using (SqlCommand secondCommand = new SqlCommand($"SELECT Id FROM Villains WHERE Name = '{villianName}'", connection))
                    {
                        villainId = (int)secondCommand.ExecuteScalar();
                    }


                    string minionServantQuery = 
                        $"INSERT INTO MinionsVillains (MinionId,VillainId) VALUES({minionId},{villainId})";

                    try
                    {
                        AddingIntoDatabase(minionServantQuery, connection);
                    }
                    catch (Exception)
                    {

                        Console.WriteLine("There is already villian with this minion!");
                        return;
                    }

                    Console.WriteLine($"Successfully added {minionName} to be minion of {villianName}.");
                }
            }
        }
        

        public static bool CheckerIfExists(string query, SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                string objectName = (string)command.ExecuteScalar();

                if (objectName == null)
                {
                    return false;
                }
                return true;
            }
        }

        public static void AddingIntoDatabase(string query, SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand(query,connection))
            {
                command.ExecuteNonQuery();
            }
        }

    }
}
