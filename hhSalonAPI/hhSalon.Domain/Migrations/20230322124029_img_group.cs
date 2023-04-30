using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hhSalon.Domain.Migrations
{
    /// <inheritdoc />
    public partial class img_group : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Workers",
                keyColumn: "gender",
                keyValue: null,
                column: "gender",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "gender",
                table: "Workers",
                type: "varchar(6)",
                maxLength: 6,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "img_url",
                table: "groups_of_services",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "gender",
                table: "Workers",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(6)",
                oldMaxLength: 6)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "groups_of_services",
                keyColumn: "img_url",
                keyValue: null,
                column: "img_url",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "img_url",
                table: "groups_of_services",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
