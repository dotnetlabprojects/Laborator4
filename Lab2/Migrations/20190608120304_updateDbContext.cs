using Microsoft.EntityFrameworkCore.Migrations;

namespace Lab2.Migrations
{
    public partial class updateDbContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserUserRol_Users_UserId",
                table: "UserUserRol");

            migrationBuilder.DropForeignKey(
                name: "FK_UserUserRol_UserRole_UserRoleId",
                table: "UserUserRol");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserUserRol",
                table: "UserUserRol");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRole",
                table: "UserRole");

            migrationBuilder.RenameTable(
                name: "UserUserRol",
                newName: "UserUserRols");

            migrationBuilder.RenameTable(
                name: "UserRole",
                newName: "UserRoles");

            migrationBuilder.RenameIndex(
                name: "IX_UserUserRol_UserRoleId",
                table: "UserUserRols",
                newName: "IX_UserUserRols_UserRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_UserUserRol_UserId",
                table: "UserUserRols",
                newName: "IX_UserUserRols_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "UserRoles",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserUserRols",
                table: "UserUserRols",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserUserRols_Users_UserId",
                table: "UserUserRols",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserUserRols_UserRoles_UserRoleId",
                table: "UserUserRols",
                column: "UserRoleId",
                principalTable: "UserRoles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserUserRols_Users_UserId",
                table: "UserUserRols");

            migrationBuilder.DropForeignKey(
                name: "FK_UserUserRols_UserRoles_UserRoleId",
                table: "UserUserRols");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserUserRols",
                table: "UserUserRols");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.RenameTable(
                name: "UserUserRols",
                newName: "UserUserRol");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "UserRole");

            migrationBuilder.RenameIndex(
                name: "IX_UserUserRols_UserRoleId",
                table: "UserUserRol",
                newName: "IX_UserUserRol_UserRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_UserUserRols_UserId",
                table: "UserUserRol",
                newName: "IX_UserUserRol_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "Description",
                table: "UserRole",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserUserRol",
                table: "UserUserRol",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRole",
                table: "UserRole",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserUserRol_Users_UserId",
                table: "UserUserRol",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserUserRol_UserRole_UserRoleId",
                table: "UserUserRol",
                column: "UserRoleId",
                principalTable: "UserRole",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
