using Baitap.Data;
using Baitap.Models;
using Microsoft.EntityFrameworkCore;

namespace Baitap.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
        => await _context.Categories.AsNoTracking().OrderBy(c => c.Name).ToListAsync();

    public async Task<Category?> GetByIdAsync(int id)
        => await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

    public async Task<Category?> GetWithProductsAsync(int id)
        => await _context.Categories
            .Include(c => c.Products)
                .ThenInclude(p => p.Images)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task AddAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteWithProductsAsync(int id)
    {
        var category = await _context.Categories
            .Include(c => c.Products)
                .ThenInclude(p => p.Images)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category is null) return;

        foreach (var product in category.Products)
        {
            _context.ProductImages.RemoveRange(product.Images);
        }
        _context.Products.RemoveRange(category.Products);
        _context.Categories.Remove(category);

        await _context.SaveChangesAsync();
    }
}
