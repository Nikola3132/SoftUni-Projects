using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace IT_Firm_DB.Initializer
{
    public class Core<ContextType> where ContextType : DbContext
    {
        public Core(string assemblyContextPath)
        {
            this.Context = Initializing(assemblyContextPath);
            
        }
        public DbContext Context { get; }

        private DbContext Initializing(string assemblyContextPath)
        {
            Database db = new Database();
            db.ConfigureContext<ContextType>(assemblyContextPath);
            var context = (ContextType)db.DbContext;

            return context;
        }
        
    }
}
