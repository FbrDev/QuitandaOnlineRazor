using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetCoreWebApp.Migrations
{
    public partial class AlterandoCliente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "CPF",
                table: "Clientes",
                type: "REAL",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldMaxLength: 11);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CPF",
                table: "Clientes",
                type: "INTEGER",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL",
                oldMaxLength: 11);
        }
    }
}
