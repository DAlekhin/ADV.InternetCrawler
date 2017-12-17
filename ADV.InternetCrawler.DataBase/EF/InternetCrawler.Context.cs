﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ADV.InternetCrawler.DataBase.EF
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using System.Collections.Generic;
    
    public partial class InternetCrawlerEntities : DbContext
    {
        public InternetCrawlerEntities(string _connectionString)
            : base(_connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<InternetCrawlerEntities>(null);
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingEntitySetNameConvention>();
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
        }

        public virtual DbSet<DataPoint> DataPoint { get; set; }
        public virtual DbSet<Logger> Logger { get; set; }
        public virtual DbSet<LoggerHeader> LoggerHeader { get; set; }
        public virtual DbSet<PointContent> PointContent { get; set; }
    
        public virtual IEnumerable<Proc_GetDataPointStats> Get_DataPointStats()
        {
            //var test2 = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Proc_GetDataPointStats>("InternetCrawlerEntities.Get_DataPointStats");
            var test = base.Database.SqlQuery<Proc_GetDataPointStats>("Proc_GetDataPointStats");
            //var test2 = this.ExecuteFunction<Proc_GetDataPointStats>("InternetCrawlerEntities.Get_DataPointStats");
            return test;
        }
    }
}
