using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore;

namespace TodoApi;

public partial class ToDoDbContext : DbContext
{
     protected readonly IConfiguration Configuration;

    public ToDoDbContext(IConfiguration configuration,DbContextOptions<ToDoDbContext> options)
        : base(options)
    {
        Configuration = configuration;
    }
    // public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
    //     : base(options)
    // {
    // }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to mysql with connection string from app settings
        var connectionString = Configuration.GetConnectionString("tododb");
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        // => optionsBuilder.UseMySql(Configuration.GetConnectionString("WebApiDatabase"), Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.36-mysql"));


    // public ToDoDbContext()
    // {
    // }



    public virtual DbSet<Item> Items { get; set; }
        // var connectionString = Configuration.GetConnectionString("WebApiDatabase");


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("items");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
