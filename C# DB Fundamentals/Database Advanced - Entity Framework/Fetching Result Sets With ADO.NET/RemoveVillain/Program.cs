using System;
using System.Data.SqlClient;

namespace RemoveVillain
{
    class Program
    {
        const string connectionString =
          @"Server=DESKTOP-JU304LN\SQLEXPRESS;initial catalog=MinionsDB;Integrated Security=true";

        static void Main(string[] args)
        {
            int villainId = 0;

            if (int.TryParse(Console.ReadLine(), out int res))
            {
                villainId = res;
            }
            else
            {
                Console.WriteLine("Please type the villainId (it should be integer)");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string villainNameQuery = $"SELECT DISTINCT Name from Villains WHERE Id = {villainId}";
                string villainName = string.Empty;
                int affectedRows = 0;

                using (SqlCommand command = new SqlCommand(villainNameQuery,connection))
                {
                    try
                    {
                        villainName = command.ExecuteScalar().ToString();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("No such villain was found.");
                        return;
                    }
                }

                string releasingMinions = $"DELETE FROM MinionsVillains WHERE VillainId = {villainId}";
                using (SqlCommand command = new SqlCommand(releasingMinions, connection))
                {
                    affectedRows = command.ExecuteNonQuery();
                }

                string deletingTheVillain = $"DELETE FROM Villains WHERE Id = {villainId}";

                using (SqlCommand command = new SqlCommand(deletingTheVillain, connection))
                {
                    command.ExecuteNonQuery();
                }

                Console.WriteLine($"{villainName} was deleted.");
                Console.WriteLine($"{affectedRows} minions were released.");
            }
        }
    }
}
