﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebERP.Data;

namespace WebERP.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("WebERP.Models.Account_Master", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ACC_TYPE");

                    b.Property<string>("ACTIVE_TAG");

                    b.Property<string>("ADD1")
                        .IsRequired();

                    b.Property<string>("ADD2");

                    b.Property<string>("CITY_CODE");

                    b.Property<string>("CR_DAYS");

                    b.Property<string>("CR_LIMIT");

                    b.Property<string>("Country_Code");

                    b.Property<string>("EMAIL_ID")
                        .IsRequired();

                    b.Property<string>("GST_NO")
                        .IsRequired();

                    b.Property<string>("GST_REGD_TAG");

                    b.Property<DateTime?>("INS_DATE");

                    b.Property<string>("INS_UID");

                    b.Property<string>("MOBILE_NO")
                        .IsRequired();

                    b.Property<string>("NAME")
                        .IsRequired();

                    b.Property<string>("OP_BAL");

                    b.Property<string>("OP_BAL_TAG");

                    b.Property<string>("PH_NO");

                    b.Property<string>("PIN_CODE")
                        .IsRequired();

                    b.Property<string>("REMARKS");

                    b.Property<string>("State_Code");

                    b.Property<DateTime?>("UDT_DATE");

                    b.Property<string>("UDT_UID");

                    b.HasKey("ID");

                    b.ToTable("Account_Masters");
                });

            modelBuilder.Entity("WebERP.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("City");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("WebERP.Models.Artical_Master", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BRAND_CODE");

                    b.Property<DateTime?>("INS_DATE");

                    b.Property<string>("INS_UID");

                    b.Property<string>("NAME")
                        .IsRequired();

                    b.Property<DateTime?>("UDT_DATE");

                    b.Property<string>("UDT_UID");

                    b.HasKey("ID");

                    b.ToTable("Artical_Master");
                });

            modelBuilder.Entity("WebERP.Models.Brand_Master", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ABV")
                        .IsRequired();

                    b.Property<DateTime?>("INS_DATE");

                    b.Property<string>("INS_UID");

                    b.Property<string>("NAME")
                        .IsRequired();

                    b.Property<DateTime?>("UDT_DATE");

                    b.Property<string>("UDT_UID");

                    b.HasKey("ID");

                    b.ToTable("Brand_Master");
                });

            modelBuilder.Entity("WebERP.Models.CityModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CityCode")
                        .HasMaxLength(3);

                    b.Property<DateTime?>("Ins_Date");

                    b.Property<string>("Ins_Uid");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("StateId");

                    b.Property<DateTime?>("Upd_Date");

                    b.Property<string>("Upd_Uid");

                    b.HasKey("Id");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("WebERP.Models.Company", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ABV")
                        .IsRequired();

                    b.Property<string>("ACTIVE_TAG");

                    b.Property<string>("ADD1")
                        .IsRequired();

                    b.Property<string>("ADD2");

                    b.Property<string>("BANK_ACC_NO");

                    b.Property<string>("BANK_BRANCH");

                    b.Property<string>("BANK_NAME");

                    b.Property<string>("CITY_CODE");

                    b.Property<DateTime?>("CST_DAT");

                    b.Property<string>("CST_NO");

                    b.Property<string>("Country_Code");

                    b.Property<string>("ECC_NO");

                    b.Property<string>("EMAIL_ID")
                        .IsRequired();

                    b.Property<string>("ESI_NO");

                    b.Property<string>("FAX_NO");

                    b.Property<string>("GST_NO")
                        .IsRequired();

                    b.Property<string>("IFSC_CODE");

                    b.Property<DateTime>("INS_DATE");

                    b.Property<string>("INS_UID");

                    b.Property<string>("LOGO_NAME");

                    b.Property<DateTime?>("LST_DATE");

                    b.Property<string>("LST_NO");

                    b.Property<string>("MOBILE_NO")
                        .IsRequired();

                    b.Property<string>("MSME_NO");

                    b.Property<string>("NAME")
                        .IsRequired();

                    b.Property<string>("PAN_NO")
                        .IsRequired();

                    b.Property<string>("PF_NO");

                    b.Property<string>("PH_NO");

                    b.Property<string>("PIN_CODE")
                        .IsRequired();

                    b.Property<string>("REMARKS");

                    b.Property<string>("SERVICE_TAX_NO");

                    b.Property<string>("State_Code");

                    b.Property<string>("TDS_NO");

                    b.Property<string>("TIN_NO");

                    b.Property<DateTime>("UDT_DATE");

                    b.Property<string>("UDT_UID");

                    b.Property<string>("URL");

                    b.HasKey("ID");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("WebERP.Models.CountryModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CountryCode")
                        .IsRequired()
                        .HasMaxLength(3);

                    b.Property<DateTime?>("Ins_Date");

                    b.Property<string>("Ins_Uid");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<DateTime?>("Upd_Date");

                    b.Property<string>("Upd_Uid");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("WebERP.Models.Cutting_Order", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ARTICAL_CODE");

                    b.Property<int>("AVG_PC_WEIGHT");

                    b.Property<int>("COMP_CODE");

                    b.Property<int>("CONT_EMP_CODE");

                    b.Property<DateTime?>("DOC_DATE");

                    b.Property<int>("DOC_FINYEAR");

                    b.Property<int>("DOC_NO");

                    b.Property<int>("EMP_CODE");

                    b.Property<DateTime?>("INS_DATE");

                    b.Property<string>("INS_UID");

                    b.Property<int>("ITEM_CODE");

                    b.Property<int>("ORDER_QTY");

                    b.Property<string>("ORDER_STATUS");

                    b.Property<int>("PROC_CODE");

                    b.Property<int>("SIZE_CODE");

                    b.Property<DateTime?>("UDT_DATE");

                    b.Property<string>("UDT_UID");

                    b.Property<int>("WASTAGE_PER");

                    b.HasKey("ID");

                    b.ToTable("Cutting_Orders");
                });

            modelBuilder.Entity("WebERP.Models.Department_Master", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("INS_DATE");

                    b.Property<string>("INS_UID");

                    b.Property<string>("NAME")
                        .IsRequired();

                    b.Property<DateTime?>("UDT_DATE");

                    b.Property<string>("UDT_UID");

                    b.HasKey("ID");

                    b.ToTable("Department_Masters");
                });

            modelBuilder.Entity("WebERP.Models.Employee_Master", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("COMP_CODE");

                    b.Property<int>("DEP_CODE");

                    b.Property<string>("Dep_Name");

                    b.Property<int>("EMP_CODE");

                    b.Property<string>("EMP_NAME")
                        .IsRequired();

                    b.Property<string>("EMP_TYPE")
                        .IsRequired();

                    b.Property<string>("Emp_Father_Name")
                        .IsRequired();

                    b.Property<DateTime?>("INS_DATE");

                    b.Property<string>("INS_UID");

                    b.Property<string>("NAME");

                    b.Property<string>("Remarks");

                    b.Property<DateTime?>("UDT_DATE");

                    b.Property<string>("UDT_UID");

                    b.Property<string>("active_tag");

                    b.Property<DateTime?>("emp_doj")
                        .IsRequired();

                    b.Property<string>("emp_mobile_no1")
                        .IsRequired();

                    b.Property<string>("emp_mobile_no2");

                    b.Property<int>("emp_salary");

                    b.HasKey("ID");

                    b.ToTable("Employee_Masters");
                });

            modelBuilder.Entity("WebERP.Models.GateEntry.GateEntryDetail", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ACC_NAME");

                    b.Property<DateTime?>("Bill_Date");

                    b.Property<string>("Bill_NO");

                    b.Property<DateTime?>("CHL_DATE");

                    b.Property<string>("CHL_NO");

                    b.Property<DateTime?>("DOC_DATE");

                    b.Property<string>("Doc_No");

                    b.Property<string>("FIN_YEAR");

                    b.Property<int>("Fin_Qty");

                    b.Property<int>("Fin_UOM");

                    b.Property<int>("GDW_NO");

                    b.Property<int>("GH_FK");

                    b.Property<DateTime?>("INS_DATE");

                    b.Property<string>("INS_UID");

                    b.Property<int>("Item_Name");

                    b.Property<int>("Item_UOM");

                    b.Property<int>("JW_FK");

                    b.Property<int>("Order_No");

                    b.Property<int>("POD_FK");

                    b.Property<string>("Remarks");

                    b.Property<int>("Stk_Qty");

                    b.Property<int>("Stk_UOM");

                    b.Property<DateTime?>("UDT_DATE");

                    b.Property<string>("UDT_UID");

                    b.HasKey("ID");

                    b.ToTable("gateEntryDetails");
                });

            modelBuilder.Entity("WebERP.Models.Gate_HDR", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Acc_Code");

                    b.Property<string>("Acc_Name");

                    b.Property<int>("Comp_Code");

                    b.Property<DateTime?>("Doc_Date");

                    b.Property<string>("Doc_FN_Year");

                    b.Property<string>("Doc_No");

                    b.Property<DateTime?>("INS_DATE");

                    b.Property<string>("INS_UID");

                    b.Property<string>("Remarks");

                    b.Property<string>("Type");

                    b.Property<DateTime?>("UDT_DATE");

                    b.Property<string>("UDT_UID");

                    b.HasKey("ID");

                    b.ToTable("Gate_HDR");
                });

            modelBuilder.Entity("WebERP.Models.Godown_Master", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ABV");

                    b.Property<string>("GO_DOWN_TYPE");

                    b.Property<DateTime?>("INS_DATE");

                    b.Property<string>("INS_UID");

                    b.Property<string>("NAME")
                        .IsRequired();

                    b.Property<string>("SALE_TAG");

                    b.Property<DateTime?>("UDT_DATE");

                    b.Property<string>("UDT_UID");

                    b.HasKey("ID");

                    b.ToTable("Godown_Master");
                });

            modelBuilder.Entity("WebERP.Models.Item_Master", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ACTIVE_TAG");

                    b.Property<int>("HSN_CODE");

                    b.Property<DateTime?>("INS_DATE");

                    b.Property<string>("INS_UID");

                    b.Property<int>("MAX_STOCK");

                    b.Property<int>("MIN_STOCK");

                    b.Property<string>("NAME")
                        .IsRequired();

                    b.Property<string>("REMARKS");

                    b.Property<DateTime?>("UDT_DATE");

                    b.Property<string>("UDT_UID");

                    b.Property<int>("UOM_CODE");

                    b.HasKey("ID");

                    b.ToTable("Item_Master");
                });

            modelBuilder.Entity("WebERP.Models.PODetailModel", b =>
                {
                    b.Property<int>("POD_PK")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("APPROVED_DATE");

                    b.Property<string>("APPROVED_UID");

                    b.Property<DateTime>("DELV_DATE");

                    b.Property<decimal>("DISC_PER");

                    b.Property<decimal>("DISC_RATE");

                    b.Property<DateTime?>("INS_DATE");

                    b.Property<string>("INS_UID");

                    b.Property<int>("ITEM_CODE");

                    b.Property<decimal>("NET_RATE");

                    b.Property<string>("POD_PK_STATUS")
                        .HasMaxLength(3);

                    b.Property<int>("POH_FK");

                    b.Property<decimal>("QTY");

                    b.Property<int>("QTY_UOM");

                    b.Property<decimal>("RATE");

                    b.Property<int>("RATE_UOM");

                    b.Property<string>("REMARKS")
                        .HasMaxLength(100);

                    b.Property<DateTime?>("UDT_DATE");

                    b.Property<string>("UDT_UID");

                    b.HasKey("POD_PK");

                    b.HasIndex("POH_FK");

                    b.ToTable("PODetail_Master");
                });

            modelBuilder.Entity("WebERP.Models.POHeaderModel", b =>
                {
                    b.Property<int>("POH_PK")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ACC_CODE");

                    b.Property<int>("COMP_CODE");

                    b.Property<DateTime?>("INS_DATE");

                    b.Property<string>("INS_UID");

                    b.Property<DateTime>("ORDER_DATE");

                    b.Property<string>("ORDER_FINYEAR");

                    b.Property<int>("ORDER_NO");

                    b.Property<string>("REMARKS")
                        .HasMaxLength(100);

                    b.Property<DateTime?>("UDT_DATE");

                    b.Property<string>("UDT_UID");

                    b.HasKey("POH_PK");

                    b.ToTable("POHeader_Master");
                });

            modelBuilder.Entity("WebERP.Models.POTermsModel", b =>
                {
                    b.Property<int>("POT_PK")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("INS_DATE");

                    b.Property<string>("INS_UID");

                    b.Property<int>("POH_FK");

                    b.Property<string>("REMARKS")
                        .HasMaxLength(100);

                    b.Property<int>("TERMS_CODE");

                    b.Property<DateTime?>("UDT_DATE");

                    b.Property<string>("UDT_UID");

                    b.HasKey("POT_PK");

                    b.HasIndex("POH_FK");

                    b.ToTable("POTerm_Master");
                });

            modelBuilder.Entity("WebERP.Models.ProcessRate_Master", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Artical_Code")
                        .IsRequired();

                    b.Property<string>("Comm_Rate")
                        .IsRequired();

                    b.Property<DateTime?>("From_DATE");

                    b.Property<DateTime?>("INS_DATE");

                    b.Property<string>("INS_UID");

                    b.Property<string>("Proc_Code")
                        .IsRequired();

                    b.Property<string>("Rate")
                        .IsRequired();

                    b.Property<DateTime?>("To_DATE");

                    b.Property<DateTime?>("UDT_DATE");

                    b.Property<string>("UDT_UID");

                    b.Property<string>("UOM_Code")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("ProcessRate_Master");
                });

            modelBuilder.Entity("WebERP.Models.Process_Master", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("INS_DATE");

                    b.Property<string>("INS_UID");

                    b.Property<string>("NAME")
                        .IsRequired();

                    b.Property<DateTime?>("UDT_DATE");

                    b.Property<string>("UDT_UID");

                    b.HasKey("ID");

                    b.ToTable("Process_Master");
                });

            modelBuilder.Entity("WebERP.Models.Size_Master", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("INS_DATE");

                    b.Property<string>("INS_UID");

                    b.Property<string>("NAME")
                        .IsRequired();

                    b.Property<DateTime?>("UDT_DATE");

                    b.Property<string>("UDT_UID");

                    b.HasKey("ID");

                    b.ToTable("Size_Master");
                });

            modelBuilder.Entity("WebERP.Models.StateModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CountryCode");

                    b.Property<int>("CountryId");

                    b.Property<DateTime?>("Ins_Date");

                    b.Property<string>("Ins_Uid");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("StateCode")
                        .IsRequired()
                        .HasMaxLength(3);

                    b.Property<DateTime?>("Upd_Date");

                    b.Property<string>("Upd_Uid");

                    b.HasKey("Id");

                    b.ToTable("States");
                });

            modelBuilder.Entity("WebERP.Models.StockDTL_Model", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Artical_CODE");

                    b.Property<int>("COMP_CODE");

                    b.Property<int>("GDW_CODE");

                    b.Property<DateTime?>("INS_DATE");

                    b.Property<string>("INS_UID");

                    b.Property<int>("Item_Code");

                    b.Property<int>("Size_Code");

                    b.Property<int>("Stk_Qty_IN");

                    b.Property<int>("Stk_Qty_OUT");

                    b.Property<string>("Tran_Table");

                    b.Property<int>("Tran_Table_PK");

                    b.Property<DateTime?>("UDT_DATE");

                    b.Property<string>("UDT_UID");

                    b.HasKey("ID");

                    b.ToTable("StockDTL_Models");
                });

            modelBuilder.Entity("WebERP.Models.Term_Master", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ACTIVE_TAG");

                    b.Property<DateTime?>("INS_DATE");

                    b.Property<string>("INS_UID");

                    b.Property<string>("NAME")
                        .IsRequired();

                    b.Property<string>("PO");

                    b.Property<string>("SAL_Order");

                    b.Property<DateTime?>("UDT_DATE");

                    b.Property<string>("UDT_UID");

                    b.HasKey("ID");

                    b.ToTable("Term_Master");
                });

            modelBuilder.Entity("WebERP.Models.UOM_MASTER", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ABV")
                        .IsRequired();

                    b.Property<DateTime?>("INS_DATE");

                    b.Property<string>("INS_UID");

                    b.Property<string>("NAME")
                        .IsRequired();

                    b.Property<DateTime?>("UDT_DATE");

                    b.Property<string>("UDT_UID");

                    b.HasKey("ID");

                    b.ToTable("UOM_MASTER");
                });

            modelBuilder.Entity("WebERP.Models.V_CITY_DTL", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CSC_NAME");

                    b.HasKey("Id");

                    b.ToTable("V_CITY_DTL");
                });

            modelBuilder.Entity("WebERP.Models.V_CuttingDetail", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ARTICAL_CODE");

                    b.Property<int>("ARTICAL_NAME");

                    b.Property<int>("AVG_PC_WEIGHT");

                    b.Property<int>("COMP_CODE");

                    b.Property<int>("CONT_EMP_CODE");

                    b.Property<int>("CONT_EMP_NAME");

                    b.Property<DateTime?>("DOC_DATE");

                    b.Property<int>("DOC_FINYEAR");

                    b.Property<int>("DOC_NO");

                    b.Property<int>("EMP_CODE");

                    b.Property<int>("EMP_NAME");

                    b.Property<DateTime?>("INS_DATE");

                    b.Property<string>("INS_UID");

                    b.Property<int>("ITEM_CODE");

                    b.Property<int>("ITEM_NAME");

                    b.Property<int>("ORDER_QTY");

                    b.Property<string>("ORDER_STATUS");

                    b.Property<int>("PROC_CODE");

                    b.Property<int>("PROC_NAME");

                    b.Property<int>("SIZE_CODE");

                    b.Property<int>("SIZE_NAME");

                    b.Property<DateTime?>("UDT_DATE");

                    b.Property<string>("UDT_UID");

                    b.Property<int>("WASTAGE_PER");

                    b.HasKey("ID");

                    b.ToTable("V_CuttingDetails");
                });

            modelBuilder.Entity("WebERP.Models.V_GateEntryDetail", b =>
                {
                    b.Property<int>("POH_PK")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ACC_CODE");

                    b.Property<string>("ACC_NAME");

                    b.Property<decimal>("BAL_QTY");

                    b.Property<int>("COMP_CODE");

                    b.Property<int>("Gate_Entry_Qty");

                    b.Property<int>("ITEM_CODE");

                    b.Property<string>("ITEM_NAME");

                    b.Property<DateTime>("ORDER_DATE");

                    b.Property<string>("ORDER_FINYEAR");

                    b.Property<int>("ORDER_NO");

                    b.Property<int>("POD_PK");

                    b.Property<decimal>("QTY");

                    b.Property<int>("QTY_CODE");

                    b.Property<string>("QTY_UOM");

                    b.Property<string>("REMARKS");

                    b.HasKey("POH_PK");

                    b.ToTable("V_GateEntryDetail");
                });

            modelBuilder.Entity("WebERP.Models.V_PODetails", b =>
                {
                    b.Property<int>("POH_PK")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ACC_CODE");

                    b.Property<DateTime?>("ORDER_DATE");

                    b.Property<decimal>("QTY");

                    b.HasKey("POH_PK");

                    b.ToTable("V_PODetails");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("WebERP.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("WebERP.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WebERP.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("WebERP.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WebERP.Models.PODetailModel", b =>
                {
                    b.HasOne("WebERP.Models.POHeaderModel", "POHeaderModel")
                        .WithMany()
                        .HasForeignKey("POH_FK")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WebERP.Models.POTermsModel", b =>
                {
                    b.HasOne("WebERP.Models.POHeaderModel", "POHeaderModel")
                        .WithMany()
                        .HasForeignKey("POH_FK")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
