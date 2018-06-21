using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebServiceForAngular.DAL.Models;

namespace WebServiceForAngular.DAL.EF
{
    public partial class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions/*<ApplicationDbContext>*/ options)
            : base(options)
        {
        }

        public DbSet<Post> Post { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<CheckItem> CheckItem { get; set; }
        public DbSet<CheckListPost> CheckListPost { get; set; }
        public DbSet<UserPost> UserPost { get; set; }
        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //                optionsBuilder.UseSqlServer("Data Source=NASTYUHA;Initial Catalog=DBforAngular;Integrated Security=True");
        //            }
        //        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserId");
            });
            modelBuilder.Entity<CheckItem>(entity =>
            {
                entity.HasOne(d => d.CheckListPost)
                    .WithMany(p => p.CheckList)
                    .HasForeignKey(d => d.CheckListPostId)
                    .HasConstraintName("FK_CheckListPostId");
            });
        }
    }
}
