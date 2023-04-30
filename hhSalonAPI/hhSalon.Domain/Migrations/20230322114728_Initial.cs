using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hhSalon.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "groups_of_services",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    img_url = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_groups_of_services", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    price = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    first_name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    last_name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    token = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    role = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Services_Groups",
                columns: table => new
                {
                    service_id = table.Column<int>(type: "int", nullable: false),
                    group_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services_Groups", x => new { x.service_id, x.group_id });
                    table.ForeignKey(
                        name: "FK_Services_Groups_Services_service_id",
                        column: x => x.service_id,
                        principalTable: "Services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Services_Groups_groups_of_services_group_id",
                        column: x => x.group_id,
                        principalTable: "groups_of_services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    client_id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    group_id = table.Column<int>(type: "int", nullable: true),
                    service_id = table.Column<int>(type: "int", nullable: true),
                    worker_id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date = table.Column<DateTime>(type: "date", nullable: false),
                    time = table.Column<TimeSpan>(type: "time", nullable: true),
                    price = table.Column<double>(type: "double", nullable: false),
                    rendered = table.Column<int>(type: "int", nullable: false),
                    paid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.id);
                    table.ForeignKey(
                        name: "FK_Attendances_Services_service_id",
                        column: x => x.service_id,
                        principalTable: "Services",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Attendances_groups_of_services_group_id",
                        column: x => x.group_id,
                        principalTable: "groups_of_services",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Attendances_users_client_id",
                        column: x => x.client_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attendances_users_worker_id",
                        column: x => x.worker_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    address = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    gender = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.id);
                    table.ForeignKey(
                        name: "FK_Workers_users_id",
                        column: x => x.id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Workers_Groups",
                columns: table => new
                {
                    worker_id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    group_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers_Groups", x => new { x.worker_id, x.group_id });
                    table.ForeignKey(
                        name: "FK_Workers_Groups_Workers_worker_id",
                        column: x => x.worker_id,
                        principalTable: "Workers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Workers_Groups_groups_of_services_group_id",
                        column: x => x.group_id,
                        principalTable: "groups_of_services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_client_id_date_service_id",
                table: "Attendances",
                columns: new[] { "client_id", "date", "service_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_group_id",
                table: "Attendances",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_service_id",
                table: "Attendances",
                column: "service_id");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_worker_id_date_service_id",
                table: "Attendances",
                columns: new[] { "worker_id", "date", "service_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_groups_of_services_name",
                table: "groups_of_services",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_name",
                table: "Services",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_Groups_group_id",
                table: "Services_Groups",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_Services_Groups_service_id",
                table: "Services_Groups",
                column: "service_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workers_Groups_group_id",
                table: "Workers_Groups",
                column: "group_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "Services_Groups");

            migrationBuilder.DropTable(
                name: "Workers_Groups");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Workers");

            migrationBuilder.DropTable(
                name: "groups_of_services");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
