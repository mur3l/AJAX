﻿namespace AdvancedAjax.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }
    }
}
