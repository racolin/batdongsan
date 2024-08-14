using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddConfigurationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SendEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SendEmailPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiveEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactZaloLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SystemPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SystemEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SystemZaloLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SystemYoutubeLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SystemFacebookLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configurations", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configurations");
        }
    }
}
