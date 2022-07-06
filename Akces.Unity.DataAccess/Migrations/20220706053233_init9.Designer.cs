﻿// <auto-generated />
using System;
using Akces.Unity.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Akces.Unity.DataAccess.Migrations
{
    [DbContext(typeof(UnityDbContext))]
    [Migration("20220706053233_init9")]
    partial class init9
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.26");

            modelBuilder.Entity("Akces.Unity.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AccountType")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("account_type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Accounts");

                    b.HasDiscriminator<string>("account_type").HasValue("Account");
                });

            modelBuilder.Entity("Akces.Unity.Models.ConfigurationMembers.BranchConfigurationMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ChannelBranch")
                        .HasColumnType("TEXT");

                    b.Property<string>("CountriesCodes")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Default")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ErpBranchSymbol")
                        .HasColumnType("TEXT");

                    b.Property<int?>("NexoConfigurationId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("NexoConfigurationId");

                    b.ToTable("BranchConfigurationMember");
                });

            modelBuilder.Entity("Akces.Unity.Models.ConfigurationMembers.DeliveryMethodConfigurationMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ChannelDeliveryMethod")
                        .HasColumnType("TEXT");

                    b.Property<string>("CountriesCodes")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Default")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ErpDeliveryMethodName")
                        .HasColumnType("TEXT");

                    b.Property<int?>("NexoConfigurationId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProductSymbol")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NexoConfigurationId");

                    b.ToTable("DeliveryMethodConfigurationMember");
                });

            modelBuilder.Entity("Akces.Unity.Models.ConfigurationMembers.PaymentMethodConfigurationMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ChannelDeliveryMethod")
                        .HasColumnType("TEXT");

                    b.Property<string>("ChannelPaymentMethod")
                        .HasColumnType("TEXT");

                    b.Property<string>("CountriesCodes")
                        .HasColumnType("TEXT");

                    b.Property<string>("CurrencyCode")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Default")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ErpPaymentMethod")
                        .HasColumnType("TEXT");

                    b.Property<int?>("NexoConfigurationId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("NexoConfigurationId");

                    b.ToTable("PaymentMethodConfigurationMember");
                });

            modelBuilder.Entity("Akces.Unity.Models.ConfigurationMembers.TaxRateConfigurationMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ChannelTaxRate")
                        .HasColumnType("TEXT");

                    b.Property<string>("CountriesCodes")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Default")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ErpTaxRateSymbol")
                        .HasColumnType("TEXT");

                    b.Property<int?>("NexoConfigurationId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("NexoConfigurationId");

                    b.ToTable("TaxRateConfigurationMember");
                });

            modelBuilder.Entity("Akces.Unity.Models.ConfigurationMembers.TransactionConfigurationMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CountriesCodes")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Default")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ErpTranstactionSymbol")
                        .HasColumnType("TEXT");

                    b.Property<int?>("NexoConfigurationId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("NexoConfigurationId");

                    b.ToTable("TransactionConfigurationMember");
                });

            modelBuilder.Entity("Akces.Unity.Models.ConfigurationMembers.UnitConfigurationMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ChannelUnit")
                        .HasColumnType("TEXT");

                    b.Property<string>("CountriesCodes")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Default")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ErpUnitSymbol")
                        .HasColumnType("TEXT");

                    b.Property<int?>("NexoConfigurationId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("NexoConfigurationId");

                    b.ToTable("UnitConfigurationMember");
                });

            modelBuilder.Entity("Akces.Unity.Models.ConfigurationMembers.WarehouseConfigurationMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ChannelWarehouse")
                        .HasColumnType("TEXT");

                    b.Property<string>("CountriesCodes")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Default")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ErpDocumentStatus")
                        .HasColumnType("TEXT");

                    b.Property<string>("ErpWarehouseSymbol")
                        .HasColumnType("TEXT");

                    b.Property<string>("ExternalOrderTemplate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("NexoConfigurationId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("NexoConfigurationId");

                    b.ToTable("WarehouseConfigurationMember");
                });

            modelBuilder.Entity("Akces.Unity.Models.Harmonogram", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<bool>("WorkerEnabled")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Harmonograms");
                });

            modelBuilder.Entity("Akces.Unity.Models.HarmonogramPosition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AccountId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("HarmonogramId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("HarmonogramOperation")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("LastLaunchTime")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Repeat")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RepeatAfterMinutes")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("HarmonogramId");

                    b.ToTable("HarmonogramPosition");
                });

            modelBuilder.Entity("Akces.Unity.Models.NexoConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AccountId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.ToTable("NexoConfiguration");
                });

            modelBuilder.Entity("Akces.Unity.Models.OperationReport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("ErrorsCount")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("HarmonogramPositionId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("InfosCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("OperationType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PositionsCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("WarningsCount")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("OperationReports");
                });

            modelBuilder.Entity("Akces.Unity.Models.OperationReportPosition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("ObjectName")
                        .HasColumnType("TEXT");

                    b.Property<int>("OperationReportId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("OperationReportId");

                    b.ToTable("OperationReportPosition");
                });

            modelBuilder.Entity("Akces.Unity.Models.SaleChannels.Allegro.AllegroConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AccessToken")
                        .HasColumnType("TEXT");

                    b.Property<int?>("AccountId")
                        .IsRequired()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BaseAddress")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClientId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClientSecret")
                        .HasColumnType("TEXT");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Sandbox")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SandboxBaseAddress")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.ToTable("AllegroConfiguration");
                });

            modelBuilder.Entity("Akces.Unity.Models.SaleChannels.Baselinker.BaselinkerConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AccountId")
                        .IsRequired()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BaseAddress")
                        .HasColumnType("TEXT");

                    b.Property<bool>("GetUnconfirmedOrders")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ImportOrdersFromOffsetHours")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Token")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.ToTable("BaselinkerConfiguration");
                });

            modelBuilder.Entity("Akces.Unity.Models.SaleChannels.Olx.OlxConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AccessToken")
                        .HasColumnType("TEXT");

                    b.Property<int?>("AccountId")
                        .IsRequired()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BaseAddress")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClientId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClientSecret")
                        .HasColumnType("TEXT");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.ToTable("OlxConfiguration");
                });

            modelBuilder.Entity("Akces.Unity.Models.SaleChannels.Shoper.ShoperConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AccountId")
                        .IsRequired()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.ToTable("ShoperConfiguration");
                });

            modelBuilder.Entity("Akces.Unity.Models.SaleChannels.Shopgold.ShopgoldConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AccountId")
                        .IsRequired()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConnectionString")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.ToTable("ShopgoldConfiguration");
                });

            modelBuilder.Entity("Akces.Unity.Models.UnityUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Admin")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("CanOpenHarmonograms")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("CanOpenTasks")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT");

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

            modelBuilder.Entity("Akces.Unity.Models.SaleChannels.Olx.OlxAccount", b =>
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

            modelBuilder.Entity("Akces.Unity.Models.OperationReportPosition", b =>
                {
                    b.HasOne("Akces.Unity.Models.OperationReport", null)
                        .WithMany("Positions")
                        .HasForeignKey("OperationReportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Akces.Unity.Models.SaleChannels.Allegro.AllegroConfiguration", b =>
                {
                    b.HasOne("Akces.Unity.Models.SaleChannels.AllegroAccount", null)
                        .WithOne("AllegroConfiguration")
                        .HasForeignKey("Akces.Unity.Models.SaleChannels.Allegro.AllegroConfiguration", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Akces.Unity.Models.SaleChannels.Baselinker.BaselinkerConfiguration", b =>
                {
                    b.HasOne("Akces.Unity.Models.SaleChannels.BaselinkerAccount", null)
                        .WithOne("BaselinkerConfiguration")
                        .HasForeignKey("Akces.Unity.Models.SaleChannels.Baselinker.BaselinkerConfiguration", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Akces.Unity.Models.SaleChannels.Olx.OlxConfiguration", b =>
                {
                    b.HasOne("Akces.Unity.Models.SaleChannels.Olx.OlxAccount", null)
                        .WithOne("OlxConfiguration")
                        .HasForeignKey("Akces.Unity.Models.SaleChannels.Olx.OlxConfiguration", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Akces.Unity.Models.SaleChannels.Shoper.ShoperConfiguration", b =>
                {
                    b.HasOne("Akces.Unity.Models.SaleChannels.ShoperAccount", null)
                        .WithOne("ShoperConfiguration")
                        .HasForeignKey("Akces.Unity.Models.SaleChannels.Shoper.ShoperConfiguration", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Akces.Unity.Models.SaleChannels.Shopgold.ShopgoldConfiguration", b =>
                {
                    b.HasOne("Akces.Unity.Models.SaleChannels.ShopgoldAccount", null)
                        .WithOne("ShopgoldConfiguration")
                        .HasForeignKey("Akces.Unity.Models.SaleChannels.Shopgold.ShopgoldConfiguration", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
