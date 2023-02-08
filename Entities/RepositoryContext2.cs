using Entities.Models;
using Microsoft.EntityFrameworkCore;
using PanacealogicsSales.Entities.Models;

namespace Entities
{
    public class RepositoryContext2 : DbContext
    {
        public RepositoryContext2(DbContextOptions<RepositoryContext2> options)
            : base(options)
        {
        }


        public DbSet<Project2> Project2 { get; set; }
     
    }
}
