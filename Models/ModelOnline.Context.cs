﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class techflowEntities : DbContext
    {
        public techflowEntities()
            : base("name=techflowEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<AccessMaster> AccessMasters { get; set; }
        public DbSet<AccessPassMaster> AccessPassMasters { get; set; }
        public DbSet<AccessTran> AccessTrans { get; set; }
        public DbSet<CompanyMaster> CompanyMasters { get; set; }
        public DbSet<DataCenterMaster> DataCenterMasters { get; set; }
        public DbSet<DeliveryMaster> DeliveryMasters { get; set; }
        public DbSet<InvoiceMaster> InvoiceMasters { get; set; }
        public DbSet<PowerUsageMaster> PowerUsageMasters { get; set; }
        public DbSet<QuotationMaster> QuotationMasters { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<TicketMaster> TicketMasters { get; set; }
        public DbSet<TicketStatusMaster> TicketStatusMasters { get; set; }
        public DbSet<TicketTran> TicketTrans { get; set; }
        public DbSet<TicketTypeMaster> TicketTypeMasters { get; set; }
        public DbSet<UserMaster> UserMasters { get; set; }
    }
}
