using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace ChangeTownNamesCasing
{
    class Program
    {
        const string connectionString =
          @"Server=DESKTOP-JU304LN\SQLEXPRESS;initial catalog=MinionsDB;Integrated Security=true";

        static void Main(string[] args)
        {
            string countryName = Console.ReadLine();

            if (string.IsNullOrEmpty(countryName) || string.IsNullOrWhiteSpace(countryName))
            {
                Console.WriteLine("Please write the name of the country");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                IList<string> changedTowns = new List<string>();

                
                string countryInfoQuery = $"SELECT c.Id AS CountryId FROM Countries AS c WHERE Name = '{countryName}'";
                int affectedRows = 0;
                int countryId = 0;
                using (SqlCommand command = new SqlCommand(countryInfoQuery,connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        int checkerForIdsCount = 0;

                        if (reader.HasRows == false)
                        {
                            Console.WriteLine("There is not a country with that name!");
                            return;
                        }

                        while (reader.Read())
                        {
                            try
                            {
                                checkerForIdsCount++;
                                countryId = (int)reader["CountryId"];
                                if (checkerForIdsCount > 1)
                                {
                                    Console.WriteLine("There were more than one countries with that name");
                                    return;
                                }
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("The countryId is probably not integer.");
                                return;
                            }
                        }
                    }

                    string changeTownNamesQuery = $"UPDATE Towns SET Name = UPPER(Name) WHERE CountryId = {countryId}";
                        
                        using (SqlCommand secondCommand = new SqlCommand(changeTownNamesQuery,connection))
                        {
                            affectedRows = secondCommand.ExecuteNonQuery();

                            if (affectedRows < 1)
                            {
                                Console.WriteLine("There weren't affected rows!");
                                return;
                            }
                        }

                        string takingTownNames = $"SELECT DISTINCT Name FROM Towns WHERE CountryId = {countryId}";

                        using (SqlCommand thirdCommand = new SqlCommand(takingTownNames,connection))
                        {
                            using (SqlDataReader dataReader = thirdCommand.ExecuteReader())
                            { 
                                if (dataReader.HasRows == false)
                                {
                                    Console.WriteLine("There is not a country with that id!");
                                    return;
                                }

                                while (dataReader.Read())
                                {
                                    changedTowns.Add((string)dataReader["Name"]);

                                    if (changedTowns.Last() == "")
                                    {
                                        changedTowns.Remove("");
                                    }
                                }
                            }
                        }
                    

                    Console.WriteLine($"{changedTowns.Count} town names were affected.");
                    Console.WriteLine($"[{string.Join(", ",changedTowns)}]");
                }
            }
        }
    }
}
