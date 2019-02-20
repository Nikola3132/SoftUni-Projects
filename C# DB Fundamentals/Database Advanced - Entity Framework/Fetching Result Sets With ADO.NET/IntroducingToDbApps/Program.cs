using System;
using System.Data.SqlClient;

namespace IntroducingToDbApps
{
    class Program
    {
        static void Main(string[] args)
        {
            //**************************
            //Problem 1.Initial Setup
            //**************************



            //**************************
            //Problem 2. Villain Names
            //**************************

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string cmdText =
                    "SELECT v.Name AS VillainName , COUNT(mv.MinionId) AS MinionCount" +
                    "FROM MinionsVillains AS mv " +
                    "JOIN Villains AS v ON v.Id = mv.VillainId " +
                    "GROUP BY v.Name,mv.MinionId HAVING COUNT(mv.MinionId) > 3" +
                    "ORDER BY COUNT(mv.MinionId) DESC";

                using (SqlCommand cmd = new SqlCommand(cmdText, connection))
                {
                    var reader = cmd.ExecuteReader();

                    using (reader)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["VillainName"]} - {reader["MinionCount"]}");
                        }
                    }
                }
            }
        }
    }
}
