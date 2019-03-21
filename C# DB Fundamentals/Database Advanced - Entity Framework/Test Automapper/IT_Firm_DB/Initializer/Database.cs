using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace IT_Firm_DB.Initializer
{
    internal class Database
    {
        internal DbContext DbContext { get; private set; }

        internal void ConfigureContext<T> (string contextAssemblyName) 
            where T : DbContext
        {
            IServiceProvider serviceProvider = ConfigureServices<T>(contextAssemblyName);

            var context = serviceProvider.GetService<T>();

           this.DbContext =  context;
        }

        private IServiceProvider ConfigureServices<T>(string contextAssemblyName) 
            where T : DbContext
        {
            IServiceCollection collection = new ServiceCollection();

            return collection.AddDbContext<T>(option =>
            {
                option.UseSqlServer(Connection.ConnectionStr, s => s.MigrationsAssembly(contextAssemblyName));
            })
             .BuildServiceProvider();
        }

    }
}
