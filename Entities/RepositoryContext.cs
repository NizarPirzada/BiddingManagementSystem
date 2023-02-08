using Entities.Models;
using Microsoft.EntityFrameworkCore;
using PanacealogicsSales.Entities.Models;

namespace Entities
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options)
            : base(options)
        {
        }


        public DbSet<Account> Accounts { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<SocialUser> Social_User { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<Skill> Skill { get; set; }
        public DbSet<ProjectSkill> Project_Skill { get; set; }
        public DbSet<UserProject> User_Project { get; set; }
        public DbSet<Thread> thread { get; set; }
        public DbSet<ThreadMessage> thread_message { get; set; }
        public DbSet<ThreadHistory> thread_history { get; set; }
        public DbSet<UserShift> user_shift { get; set; }
        public DbSet<Proposal> proposal { get; set; }
    }
}
