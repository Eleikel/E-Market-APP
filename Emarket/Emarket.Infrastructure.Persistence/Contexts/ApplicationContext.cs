using Emarket.Core.Domain.Common;
using Emarket.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Emarket.Infrastructure.Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }



        DbSet<Category> Categories { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Advertisement> Advertisements { get; set; }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = DateTime.Now;
                        entry.Entity.CreatedBy = "DefaultAppUser";
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.Now;
                        entry.Entity.LastModifiedBy = "DefaultAppUser";
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }


        //Fluent Api

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region tables
            modelBuilder.Entity<Category>()
                .ToTable("Categories");

            modelBuilder.Entity<User>()
                .ToTable("Users");

            modelBuilder.Entity<Advertisement>()
                .ToTable("Advertisements");
            #endregion


            #region "primary keys"
            modelBuilder.Entity<Category>()
                .HasKey(category => category.Id);

            modelBuilder.Entity<User>()
                .HasKey(user => user.Id);

            modelBuilder.Entity<Advertisement>()
                .HasKey(advertisement => advertisement.Id);
            #endregion


            #region "Relationships"
            modelBuilder.Entity<User>()
                .HasMany<Advertisement>(user => user.Advertisements)
                .WithOne(advertisement => advertisement.User)
                .HasForeignKey(advertisement => advertisement.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>()
                .HasMany<Advertisement>(category => category.Advertisements)
                .WithOne(advertisement => advertisement.Category)
                .HasForeignKey(advertisement => advertisement.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion


            #region "Property Configuration"

            #region Category
            modelBuilder.Entity<Category>()
                .Property(category => category.Name)
                .IsRequired();

            modelBuilder.Entity<Category>()
                .Property(category => category.Description)
                .IsRequired();
            #endregion


            #region User

            modelBuilder.Entity<User>()
            .Property(user => user.Username)
            .IsRequired();

            modelBuilder.Entity<User>()
            .Property(user => user.Password)
            .IsRequired();

            modelBuilder.Entity<User>()
                 .Property(user => user.Name)
                 .IsRequired();

            modelBuilder.Entity<User>()
            .Property(user => user.LastName)
            .IsRequired();


            modelBuilder.Entity<User>()
            .Property(user => user.Phone)
            .IsRequired();


            modelBuilder.Entity<User>()
            .Property(user => user.Email)
            .IsRequired();

            #endregion


            #region Advertisement
            modelBuilder.Entity<Advertisement>()
            .Property(advertisement => advertisement.Name)
            .IsRequired();

            modelBuilder.Entity<Advertisement>()
            .Property(advertisement => advertisement.Description)
            .IsRequired();

            modelBuilder.Entity<Advertisement>()
            .Property(advertisement => advertisement.Price)
            .IsRequired();

            #endregion

            #endregion


        }
    }
}
