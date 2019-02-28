using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (SoftUniContext dbContext = new SoftUniContext())
            {
                Console.WriteLine(EmployeesInfo.GetAddressesByTown(dbContext));
            }
        }
    }
}
