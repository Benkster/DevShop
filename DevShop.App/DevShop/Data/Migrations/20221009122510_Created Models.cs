using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevShop.Data.Migrations
{
    public partial class CreatedModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostTitle",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreTitle",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoleNr",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoleNr1",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CategoryDescr = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    ParentID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    CompCode = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    CompName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompAddName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CompDescr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Tel = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Mail = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.CompCode);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => new { x.CountryID, x.CountryCode });
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleNr = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RoleDescr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleNr);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    UnitCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    UnitName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.UnitCode);
                });

            migrationBuilder.CreateTable(
                name: "ProductGroups",
                columns: table => new
                {
                    ProductGroupNr = table.Column<int>(type: "int", nullable: false),
                    CompCode = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    GroupDescr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SortNr = table.Column<int>(type: "int", nullable: false),
                    ParentNr = table.Column<int>(type: "int", nullable: true),
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductGroups", x => new { x.ProductGroupNr, x.CompCode });
                    table.ForeignKey(
                        name: "FK_ProductGroups_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductGroups_Companies_CompCode",
                        column: x => x.CompCode,
                        principalTable: "Companies",
                        principalColumn: "CompCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    StateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StateName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CountryID = table.Column<int>(type: "int", nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    CountryID1 = table.Column<int>(type: "int", nullable: false),
                    CountryCode1 = table.Column<string>(type: "nvarchar(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.StateID);
                    table.ForeignKey(
                        name: "FK_States_Countries_CountryID1_CountryCode1",
                        columns: x => new { x.CountryID1, x.CountryCode1 },
                        principalTable: "Countries",
                        principalColumns: new[] { "CountryID", "CountryCode" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductNr = table.Column<int>(type: "int", nullable: false),
                    ProductGroupNr = table.Column<int>(type: "int", nullable: false),
                    CompCode = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ProductDescr = table.Column<string>(type: "nvarchar(800)", maxLength: 800, nullable: true),
                    SortNr = table.Column<int>(type: "int", nullable: false),
                    ProductGroupNr1 = table.Column<int>(type: "int", nullable: false),
                    ProductGroupCompCode = table.Column<string>(type: "nvarchar(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => new { x.ProductNr, x.ProductGroupNr });
                    table.ForeignKey(
                        name: "FK_Products_ProductGroups_ProductGroupNr1_ProductGroupCompCode",
                        columns: x => new { x.ProductGroupNr1, x.ProductGroupCompCode },
                        principalTable: "ProductGroups",
                        principalColumns: new[] { "ProductGroupNr", "CompCode" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    ZIP = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    StateID = table.Column<int>(type: "int", nullable: false),
                    CityName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => new { x.StateID, x.ZIP });
                    table.ForeignKey(
                        name: "FK_Cities_States_StateID",
                        column: x => x.StateID,
                        principalTable: "States",
                        principalColumn: "StateID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    ArticleNr = table.Column<int>(type: "int", nullable: false),
                    ProductNr = table.Column<int>(type: "int", nullable: false),
                    ProductGroupNr = table.Column<int>(type: "int", nullable: false),
                    ArticleName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ArticleDescr = table.Column<string>(type: "nvarchar(800)", maxLength: 800, nullable: true),
                    ArticleCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    EAN = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
                    SortNr = table.Column<int>(type: "int", nullable: false),
                    BillingUnit = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    OrderUnit = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    F1 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    F2 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    F3 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    F4 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    F5 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Discount = table.Column<float>(type: "real", nullable: true),
                    OverruleUserDiscount = table.Column<bool>(type: "bit", nullable: false),
                    ProductNr1 = table.Column<int>(type: "int", nullable: false),
                    ProductGroupNr1 = table.Column<int>(type: "int", nullable: false),
                    UnitCode = table.Column<string>(type: "nvarchar(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => new { x.ArticleNr, x.ProductNr });
                    table.ForeignKey(
                        name: "FK_Articles_Products_ProductNr1_ProductGroupNr1",
                        columns: x => new { x.ProductNr1, x.ProductGroupNr1 },
                        principalTable: "Products",
                        principalColumns: new[] { "ProductNr", "ProductGroupNr" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Articles_Units_UnitCode",
                        column: x => x.UnitCode,
                        principalTable: "Units",
                        principalColumn: "UnitCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    AddressID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StateID = table.Column<int>(type: "int", nullable: false),
                    ZIP = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    HouseNr = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AddressInfo = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CityStateID = table.Column<int>(type: "int", nullable: false),
                    CityZIP = table.Column<string>(type: "nvarchar(5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressID);
                    table.ForeignKey(
                        name: "FK_Addresses_Cities_CityStateID_CityZIP",
                        columns: x => new { x.CityStateID, x.CityZIP },
                        principalTable: "Cities",
                        principalColumns: new[] { "StateID", "ZIP" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserDiscounts",
                columns: table => new
                {
                    UserDiscountID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductNr = table.Column<int>(type: "int", nullable: false),
                    ArticleNr = table.Column<int>(type: "int", nullable: false),
                    Discount = table.Column<float>(type: "real", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ArticleNr1 = table.Column<int>(type: "int", nullable: false),
                    ArticleProductNr = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDiscounts", x => x.UserDiscountID);
                    table.ForeignKey(
                        name: "FK_UserDiscounts_Articles_ArticleNr1_ArticleProductNr",
                        columns: x => new { x.ArticleNr1, x.ArticleProductNr },
                        principalTable: "Articles",
                        principalColumns: new[] { "ArticleNr", "ProductNr" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDiscounts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AddressCompany",
                columns: table => new
                {
                    AddressesAddressID = table.Column<int>(type: "int", nullable: false),
                    CompaniesCompCode = table.Column<string>(type: "nvarchar(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressCompany", x => new { x.AddressesAddressID, x.CompaniesCompCode });
                    table.ForeignKey(
                        name: "FK_AddressCompany_Addresses_AddressesAddressID",
                        column: x => x.AddressesAddressID,
                        principalTable: "Addresses",
                        principalColumn: "AddressID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AddressCompany_Companies_CompaniesCompCode",
                        column: x => x.CompaniesCompCode,
                        principalTable: "Companies",
                        principalColumn: "CompCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RoleNr1",
                table: "AspNetUsers",
                column: "RoleNr1");

            migrationBuilder.CreateIndex(
                name: "IX_AddressCompany_CompaniesCompCode",
                table: "AddressCompany",
                column: "CompaniesCompCode");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CityStateID_CityZIP",
                table: "Addresses",
                columns: new[] { "CityStateID", "CityZIP" });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ProductNr1_ProductGroupNr1",
                table: "Articles",
                columns: new[] { "ProductNr1", "ProductGroupNr1" });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_UnitCode",
                table: "Articles",
                column: "UnitCode");

            migrationBuilder.CreateIndex(
                name: "IX_ProductGroups_CategoryID",
                table: "ProductGroups",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductGroups_CompCode",
                table: "ProductGroups",
                column: "CompCode");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductGroupNr1_ProductGroupCompCode",
                table: "Products",
                columns: new[] { "ProductGroupNr1", "ProductGroupCompCode" });

            migrationBuilder.CreateIndex(
                name: "IX_States_CountryID1_CountryCode1",
                table: "States",
                columns: new[] { "CountryID1", "CountryCode1" });

            migrationBuilder.CreateIndex(
                name: "IX_UserDiscounts_ArticleNr1_ArticleProductNr",
                table: "UserDiscounts",
                columns: new[] { "ArticleNr1", "ArticleProductNr" });

            migrationBuilder.CreateIndex(
                name: "IX_UserDiscounts_UserId",
                table: "UserDiscounts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Roles_RoleNr1",
                table: "AspNetUsers",
                column: "RoleNr1",
                principalTable: "Roles",
                principalColumn: "RoleNr",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Roles_RoleNr1",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AddressCompany");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "UserDiscounts");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.DropTable(
                name: "ProductGroups");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RoleNr1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PostTitle",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PreTitle",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RoleNr",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RoleNr1",
                table: "AspNetUsers");
        }
    }
}
