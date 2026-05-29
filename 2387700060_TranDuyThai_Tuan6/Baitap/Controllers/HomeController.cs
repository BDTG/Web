using System.Diagnostics;
using Baitap.Models;
using Baitap.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Baitap.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
    private readonly IProductRepository _productRepo;
    private readonly ICategoryRepository _categoryRepo;

    public HomeController(IProductRepository productRepo, ICategoryRepository categoryRepo)
    {
        _productRepo = productRepo;
        _categoryRepo = categoryRepo;
    }

    public async Task<IActionResult> Index(int? categoryId)
    {
        var categories = await _categoryRepo.GetAllAsync();
        ViewBag.Categories = categories;
        ViewBag.SelectedCategoryId = categoryId;

        IEnumerable<Product> products;
        if (categoryId.HasValue)
            products = await _productRepo.GetByCategoryIdAsync(categoryId.Value);
        else
            products = await _productRepo.GetAllAsync();

        return View(products);
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _productRepo.GetByIdAsync(id);
        if (product is null) return NotFound();

        return View(product);
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
