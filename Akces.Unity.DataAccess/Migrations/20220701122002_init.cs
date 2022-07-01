﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Akces.Unity.DataAccess.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    AccountType = table.Column<int>(nullable: false),
                    account_type = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Harmonograms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Harmonograms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OperationReports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HarmonogramPositionId = table.Column<int>(nullable: false),
                    ObjectName = table.Column<string>(nullable: true),
                    OperationType = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    InfosCount = table.Column<int>(nullable: false),
                    WarningsCount = table.Column<int>(nullable: false),
                    ErrorsCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AllegroConfiguration",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllegroConfiguration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AllegroConfiguration_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BaselinkerConfiguration",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BaseAddress = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true),
                    GetUnconfirmedOrders = table.Column<bool>(nullable: false),
                    AccountId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaselinkerConfiguration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaselinkerConfiguration_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NexoConfiguration",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NexoConfiguration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NexoConfiguration_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoperConfiguration",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoperConfiguration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoperConfiguration_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShopgoldConfiguration",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopgoldConfiguration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopgoldConfiguration_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HarmonogramPosition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Active = table.Column<bool>(nullable: false),
                    Repeat = table.Column<bool>(nullable: false),
                    RepeatAfterMinutes = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: false),
                    LastLaunchTime = table.Column<DateTime>(nullable: true),
                    HarmonogramOperation = table.Column<int>(nullable: false),
                    AccountId = table.Column<int>(nullable: true),
                    HarmonogramId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HarmonogramPosition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HarmonogramPosition_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HarmonogramPosition_Harmonograms_HarmonogramId",
                        column: x => x.HarmonogramId,
                        principalTable: "Harmonograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationReportPosition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    OperationReportId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationReportPosition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationReportPosition_OperationReports_OperationReportId",
                        column: x => x.OperationReportId,
                        principalTable: "OperationReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BranchConfigurationMember",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CountriesCodes = table.Column<string>(nullable: true),
                    ChannelBranch = table.Column<string>(nullable: true),
                    ErpBranchSymbol = table.Column<string>(nullable: true),
                    Default = table.Column<bool>(nullable: false),
                    NexoConfigurationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchConfigurationMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BranchConfigurationMember_NexoConfiguration_NexoConfigurationId",
                        column: x => x.NexoConfigurationId,
                        principalTable: "NexoConfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryMethodConfigurationMember",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CountriesCodes = table.Column<string>(nullable: true),
                    ProductSymbol = table.Column<string>(nullable: true),
                    ChannelDeliveryMethod = table.Column<string>(nullable: true),
                    ErpDeliveryMethodName = table.Column<string>(nullable: true),
                    Default = table.Column<bool>(nullable: false),
                    NexoConfigurationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryMethodConfigurationMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryMethodConfigurationMember_NexoConfiguration_NexoConfigurationId",
                        column: x => x.NexoConfigurationId,
                        principalTable: "NexoConfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethodConfigurationMember",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CountriesCodes = table.Column<string>(nullable: true),
                    ChannelPaymentMethod = table.Column<string>(nullable: true),
                    ChannelDeliveryMethod = table.Column<string>(nullable: true),
                    ErpPaymentMethod = table.Column<string>(nullable: true),
                    CurrencyCode = table.Column<string>(nullable: true),
                    Default = table.Column<bool>(nullable: false),
                    NexoConfigurationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethodConfigurationMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentMethodConfigurationMember_NexoConfiguration_NexoConfigurationId",
                        column: x => x.NexoConfigurationId,
                        principalTable: "NexoConfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaxRateConfigurationMember",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CountriesCodes = table.Column<string>(nullable: true),
                    ChannelTaxRate = table.Column<string>(nullable: true),
                    ErpTaxRateSymbol = table.Column<string>(nullable: true),
                    Default = table.Column<bool>(nullable: false),
                    NexoConfigurationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxRateConfigurationMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxRateConfigurationMember_NexoConfiguration_NexoConfigurationId",
                        column: x => x.NexoConfigurationId,
                        principalTable: "NexoConfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransactionConfigurationMember",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CountriesCodes = table.Column<string>(nullable: true),
                    ErpTranstactionSymbol = table.Column<string>(nullable: true),
                    Default = table.Column<bool>(nullable: false),
                    NexoConfigurationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionConfigurationMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionConfigurationMember_NexoConfiguration_NexoConfigurationId",
                        column: x => x.NexoConfigurationId,
                        principalTable: "NexoConfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UnitConfigurationMember",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CountriesCodes = table.Column<string>(nullable: true),
                    ChannelUnit = table.Column<string>(nullable: true),
                    ErpUnitSymbol = table.Column<string>(nullable: true),
                    Default = table.Column<bool>(nullable: false),
                    NexoConfigurationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitConfigurationMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitConfigurationMember_NexoConfiguration_NexoConfigurationId",
                        column: x => x.NexoConfigurationId,
                        principalTable: "NexoConfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseConfigurationMember",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ExternalOrderTemplate = table.Column<string>(nullable: true),
                    CountriesCodes = table.Column<string>(nullable: true),
                    ChannelWarehouse = table.Column<string>(nullable: true),
                    ErpWarehouseSymbol = table.Column<string>(nullable: true),
                    ErpDocumentStatus = table.Column<string>(nullable: true),
                    Default = table.Column<bool>(nullable: false),
                    NexoConfigurationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseConfigurationMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseConfigurationMember_NexoConfiguration_NexoConfigurationId",
                        column: x => x.NexoConfigurationId,
                        principalTable: "NexoConfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AllegroConfiguration_AccountId",
                table: "AllegroConfiguration",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BaselinkerConfiguration_AccountId",
                table: "BaselinkerConfiguration",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BranchConfigurationMember_NexoConfigurationId",
                table: "BranchConfigurationMember",
                column: "NexoConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryMethodConfigurationMember_NexoConfigurationId",
                table: "DeliveryMethodConfigurationMember",
                column: "NexoConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_HarmonogramPosition_AccountId",
                table: "HarmonogramPosition",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_HarmonogramPosition_HarmonogramId",
                table: "HarmonogramPosition",
                column: "HarmonogramId");

            migrationBuilder.CreateIndex(
                name: "IX_NexoConfiguration_AccountId",
                table: "NexoConfiguration",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OperationReportPosition_OperationReportId",
                table: "OperationReportPosition",
                column: "OperationReportId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethodConfigurationMember_NexoConfigurationId",
                table: "PaymentMethodConfigurationMember",
                column: "NexoConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoperConfiguration_AccountId",
                table: "ShoperConfiguration",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopgoldConfiguration_AccountId",
                table: "ShopgoldConfiguration",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaxRateConfigurationMember_NexoConfigurationId",
                table: "TaxRateConfigurationMember",
                column: "NexoConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionConfigurationMember_NexoConfigurationId",
                table: "TransactionConfigurationMember",
                column: "NexoConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitConfigurationMember_NexoConfigurationId",
                table: "UnitConfigurationMember",
                column: "NexoConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseConfigurationMember_NexoConfigurationId",
                table: "WarehouseConfigurationMember",
                column: "NexoConfigurationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllegroConfiguration");

            migrationBuilder.DropTable(
                name: "BaselinkerConfiguration");

            migrationBuilder.DropTable(
                name: "BranchConfigurationMember");

            migrationBuilder.DropTable(
                name: "DeliveryMethodConfigurationMember");

            migrationBuilder.DropTable(
                name: "HarmonogramPosition");

            migrationBuilder.DropTable(
                name: "OperationReportPosition");

            migrationBuilder.DropTable(
                name: "PaymentMethodConfigurationMember");

            migrationBuilder.DropTable(
                name: "ShoperConfiguration");

            migrationBuilder.DropTable(
                name: "ShopgoldConfiguration");

            migrationBuilder.DropTable(
                name: "TaxRateConfigurationMember");

            migrationBuilder.DropTable(
                name: "TransactionConfigurationMember");

            migrationBuilder.DropTable(
                name: "UnitConfigurationMember");

            migrationBuilder.DropTable(
                name: "WarehouseConfigurationMember");

            migrationBuilder.DropTable(
                name: "Harmonograms");

            migrationBuilder.DropTable(
                name: "OperationReports");

            migrationBuilder.DropTable(
                name: "NexoConfiguration");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}