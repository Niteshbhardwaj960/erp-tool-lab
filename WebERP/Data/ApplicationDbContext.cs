﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebERP.Models;
//using WebERP.Models.Location;
using WebERP.Models;
using WebERP.Models.GateEntry;

namespace WebERP.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CountryModel> Countries { get; set; }
        public DbSet<StateModel> States { get; set; }
        public DbSet<CityModel> Cities { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Account_Master> Account_Masters { get; set; }
        public DbSet<Term_Master> Term_Master { get; set; }
        public DbSet<UOM_MASTER> UOM_MASTER { get; set; }
        public DbSet<Brand_Master> Brand_Master { get; set; }
        public DbSet<Item_Master> Item_Master { get; set; }
        public DbSet<Artical_Master> Artical_Master { get; set; }
        public DbSet<Size_Master> Size_Master { get; set; }


        public DbSet<POHeaderModel> POHeader_Master { get; set; }
        public DbSet<PODetailModel> PODetail_Master { get; set; }
        public DbSet<POTermsModel> POTerm_Master { get; set; }

        public DbSet<JobWorkIssueHdr> JobWorkIssue_Header { get; set; }
        public DbSet<JobWorkIssueDet> JobWorkIssue_Details { get; set; }
        //protected void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<PODetail_Master>().Property(x => x.SnachCount).HasPrecision(16, 3);
        //    modelBuilder.Entity<PODetail_Master>().Property(x => x.MinimumStock).HasPrecision(16, 3);
        //    modelBuilder.Entity<PODetail_Master>().Property(x => x.MaximumStock).HasPrecision(16, 3);
        //}

        public DbSet<Process_Master> Process_Master { get; set; }
        public DbSet<ProcessRate_Master> ProcessRate_Master { get; set; }
        public DbSet<Godown_Master> Godown_Master { get; set; }
        public DbSet<Gate_HDR> Gate_HDR { get; set; }
        //public DbSet<POHeaderModel> POHeaderModel { get; set; }
        //public DbSet<PODetailModel> PODetailModel { get; set; }
        public DbSet<V_PODetails> V_PODetails { get; set; }
        public DbSet<V_CITY_DTL> V_CITY_DTL { get; set; }
        public DbSet<GateEntryDetail> gateEntryDetails { get; set; }
        public DbSet<V_GateEntryDetail> V_GateEntryDetail { get; set; }
        public DbSet<Department_Master> Department_Masters { get; set; }
        public DbSet<Employee_Master> Employee_Masters { get; set; }
        public DbSet<StockDTL_Model> StockDTL_Models { get; set; }
        public DbSet<Cutting_Order> Cutting_Orders { get; set; }
        public DbSet<V_CuttingDetail> V_CuttingDetail { get; set; }
        public DbSet<V_RM_DTL> V_RM_DTL { get; set; }
        public DbSet<RM_HDR> RM_HDR { get; set; }
        public DbSet<RM_DTL> RM_DTL { get; set; }
        public DbSet<Cutting_Receipt> Cutting_Receipt { get; set; }
        public DbSet<MGF_RECEIPT> MGF_RECEIPT { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<Employee_Attandance> Employee_Attandance { get; set; }
        public DbSet<Employee_Advance> Employee_Advance { get; set; }
        public DbSet<AgentCommRate> AgentCommRate { get; set; }
        public DbSet<V_GATE_ENTRY_ACC> V_GATE_ENTRY_ACC { get; set; }
        public DbSet<V_JW_DTL> V_JW_DTL { get; set; }

        public DbSet<SalesHeader> SalesHeader { get; set; }
        public DbSet<SalesDetail> SalesDetails { get; set; }

        public DbSet<TAX_MASTER> TAX_MASTER { get; set; }
        public DbSet<V_LEDGER> V_LEDGER { get; set; }
        public DbSet<Artical_Merge_DTL> Artical_Merge_DTL { get; set; }
        public DbSet<Artical_Merge_HDR> Artical_Merge_HDR { get; set; }
        public DbSet<Emp_Sal> EMP_SAL { get; set; }
        public DbSet<Emp_Sal_PC_Cont_Dtl> Emp_Sal_PC_Cont_Dtl { get; set; }
        public DbSet<V_PRODUCTION_DETAIL> V_PRODUCTION_DETAIL { get; set; }
        public DbSet<RMR_HDR> RMR_HDR { get; set; }
        public DbSet<RMR_DTL> RMR_DTL { get; set; }
        public DbSet<V_RM_ISSUE> V_RM_ISSUE { get; set; }
    }

}
