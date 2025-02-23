﻿using EFCoreInMemoryDemo.DataModel;
using Microsoft.EntityFrameworkCore;

namespace EFCoreInMemoryDemo.DatabaseContext
{
    public class MyDatabaseContext:DbContext
    {
        private readonly IConfiguration configuration;
        public MyDatabaseContext(IConfiguration _config)
        {
            configuration = _config;
        }
        protected override void OnConfiguring
        (DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseInMemoryDatabase(databaseName: "ProductDb");
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductDataModel>()
                .ToTable(name: "Products", Productstable => Productstable.IsTemporal());
        }

        public DbSet<ProductDataModel> Products { get; set; }
    }
}
