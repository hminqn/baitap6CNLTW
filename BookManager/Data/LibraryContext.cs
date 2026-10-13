using BookManager.Models;
using Microsoft.EntityFrameworkCore;
namespace BookManager.Data;
public class LibraryContext(DbContextOptions<LibraryContext> options) : DbContext(options)
{
    public DbSet<Book> Books => Set<Book>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>().Property(b => b.Price).HasPrecision(18, 2);
        modelBuilder.Entity<Book>().HasData(
            new Book { Id=1, Title="Dế Mèn phiêu lưu ký", Author="Tô Hoài", Category="Văn học", Publisher="Kim Đồng", PublishedYear=2020, Price=75000, Quantity=24, Description="Hành trình trưởng thành và những cuộc phiêu lưu của Dế Mèn." },
            new Book { Id=2, Title="Tôi thấy hoa vàng trên cỏ xanh", Author="Nguyễn Nhật Ánh", Category="Văn học", Publisher="Trẻ", PublishedYear=2018, Price=125000, Quantity=18, Description="Câu chuyện tuổi thơ trong trẻo ở một làng quê Việt Nam." },
            new Book { Id=3, Title="Clean Code", Author="Robert C. Martin", Category="Công nghệ", Publisher="Prentice Hall", PublishedYear=2008, Price=450000, Quantity=8, Description="Các nguyên tắc viết mã nguồn rõ ràng và dễ bảo trì." },
            new Book { Id=4, Title="Nhà giả kim", Author="Paulo Coelho", Category="Tiểu thuyết", Publisher="Văn học", PublishedYear=2020, Price=89000, Quantity=32, Description="Hành trình theo đuổi ước mơ của chàng chăn cừu Santiago." },
            new Book { Id=5, Title="Lược sử thời gian", Author="Stephen Hawking", Category="Khoa học", Publisher="Trẻ", PublishedYear=2021, Price=165000, Quantity=12, Description="Khám phá các câu hỏi lớn về vũ trụ và thời gian." });
    }
}
