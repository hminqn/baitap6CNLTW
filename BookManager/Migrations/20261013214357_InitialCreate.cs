using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookManager.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Author = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    Category = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    Publisher = table.Column<string>(type: "TEXT", maxLength: 120, nullable: true),
                    PublishedYear = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "Category", "Description", "Price", "PublishedYear", "Publisher", "Quantity", "Title" },
                values: new object[,]
                {
                    { 1, "Tô Hoài", "Văn học", "Hành trình trưởng thành và những cuộc phiêu lưu của Dế Mèn.", 75000m, 2020, "Kim Đồng", 24, "Dế Mèn phiêu lưu ký" },
                    { 2, "Nguyễn Nhật Ánh", "Văn học", "Câu chuyện tuổi thơ trong trẻo ở một làng quê Việt Nam.", 125000m, 2018, "Trẻ", 18, "Tôi thấy hoa vàng trên cỏ xanh" },
                    { 3, "Robert C. Martin", "Công nghệ", "Các nguyên tắc viết mã nguồn rõ ràng và dễ bảo trì.", 450000m, 2008, "Prentice Hall", 8, "Clean Code" },
                    { 4, "Paulo Coelho", "Tiểu thuyết", "Hành trình theo đuổi ước mơ của chàng chăn cừu Santiago.", 89000m, 2020, "Văn học", 32, "Nhà giả kim" },
                    { 5, "Stephen Hawking", "Khoa học", "Khám phá các câu hỏi lớn về vũ trụ và thời gian.", 165000m, 2021, "Trẻ", 12, "Lược sử thời gian" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");
        }
    }
}
