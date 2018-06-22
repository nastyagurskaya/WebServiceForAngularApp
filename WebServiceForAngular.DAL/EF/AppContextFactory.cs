using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace WebServiceForAngular.DAL.EF
{
    public class AppContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var connection = @"Data Source=NASTYUHA;Initial Catalog=UsersPostDB;Integrated Security=True";
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext> ();
            optionsBuilder.UseSqlServer(connection);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}