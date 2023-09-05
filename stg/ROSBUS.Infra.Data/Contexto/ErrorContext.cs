using Microsoft.EntityFrameworkCore;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.infra.Data.EntityConfig;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Infra.Data;

namespace ROSBUS.infra.Data.Contexto
{
    public class ErrorDBContext : BaseContext, IErrorDbContext
    {

        

        public ErrorDBContext(DbContextOptions<ErrorDBContext> options)
            : base(options)
        {

        }



        public DbSet<Error> Errors { get; set; }
        public DbSet<Logs> Logs { get; set; }
        


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Logs>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Error>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            //modelBuilder.ApplyConfiguration<Producto>(new ProductoConfiguration());
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }


        public override int? GetAuditUserId()
        {
            return AuditUserId ?? (AuditUserId = authService.GetCurretUserId());
        }
    }
}
