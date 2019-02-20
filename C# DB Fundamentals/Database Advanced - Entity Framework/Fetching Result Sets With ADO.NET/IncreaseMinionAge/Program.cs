using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace IncreaseMinionAge
{
    class Program
    {
        const string connectionString =
                @"Server=DESKTOP-JU304LN\SQLEXPRESS;initial catalog=MinionsDB;Integrated Security=true";

        static void Main(string[] args)
        {

            string[] minionIdsSTR = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            int[] selectedIds = new int[minionIdsSTR.Length];
            CheckingAllTheIds(minionIdsSTR, selectedIds);
            
            SqlConnection dbCon = new SqlConnection(connectionString);
            dbCon.Open();

            List<int> minionIds = new List<int>();
            List<string> minionaNames = new List<string>();
            List<int> minionAges = new List<int>();

            using (dbCon)
            {
                SqlCommand command = new SqlCommand($"SELECT * FROM Minions WHERE Id IN ({String.Join(", ", selectedIds)})", dbCon);
                SqlDataReader reader = command.ExecuteReader();

                using (reader)
                {
                    if (!reader.HasRows)
                    {
                        reader.Close();
                        dbCon.Close();
                        Console.WriteLine("There weren't minions with theese Ids");
                        return;
                    }

                    while (reader.Read())
                    {
                        minionIds.Add((int)reader["Id"]);
                        minionaNames.Add((string)reader["Name"]);
                        minionAges.Add((int)reader["Age"]);
                    }
                }

                for (int i = 0; i < minionIds.Count; i++)
                {
                    int id = minionIds[i];
                    string name = String.Join(" ", minionaNames[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList().Select(n => n = char.ToUpper(n.First()) + n.Substring(1).ToLower()).ToArray());
                    int age = minionAges[i] + 1;

                    command = new SqlCommand("UPDATE Minions SET Name = @name, Age = @age WHERE Id = @Id", dbCon);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@age", age);
                    command.Parameters.AddWithValue("@id", id);

                    command.ExecuteNonQuery();
                }

                command = new SqlCommand($"SELECT * FROM Minions", dbCon);
                reader = command.ExecuteReader();

                using (reader)
                {
                    if (!reader.HasRows)
                    {
                        reader.Close();
                        dbCon.Close();
                        return;
                    }

                    while (reader.Read())
                    {
                        Console.WriteLine($"{(int)reader["Id"]} {(string)reader["Name"]} {reader["Age"].ToString()}");
                    }
                }

            }
        }

        public static void CheckingAllTheIds(string[] minionIdsSTR, int[] minionIds)
        {
            for (int i = 0; i < minionIdsSTR.Length; i++)
            {
                if (int.TryParse(minionIdsSTR[i], out int res))
                {
                    minionIds[i] = res;
                }
                else
                {
                    Console.WriteLine("Please ,type only ids(integers) and separate them by a single space ' '");
                    Environment.Exit(0);
                }
            }
        }
    }
}
