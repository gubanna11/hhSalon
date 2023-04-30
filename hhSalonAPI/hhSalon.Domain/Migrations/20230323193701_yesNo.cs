using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hhSalon.Domain.Migrations
{
    /// <inheritdoc />
    public partial class yesNo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_users_worker_id",
                table: "Attendances");

            migrationBuilder.AlterColumn<string>(
                name: "rendered",
                table: "Attendances",
                type: "varchar(3)",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "paid",
                table: "Attendances",
                type: "varchar(3)",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Workers_worker_id",
                table: "Attendances",
                column: "worker_id",
                principalTable: "Workers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Workers_worker_id",
                table: "Attendances");

            migrationBuilder.AlterColumn<int>(
                name: "rendered",
                table: "Attendances",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "varchar(3)",
                oldMaxLength: 3,
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "paid",
                table: "Attendances",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "varchar(3)",
                oldMaxLength: 3,
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_users_worker_id",
                table: "Attendances",
                column: "worker_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
