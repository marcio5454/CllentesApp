using ClientesApp.Infra.Data.SqlServer.Mappings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientesApp.Infra.Data.SqlServer
{
    public class DataContext: DbContext
    {

        public DataContext(DbContextOptions<DataContext> options): base(options)
        { 
              
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ClienteMap());
        }

    }
}
