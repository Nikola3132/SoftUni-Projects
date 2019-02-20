using System;
using System.Data.SqlClient;

namespace IncreaseAgeStoredProcedure
{
    class Program
    {
        const string connectionString =
             @"Server=DESKTOP-JU304LN\SQLEXPRESS;initial catalog=MinionsDB;Integrated Security=true";

        static void Main(string[] args)
        {
            int minionId = 0;

            if (int.TryParse(Console.ReadLine(), out int res))
            {
                minionId = res;
            }
            else
            {
                Console.WriteLine("Please type the minionId (it should be integer)");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                
                string getOlderQuery = $"EXEC usp_GetOlder {minionId}";
                using (SqlCommand command = new SqlCommand(getOlderQuery, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows == false)
                    {
                        Console.WriteLine("There is not a such minion with that id");
                        return;
                    }
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["Name"]} - {reader["Age"]}");
                    }
                }
            }
        }
    }
}
