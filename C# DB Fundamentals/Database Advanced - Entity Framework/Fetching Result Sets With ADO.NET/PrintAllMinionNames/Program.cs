using System;
using System.Data.SqlClient;

namespace PrintAllMinionNames
{
    class Program
    {
        const string connectionString =
             @"Server=DESKTOP-JU304LN\SQLEXPRESS;initial catalog=MinionsDB;Integrated Security=true";

        static void Main(string[] args)
        {
            int first = 1;
            int last = 1;
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string lastIdQuery = "SELECT COUNT(Id) FROM Minions";

                using (SqlCommand command = new SqlCommand(lastIdQuery,connection))
                {
                    last = (int)command.ExecuteScalar();
                }
                int count = last;
                bool firstRow = true;
                do
                {
                    if (firstRow)
                    {
                        Console.WriteLine(TakingTheNameById(first,connection)); 
                        firstRow = false;
                        first++;
                    }
                    else
                    {
                        firstRow = true;
                        Console.WriteLine(TakingTheNameById(last, connection));
                        last--;
                    }
                    count--;
                } while (count != 0);
            }
        }

        public static string TakingTheNameById(int Id,SqlConnection connection)
        {
            string takingNameQuery = $"SELECT Name FROM Minions WHERE Id = {Id}";
            using (SqlCommand command = new SqlCommand(takingNameQuery,connection))
            {
                return command.ExecuteScalar().ToString();
            }
        }
    }


}
