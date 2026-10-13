using BookManager.Data;
using BookManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace BookManager.Controllers;
public class BooksController(LibraryContext db) : Controller
{
    public async Task<IActionResult> Index(string? search, string? category, int page = 1)
    {
        var query = db.Books.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) { search = search.Trim(); query = query.Where(b => b.Title.Contains(search) || b.Author.Contains(search)); }
        if (!string.IsNullOrWhiteSpace(category)) query = query.Where(b => b.Category == category);
        var count = await query.CountAsync();
        var pages = Math.Max(1, (int)Math.Ceiling(count / 8.0));
        page = Math.Clamp(page, 1, pages);
        return View(new BookListViewModel { Books = await query.OrderByDescending(b => b.Id).Skip((page-1)*8).Take(8).ToListAsync(), Categories=await db.Books.Select(b=>b.Category).Distinct().OrderBy(c=>c).ToListAsync(), Search=search, Category=category, Page=page, TotalPages=pages, FilteredCount=count, TotalBooks=await db.Books.CountAsync(), TotalQuantity=await db.Books.SumAsync(b=>(int?)b.Quantity) ?? 0 });
    }
    public async Task<IActionResult> Details(int id) { var book=await db.Books.AsNoTracking().FirstOrDefaultAsync(b=>b.Id==id); return book is null ? NotFound() : View(book); }
    public IActionResult Create() => View(new Book());
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,Author,Category,Publisher,PublishedYear,Price,Quantity,Description")] Book book)
    {
        if (!ModelState.IsValid) return View(book);
        db.Add(book); await db.SaveChangesAsync(); TempData["Success"]="Đã thêm sách mới."; return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Edit(int id) { var book=await db.Books.FindAsync(id); return book is null ? NotFound() : View(book); }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,Category,Publisher,PublishedYear,Price,Quantity,Description")] Book book)
    {
        if(id!=book.Id) return BadRequest();
        if(!ModelState.IsValid) return View(book);
        var existing=await db.Books.FindAsync(id); if(existing is null) return NotFound();
        db.Entry(existing).CurrentValues.SetValues(book); await db.SaveChangesAsync(); TempData["Success"]="Đã cập nhật thông tin sách."; return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int id) { var book=await db.Books.AsNoTracking().FirstOrDefaultAsync(b=>b.Id==id); return book is null ? NotFound() : View(book); }
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var book=await db.Books.FindAsync(id); if(book is null) return NotFound();
        db.Remove(book); await db.SaveChangesAsync(); TempData["Success"]="Đã xóa sách."; return RedirectToAction(nameof(Index));
    }
}
