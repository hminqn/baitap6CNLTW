using System.ComponentModel.DataAnnotations;
namespace BookManager.Models;
public class Book
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập tên sách."), StringLength(200), Display(Name = "Tên sách")]
    public string Title { get; set; } = "";
    [Required(ErrorMessage = "Vui lòng nhập tác giả."), StringLength(120), Display(Name = "Tác giả")]
    public string Author { get; set; } = "";
    [Required(ErrorMessage = "Vui lòng nhập thể loại."), StringLength(80), Display(Name = "Thể loại")]
    public string Category { get; set; } = "";
    [StringLength(120), Display(Name = "Nhà xuất bản")]
    public string? Publisher { get; set; }
    [Range(1, 9999, ErrorMessage = "Năm xuất bản phải từ 1 đến 9999."), Display(Name = "Năm xuất bản")]
    public int PublishedYear { get; set; } = DateTime.Today.Year;
    [Range(0, 1000000000, ErrorMessage = "Giá phải từ 0 đến 1 tỷ đồng."), Display(Name = "Giá bán (VNĐ)")]
    public decimal Price { get; set; }
    [Range(0, 1000000, ErrorMessage = "Số lượng phải từ 0 đến 1 triệu."), Display(Name = "Số lượng")]
    public int Quantity { get; set; }
    [StringLength(2000), Display(Name = "Mô tả")]
    public string? Description { get; set; }
}
public class BookListViewModel
{
    public List<Book> Books { get; set; } = [];
    public List<string> Categories { get; set; } = [];
    public string? Search { get; set; }
    public string? Category { get; set; }
    public int Page { get; set; }
    public int TotalPages { get; set; }
    public int TotalBooks { get; set; }
    public int TotalQuantity { get; set; }
    public int FilteredCount { get; set; }
}
