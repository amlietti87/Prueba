﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.infra.Data.EntityConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.bus;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Event;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Services;
using TECSO.FWK.Domain.UOW;
using Microsoft.Extensions.DependencyInjection;
using ROSBUS.Admin.Domain;
using TECSO.FWK.Infra.Data.Transaction;
using Microsoft.EntityFrameworkCore.Storage;
using TECSO.FWK.Domain.Interfaces.Repositories;
using TECSO.FWK.Infra.Data;

namespace ROSBUS.infra.Data.Contexto
{
    public class AdjuntosContext : BaseContext, IAdjuntosDbContext
    {
        //private IAdminDBResilientTransaction resilientTransaction;

        //private IAdminDBResilientTransaction ResilientTransaction
        //{
        //    get
        //    {
        //        return this.resilientTransaction ?? (this.resilientTransaction = ServiceProviderResolver.ServiceProvider.GetService<IAdminDBResilientTransaction>());
        //    }

        //}


        public AdjuntosContext(DbContextOptions<AdjuntosContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Para regenerar todo el modelo nuevamente correr
            //En package Manager Console lo siguiente
            //scaffold-dbcontext 'Server=172.16.17.59;Database=ROSBUS;User Id=sa; Password=Pa$$w0rd'  Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force 
            //En la carpeta Modelss se van a generar todas las clases.

            modelBuilder.ApplyConfigurationsAdjuntos();


        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            //if (ResilientTransaction.IsResilientTransaction())
            //{
            //    return Task.FromResult(0);
            //}
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            //if (ResilientTransaction.IsResilientTransaction())
            //{
            //    return Task.FromResult(0);
            //} 
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public virtual DbSet<Adjuntos> Adjuntos { get; set; }



        public class AdjuntosDBResilientTransaction : IAdjuntosDBResilientTransaction
        {
            private readonly ResilientTransaction<AdjuntosContext> resilientTransaction;

            public AdjuntosDBResilientTransaction(AdjuntosContext context)
            {
                this.resilientTransaction = ResilientTransaction<AdjuntosContext>.New(context);
            }

            public Task ExecuteAsync(Func<Task> action)
            {
                return this.resilientTransaction.ExecuteAsync(action);
            }

            public bool IsResilientTransaction()
            {
                return this.resilientTransaction.IsResilientTransaction();
            }
        }



    }
}