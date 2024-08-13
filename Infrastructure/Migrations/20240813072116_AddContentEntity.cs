using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddContentEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SliderImageEntity_Images_ImageId",
                table: "SliderImageEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_SliderImageEntity_Sliders_SliderEntityId",
                table: "SliderImageEntity");

            migrationBuilder.DropTable(
                name: "ImagePages");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "Sliders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SliderImageEntity",
                table: "SliderImageEntity");

            migrationBuilder.RenameTable(
                name: "SliderImageEntity",
                newName: "SliderImages");

            migrationBuilder.RenameColumn(
                name: "SliderEntityId",
                table: "SliderImages",
                newName: "ContentEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_SliderImageEntity_SliderEntityId",
                table: "SliderImages",
                newName: "IX_SliderImages_ContentEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_SliderImageEntity_ImageId",
                table: "SliderImages",
                newName: "IX_SliderImages_ImageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SliderImages",
                table: "SliderImages",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Contents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeImageId = table.Column<int>(type: "int", nullable: true),
                    BgHomeImageId = table.Column<int>(type: "int", nullable: true),
                    NewsImageId = table.Column<int>(type: "int", nullable: true),
                    ContactImageId = table.Column<int>(type: "int", nullable: true),
                    IntroduceSection = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewsMarketSection = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewsProjectSection = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contents_Images_BgHomeImageId",
                        column: x => x.BgHomeImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contents_Images_ContactImageId",
                        column: x => x.ContactImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contents_Images_HomeImageId",
                        column: x => x.HomeImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contents_Images_NewsImageId",
                        column: x => x.NewsImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contents_BgHomeImageId",
                table: "Contents",
                column: "BgHomeImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_ContactImageId",
                table: "Contents",
                column: "ContactImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_HomeImageId",
                table: "Contents",
                column: "HomeImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_NewsImageId",
                table: "Contents",
                column: "NewsImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_SliderImages_Contents_ContentEntityId",
                table: "SliderImages",
                column: "ContentEntityId",
                principalTable: "Contents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SliderImages_Images_ImageId",
                table: "SliderImages",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SliderImages_Contents_ContentEntityId",
                table: "SliderImages");

            migrationBuilder.DropForeignKey(
                name: "FK_SliderImages_Images_ImageId",
                table: "SliderImages");

            migrationBuilder.DropTable(
                name: "Contents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SliderImages",
                table: "SliderImages");

            migrationBuilder.RenameTable(
                name: "SliderImages",
                newName: "SliderImageEntity");

            migrationBuilder.RenameColumn(
                name: "ContentEntityId",
                table: "SliderImageEntity",
                newName: "SliderEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_SliderImages_ImageId",
                table: "SliderImageEntity",
                newName: "IX_SliderImageEntity_ImageId");

            migrationBuilder.RenameIndex(
                name: "IX_SliderImages_ContentEntityId",
                table: "SliderImageEntity",
                newName: "IX_SliderImageEntity_SliderEntityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SliderImageEntity",
                table: "SliderImageEntity",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ImagePages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Position = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagePages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImagePages_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Position = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sliders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Position = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sliders", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImagePages_ImageId",
                table: "ImagePages",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_SliderImageEntity_Images_ImageId",
                table: "SliderImageEntity",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SliderImageEntity_Sliders_SliderEntityId",
                table: "SliderImageEntity",
                column: "SliderEntityId",
                principalTable: "Sliders",
                principalColumn: "Id");
        }
    }
}
