using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetCoreWebApp.Migrations
{
    public partial class AlterandoCpfCliente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CPF",
                table: "Clientes",
                type: "TEXT",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL",
                oldMaxLength: 11);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "CPF",
                table: "Clientes",
                type: "REAL",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 11);
        }
    }
}
