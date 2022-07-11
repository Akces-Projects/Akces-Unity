﻿// <auto-generated />
using System;
using Akces.Unity.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Akces.Unity.DataAccess.Migrations
{
    [DbContext(typeof(UnityDbContext))]
    [Migration("20220710135448_init2")]
    partial class init2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.26")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Akces.Unity.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountType")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("account_type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Accounts");

                    b.HasDiscriminator<string>("account_type").HasValue("Account");
                });

            modelBuilder.Entity("Akces.Unity.Models.Authorisation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AuthorisationType")
                        .HasColumnType("int");

                    b.Property<string>("Module")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UnityUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UnityUserId");

                    b.ToTable("Authorisation");
                });

            modelBuilder.Entity("Akces.Unity.Models.ConfigurationMembers.BranchConfigurationMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ChannelBranch")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CountriesCodes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Default")
                        .HasColumnType("bit");

                    b.Property<string>("NexoBranchSymbol")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("NexoConfigurationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NexoConfigurationId");

                    b.ToTable("BranchConfigurationMember");
                });

            modelBuilder.Entity("Akces.Unity.Models.ConfigurationMembers.DeliveryMethodConfigurationMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ChannelDeliveryMethod")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CountriesCodes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Default")
                        .HasColumnType("bit");

                    b.Property<int?>("NexoConfigurationId")
                        .HasColumnType("int");

                    b.Property<string>("NexoDeliveryMethodName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductSymbol")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("NexoConfigurationId");

                    b.ToTable("DeliveryMethodConfigurationMember");
                });

            modelBuilder.Entity("Akces.Unity.Models.ConfigurationMembers.PaymentMethodConfigurationMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ChannelDeliveryMethod")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ChannelPaymentMethod")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CountriesCodes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CurrencyCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Default")
                        .HasColumnType("bit");

                    b.Property<int?>("NexoConfigurationId")
                        .HasColumnType("int");

                    b.Property<string>("NexoPaymentMethod")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("NexoConfigurationId");

                    b.ToTable("PaymentMethodConfigurationMember");
                });

            modelBuilder.Entity("Akces.Unity.Models.ConfigurationMembers.TaxRateConfigurationMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ChannelTaxRate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CountriesCodes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Default")
                        .HasColumnType("bit");

                    b.Property<int?>("NexoConfigurationId")
                        .HasColumnType("int");

                    b.Property<string>("NexoTaxRateSymbol")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("NexoConfigurationId");

                    b.ToTable("TaxRateConfigurationMember");
                });

            modelBuilder.Entity("Akces.Unity.Models.ConfigurationMembers.TransactionConfigurationMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CountriesCodes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Default")
                        .HasColumnType("bit");

                    b.Property<int?>("NexoConfigurationId")
                        .HasColumnType("int");

                    b.Property<string>("NexoTranstactionSymbol")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("NexoConfigurationId");

                    b.ToTable("TransactionConfigurationMember");
                });

            modelBuilder.Entity("Akces.Unity.Models.ConfigurationMembers.UnitConfigurationMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ChannelUnit")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CountriesCodes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Default")
                        .HasColumnType("bit");

                    b.Property<int?>("NexoConfigurationId")
                        .HasColumnType("int");

                    b.Property<string>("NexoUnitSymbol")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("NexoConfigurationId");

                    b.ToTable("UnitConfigurationMember");
                });

            modelBuilder.Entity("Akces.Unity.Models.ConfigurationMembers.WarehouseConfigurationMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ChannelWarehouse")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CountriesCodes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Default")
                        .HasColumnType("bit");

                    b.Property<string>("ExternalNumberTemplate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("NexoConfigurationId")
                        .HasColumnType("int");

                    b.Property<string>("NexoDocumentStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NexoWarehouseSymbol")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("NexoConfigurationId");

                    b.ToTable("WarehouseConfigurationMember");
                });

            modelBuilder.Entity("Akces.Unity.Models.Harmonogram", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("WorkerEnabled")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Harmonograms");
                });

            modelBuilder.Entity("Akces.Unity.Models.HarmonogramPosition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AccountId")
                        .HasColumnType("int");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("HarmonogramId")
                        .HasColumnType("int");

                    b.Property<int>("HarmonogramOperation")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastLaunchTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Repeat")
                        .HasColumnType("bit");

                    b.Property<int>("RepeatAfterMinutes")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("HarmonogramId");

                    b.ToTable("HarmonogramPosition");
                });

            modelBuilder.Entity("Akces.Unity.Models.NexoConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.ToTable("NexoConfiguration");
                });

            modelBuilder.Entity("Akces.Unity.Models.SaleChannels.AllegroConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccessToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("AccountId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("BaseAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClientId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClientSecret")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Sandbox")
                        .HasColumnType("bit");

                    b.Property<string>("SandboxBaseAddress")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.ToTable("AllegroConfiguration");
                });

            modelBuilder.Entity("Akces.Unity.Models.SaleChannels.BaselinkerConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AccountId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("BaseAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("GetUnconfirmedOrders")
                        .HasColumnType("bit");

                    b.Property<int>("ImportOrdersFromOffsetHours")
                        .HasColumnType("int");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.ToTable("BaselinkerConfiguration");
                });

            modelBuilder.Entity("Akces.Unity.Models.SaleChannels.OlxConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccessToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("AccountId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("BaseAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClientId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClientSecret")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.ToTable("OlxConfiguration");
                });

            modelBuilder.Entity("Akces.Unity.Models.SaleChannels.ShoperConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AccountId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.ToTable("ShoperConfiguration");
                });

            modelBuilder.Entity("Akces.Unity.Models.SaleChannels.ShopgoldConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AccountId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("ConnectionString")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.ToTable("ShopgoldConfiguration");
                });

            modelBuilder.Entity("Akces.Unity.Models.TaskReport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ErrorsCount")
                        .HasColumnType("int");

                    b.Property<int?>("HarmonogramPositionId")
                        .HasColumnType("int");

                    b.Property<int>("InfosCount")
                        .HasColumnType("int");

                    b.Property<int>("OperationType")
                        .HasColumnType("int");

                    b.Property<int>("PositionsCount")
                        .HasColumnType("int");

                    b.Property<int>("WarningsCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("OperationReports");
                });

            modelBuilder.Entity("Akces.Unity.Models.TaskReportPosition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ObjectName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TaskReportId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TaskReportId");

                    b.ToTable("TaskReportPosition");
                });

            modelBuilder.Entity("Akces.Unity.Models.UnityUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsWorker")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UnityUsers");
                });

            modelBuilder.Entity("Akces.Unity.Models.SaleChannels.AllegroAccount", b =>
                {
                    b.HasBaseType("Akces.Unity.Models.Account");

                    b.HasDiscriminator().HasValue("AllegroAccount");
                });

            modelBuilder.Entity("Akces.Unity.Models.SaleChannels.BaselinkerAccount", b =>
                {
                    b.HasBaseType("Akces.Unity.Models.Account");

                    b.HasDiscriminator().HasValue("BaselinkerAccount");
                });

            modelBuilder.Entity("Akces.Unity.Models.SaleChannels.OlxAccount", b =>
                {
                    b.HasBaseType("Akces.Unity.Models.Account");

                    b.HasDiscriminator().HasValue("OlxAccount");
                });

            modelBuilder.Entity("Akces.Unity.Models.SaleChannels.ShoperAccount", b =>
                {
                    b.HasBaseType("Akces.Unity.Models.Account");

                    b.HasDiscriminator().HasValue("ShoperAccount");
                });

            modelBuilder.Entity("Akces.Unity.Models.SaleChannels.ShopgoldAccount", b =>
                {
                    b.HasBaseType("Akces.Unity.Models.Account");

                    b.HasDiscriminator().HasValue("ShopgoldAccount");
                });

            modelBuilder.Entity("Akces.Unity.Models.Authorisation", b =>
                {
                    b.HasOne("Akces.Unity.Models.UnityUser", null)
                        .WithMany("Authorisations")
                        .HasForeignKey("UnityUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Akces.Unity.Models.ConfigurationMembers.BranchConfigurationMember", b =>
                {
                    b.HasOne("Akces.Unity.Models.NexoConfiguration", null)
                        .WithMany("Branches")
                        .HasForeignKey("NexoConfigurationId");
                });

            modelBuilder.Entity("Akces.Unity.Models.ConfigurationMembers.DeliveryMethodConfigurationMember", b =>
                {
                    b.HasOne("Akces.Unity.Models.NexoConfiguration", null)
                        .WithMany("DeliveryMethods")
                        .HasForeignKey("NexoConfigurationId");
                });

            modelBuilder.Entity("Akces.Unity.Models.ConfigurationMembers.PaymentMethodConfigurationMember", b =>
                {
                    b.HasOne("Akces.Unity.Models.NexoConfiguration", null)
                        .WithMany("PaymentMethods")
                        .HasForeignKey("NexoConfigurationId");
                });

            modelBuilder.Entity("Akces.Unity.Models.ConfigurationMembers.TaxRateConfigurationMember", b =>
                {
                    b.HasOne("Akces.Unity.Models.NexoConfiguration", null)
                        .WithMany("TaxRates")
                        .HasForeignKey("NexoConfigurationId");
                });

            modelBuilder.Entity("Akces.Unity.Models.ConfigurationMembers.TransactionConfigurationMember", b =>
                {
                    b.HasOne("Akces.Unity.Models.NexoConfiguration", null)
                        .WithMany("Transactions")
                        .HasForeignKey("NexoConfigurationId");
                });

            modelBuilder.Entity("Akces.Unity.Models.ConfigurationMembers.UnitConfigurationMember", b =>
                {
                    b.HasOne("Akces.Unity.Models.NexoConfiguration", null)
                        .WithMany("Units")
                        .HasForeignKey("NexoConfigurationId");
                });

            modelBuilder.Entity("Akces.Unity.Models.ConfigurationMembers.WarehouseConfigurationMember", b =>
                {
                    b.HasOne("Akces.Unity.Models.NexoConfiguration", null)
                        .WithMany("Warehouses")
                        .HasForeignKey("NexoConfigurationId");
                });

            modelBuilder.Entity("Akces.Unity.Models.HarmonogramPosition", b =>
                {
                    b.HasOne("Akces.Unity.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Akces.Unity.Models.Harmonogram", null)
                        .WithMany("Positions")
                        .HasForeignKey("HarmonogramId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Akces.Unity.Models.NexoConfiguration", b =>
                {
                    b.HasOne("Akces.Unity.Models.Account", null)
                        .WithOne("NexoConfiguration")
                        .HasForeignKey("Akces.Unity.Models.NexoConfiguration", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Akces.Unity.Models.SaleChannels.AllegroConfiguration", b =>
                {
                    b.HasOne("Akces.Unity.Models.SaleChannels.AllegroAccount", null)
                        .WithOne("AllegroConfiguration")
                        .HasForeignKey("Akces.Unity.Models.SaleChannels.AllegroConfiguration", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Akces.Unity.Models.SaleChannels.BaselinkerConfiguration", b =>
                {
                    b.HasOne("Akces.Unity.Models.SaleChannels.BaselinkerAccount", null)
                        .WithOne("BaselinkerConfiguration")
                        .HasForeignKey("Akces.Unity.Models.SaleChannels.BaselinkerConfiguration", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Akces.Unity.Models.SaleChannels.OlxConfiguration", b =>
                {
                    b.HasOne("Akces.Unity.Models.SaleChannels.OlxAccount", null)
                        .WithOne("OlxConfiguration")
                        .HasForeignKey("Akces.Unity.Models.SaleChannels.OlxConfiguration", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Akces.Unity.Models.SaleChannels.ShoperConfiguration", b =>
                {
                    b.HasOne("Akces.Unity.Models.SaleChannels.ShoperAccount", null)
                        .WithOne("ShoperConfiguration")
                        .HasForeignKey("Akces.Unity.Models.SaleChannels.ShoperConfiguration", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Akces.Unity.Models.SaleChannels.ShopgoldConfiguration", b =>
                {
                    b.HasOne("Akces.Unity.Models.SaleChannels.ShopgoldAccount", null)
                        .WithOne("ShopgoldConfiguration")
                        .HasForeignKey("Akces.Unity.Models.SaleChannels.ShopgoldConfiguration", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Akces.Unity.Models.TaskReportPosition", b =>
                {
                    b.HasOne("Akces.Unity.Models.TaskReport", null)
                        .WithMany("Positions")
                        .HasForeignKey("TaskReportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
