using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantGate.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorizationModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "UserRoles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "FunctionalPrivileges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsSystemPrivilege = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FunctionalPrivileges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FunctionalPrivileges_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrivilegeSets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsSystemPrivilegeSet = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivilegeSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrivilegeSets_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TechnicalPrivileges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ApiScope = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsSystemPrivilege = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalPrivileges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnicalPrivileges_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrivilegeSetPrivileges",
                columns: table => new
                {
                    PrivilegeSetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FunctionalPrivilegeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivilegeSetPrivileges", x => new { x.PrivilegeSetId, x.FunctionalPrivilegeId });
                    table.ForeignKey(
                        name: "FK_PrivilegeSetPrivileges_FunctionalPrivileges_FunctionalPrivilegeId",
                        column: x => x.FunctionalPrivilegeId,
                        principalTable: "FunctionalPrivileges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrivilegeSetPrivileges_PrivilegeSets_PrivilegeSetId",
                        column: x => x.PrivilegeSetId,
                        principalTable: "PrivilegeSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolePrivilegeSets",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrivilegeSetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePrivilegeSets", x => new { x.RoleId, x.PrivilegeSetId });
                    table.ForeignKey(
                        name: "FK_RolePrivilegeSets_PrivilegeSets_PrivilegeSetId",
                        column: x => x.PrivilegeSetId,
                        principalTable: "PrivilegeSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePrivilegeSets_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FunctionalTechnicalPrivileges",
                columns: table => new
                {
                    FunctionalPrivilegeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TechnicalPrivilegeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FunctionalTechnicalPrivileges", x => new { x.FunctionalPrivilegeId, x.TechnicalPrivilegeId });
                    table.ForeignKey(
                        name: "FK_FunctionalTechnicalPrivileges_FunctionalPrivileges_FunctionalPrivilegeId",
                        column: x => x.FunctionalPrivilegeId,
                        principalTable: "FunctionalPrivileges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FunctionalTechnicalPrivileges_TechnicalPrivileges_TechnicalPrivilegeId",
                        column: x => x.TechnicalPrivilegeId,
                        principalTable: "TechnicalPrivileges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FunctionalPrivileges_TenantId_Code",
                table: "FunctionalPrivileges",
                columns: new[] { "TenantId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FunctionalTechnicalPrivileges_TechnicalPrivilegeId",
                table: "FunctionalTechnicalPrivileges",
                column: "TechnicalPrivilegeId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivilegeSetPrivileges_FunctionalPrivilegeId",
                table: "PrivilegeSetPrivileges",
                column: "FunctionalPrivilegeId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivilegeSets_TenantId_Code",
                table: "PrivilegeSets",
                columns: new[] { "TenantId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePrivilegeSets_PrivilegeSetId",
                table: "RolePrivilegeSets",
                column: "PrivilegeSetId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalPrivileges_TenantId_Code",
                table: "TechnicalPrivileges",
                columns: new[] { "TenantId", "Code" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FunctionalTechnicalPrivileges");

            migrationBuilder.DropTable(
                name: "PrivilegeSetPrivileges");

            migrationBuilder.DropTable(
                name: "RolePrivilegeSets");

            migrationBuilder.DropTable(
                name: "TechnicalPrivileges");

            migrationBuilder.DropTable(
                name: "FunctionalPrivileges");

            migrationBuilder.DropTable(
                name: "PrivilegeSets");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "UserRoles");
        }
    }
}
