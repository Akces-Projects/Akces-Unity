using System;
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
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    WorkerEnabled = table.Column<bool>(nullable: false),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HarmonogramPositionId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    OperationType = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    PositionsCount = table.Column<int>(nullable: false),
                    InfosCount = table.Column<int>(nullable: false),
                    WarningsCount = table.Column<int>(nullable: false),
                    ErrorsCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnityUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    IsWorker = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnityUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkerStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AllegroConfiguration",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<string>(nullable: true),
                    ClientSecret = table.Column<string>(nullable: true),
                    Sandbox = table.Column<bool>(nullable: false),
                    BaseAddress = table.Column<string>(nullable: true),
                    SandboxBaseAddress = table.Column<string>(nullable: true),
                    RefreshToken = table.Column<string>(nullable: true),
                    AccessToken = table.Column<string>(nullable: true),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BaseAddress = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true),
                    GetUnconfirmedOrders = table.Column<bool>(nullable: false),
                    ImportOrdersFromOffsetHours = table.Column<int>(nullable: false),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                name: "OlxConfiguration",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<string>(nullable: true),
                    ClientSecret = table.Column<string>(nullable: true),
                    BaseAddress = table.Column<string>(nullable: true),
                    RefreshToken = table.Column<string>(nullable: true),
                    AccessToken = table.Column<string>(nullable: true),
                    AccountId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OlxConfiguration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OlxConfiguration_Accounts_AccountId",
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
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConnectionString = table.Column<string>(nullable: true),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                name: "TaskReportPosition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObjectName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    TaskReportId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskReportPosition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskReportPosition_OperationReports_TaskReportId",
                        column: x => x.TaskReportId,
                        principalTable: "OperationReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Authorisation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Module = table.Column<string>(nullable: true),
                    AuthorisationType = table.Column<int>(nullable: false),
                    UnityUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authorisation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Authorisation_UnityUsers_UnityUserId",
                        column: x => x.UnityUserId,
                        principalTable: "UnityUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BranchConfigurationMember",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountriesCodes = table.Column<string>(nullable: true),
                    ChannelBranch = table.Column<string>(nullable: true),
                    NexoBranchSymbol = table.Column<string>(nullable: true),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountriesCodes = table.Column<string>(nullable: true),
                    ProductSymbol = table.Column<string>(nullable: true),
                    ChannelDeliveryMethod = table.Column<string>(nullable: true),
                    NexoDeliveryMethodName = table.Column<string>(nullable: true),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountriesCodes = table.Column<string>(nullable: true),
                    ChannelPaymentMethod = table.Column<string>(nullable: true),
                    ChannelDeliveryMethod = table.Column<string>(nullable: true),
                    NexoPaymentMethod = table.Column<string>(nullable: true),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountriesCodes = table.Column<string>(nullable: true),
                    ChannelTaxRate = table.Column<string>(nullable: true),
                    NexoTaxRateSymbol = table.Column<string>(nullable: true),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountriesCodes = table.Column<string>(nullable: true),
                    NexoTranstactionSymbol = table.Column<string>(nullable: true),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountriesCodes = table.Column<string>(nullable: true),
                    ChannelUnit = table.Column<string>(nullable: true),
                    NexoUnitSymbol = table.Column<string>(nullable: true),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalNumberTemplate = table.Column<string>(nullable: true),
                    CountriesCodes = table.Column<string>(nullable: true),
                    ChannelWarehouse = table.Column<string>(nullable: true),
                    NexoWarehouseSymbol = table.Column<string>(nullable: true),
                    NexoDocumentStatus = table.Column<string>(nullable: true),
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
                name: "IX_Authorisation_UnityUserId",
                table: "Authorisation",
                column: "UnityUserId");

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
                name: "IX_OlxConfiguration_AccountId",
                table: "OlxConfiguration",
                column: "AccountId",
                unique: true);

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
                name: "IX_TaskReportPosition_TaskReportId",
                table: "TaskReportPosition",
                column: "TaskReportId");

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
                name: "Authorisation");

            migrationBuilder.DropTable(
                name: "BaselinkerConfiguration");

            migrationBuilder.DropTable(
                name: "BranchConfigurationMember");

            migrationBuilder.DropTable(
                name: "DeliveryMethodConfigurationMember");

            migrationBuilder.DropTable(
                name: "HarmonogramPosition");

            migrationBuilder.DropTable(
                name: "OlxConfiguration");

            migrationBuilder.DropTable(
                name: "PaymentMethodConfigurationMember");

            migrationBuilder.DropTable(
                name: "ShoperConfiguration");

            migrationBuilder.DropTable(
                name: "ShopgoldConfiguration");

            migrationBuilder.DropTable(
                name: "TaskReportPosition");

            migrationBuilder.DropTable(
                name: "TaxRateConfigurationMember");

            migrationBuilder.DropTable(
                name: "TransactionConfigurationMember");

            migrationBuilder.DropTable(
                name: "UnitConfigurationMember");

            migrationBuilder.DropTable(
                name: "WarehouseConfigurationMember");

            migrationBuilder.DropTable(
                name: "WorkerStatuses");

            migrationBuilder.DropTable(
                name: "UnityUsers");

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
