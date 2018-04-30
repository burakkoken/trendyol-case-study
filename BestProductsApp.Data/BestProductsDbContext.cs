using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BestProductsApp.Data
{
    public class BestProductsDbContext : DbContext
    {
        public BestProductsDbContext(DbContextOptions<BestProductsDbContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Entities.Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entities.Product>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .ValueGeneratedOnAdd()
                    .UseNpgsqlSerialColumn();
            });
        }
    }
}
