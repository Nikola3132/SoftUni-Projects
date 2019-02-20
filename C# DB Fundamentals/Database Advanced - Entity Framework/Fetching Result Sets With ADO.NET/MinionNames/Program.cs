using System;
using System.Data.SqlClient;

namespace MinionNames
{
    class Program
    {
        const string connectionString = 
            @"Server=DESKTOP-JU304LN\SQLEXPRESS;initial catalog=MinionsDB;Integrated Security=true";

        static void Main(string[] args)
        {
            int villianId = 0;
            try
            {
                villianId = int.Parse(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string QueryForVillianName = $"SELECT DISTINCT v.Name AS VillianName FROM MinionsVillains AS mv JOIN Villains AS v ON v.Id =mv.VillainId JOIN Minions AS m ON m.Id = mv.MinionId WHERE mv.VillainId = {villianId}";
                string QueryForTheMinions = $"SELECT m.Name AS MinionName, m.Age AS MinionAge FROM MinionsVillains AS mv JOIN Villains AS v ON v.Id =mv.VillainId JOIN Minions AS m ON m.Id = mv.MinionId WHERE mv.VillainId = {villianId}";

                using (SqlCommand command = new SqlCommand(QueryForVillianName, connection))
                {
                    string villianName =(string)command.ExecuteScalar();

                    if (villianName == string.Empty || string.IsNullOrWhiteSpace(villianName))
                    {
                        Console.WriteLine($"No villain with ID {villianId} exists .");
                        return;
                    }

                    Console.WriteLine($"Villian: {villianName}");
                }

                using (SqlCommand cmd = new SqlCommand(QueryForTheMinions,connection))
                {
                    var reader = cmd.ExecuteReader();

                    if (reader.HasRows == false)
                    {
                        Console.WriteLine("There are any minions.");
                        return;
                    }

                    int counter = 1;
                    while (reader.Read())
                    {
                        Console.WriteLine($"{counter}. {reader["MinionName"]} {reader["MinionAge"]}");

                        counter++;
                    }
                    
                }
            }
        }
    }
}
