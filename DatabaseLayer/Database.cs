using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer
{
    public class Database: DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;user=nico622f;password=xrq22baw;database=Svendeprøven;").UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }


        public DbSet<User> Users { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<SaveList> SaveLists { get; set; }
        public DbSet<Item> Items { get; set; }
    }
}
