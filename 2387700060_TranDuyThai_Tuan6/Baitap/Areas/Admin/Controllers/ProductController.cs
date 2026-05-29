using Baitap.Models;
using Baitap.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Baitap.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ProductController : Controller
{
    private readonly IProductRepository _productRepo;
    private readonly ICategoryRepository _categoryRepo;

    public ProductController(IProductRepository productRepo, ICategoryRepository categoryRepo)
    {
        _productRepo = productRepo;
        _categoryRepo = categoryRepo;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productRepo.GetAllAsync();
        return View(products);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await _categoryRepo.GetAllAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Price,Description,CategoryId")] Product product)
    {
        ModelState.Remove("Category");
        ModelState.Remove("Images");

        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await _categoryRepo.GetAllAsync();
            return View(product);
        }

        product.CreatedAt = DateTime.UtcNow;
        await _productRepo.AddAsync(product);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var product = await _productRepo.GetByIdAsync(id);
        if (product is null) return NotFound();

        ViewBag.Categories = await _categoryRepo.GetAllAsync();
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description,CategoryId")] Product product)
    {
        if (id != product.Id) return NotFound();

        ModelState.Remove("Category");
        ModelState.Remove("Images");

        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await _categoryRepo.GetAllAsync();
            return View(product);
        }

        var existing = await _productRepo.GetByIdAsync(id);
        if (existing is null) return NotFound();

        existing.Name = product.Name;
        existing.Price = product.Price;
        existing.Description = product.Description;
        existing.CategoryId = product.CategoryId;
        await _productRepo.UpdateAsync(existing);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var product = await _productRepo.GetByIdAsync(id);
        if (product is null) return NotFound();

        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _productRepo.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
