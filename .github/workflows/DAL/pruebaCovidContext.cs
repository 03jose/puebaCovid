using pruebaCovid.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace pruebaCovid.DAL
{
    public class pruebaCovidContext : DbContext
    {
        public pruebaCovidContext() : base("DefaultConnection")
        {
        }

        public DbSet<Regions> Regions { get; set; }
        public DbSet<Provinces> Provinces { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}