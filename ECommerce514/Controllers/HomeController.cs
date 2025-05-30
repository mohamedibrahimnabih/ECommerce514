using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ECommerce514.Models;
using ECommerce514.ViewModels;
using ECommerce514.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ECommerce514.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context = new();

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index(ProductsWithFilterVM productsWithFilterVM, int page = 1)
    {
        IQueryable<Product> product = _context.Products.Include(e => e.Category);
        var categories = _context.Categories;
        const double discountThreshold = 50;
        const double totalNumberOfProductInPages = 8.0;

        #region Filter Product
        if (productsWithFilterVM.ProductName is not null)
        {
            product = product.Where(e => e.Name.Contains(productsWithFilterVM.ProductName));
            //ViewData["productName"] = productName;
            ViewBag.productName = productsWithFilterVM.ProductName;
        }

        if (productsWithFilterVM.MinPrice > 0)
        {
            product = product.Where(e => e.Price - (e.Price * (decimal)(e.Discount / 100.0)) >= (decimal)productsWithFilterVM.MinPrice);
            //ViewData["minPrice"] = minPrice;
            ViewBag.minPrice = productsWithFilterVM.MinPrice;
        }

        if (productsWithFilterVM.MaxPrice > 0)
        {
            product = product.Where(e => e.Price - (e.Price * (decimal)(e.Discount / 100.0)) <= (decimal)productsWithFilterVM.MaxPrice);
            //ViewData["maxPrice"] = maxPrice;
            ViewBag.maxPrice = productsWithFilterVM.MaxPrice;
        }

        if (productsWithFilterVM.CategoryId > 0 && productsWithFilterVM.CategoryId < categories.Count())
        {
            product = product.Where(e => e.CategoryId == productsWithFilterVM.CategoryId);
            ViewData["categoryId"] = productsWithFilterVM.CategoryId;
            //ViewBag.categoryId = categoryId;
        }

        if (productsWithFilterVM.IsHot)
        {
            product = product.Where(e => e.Discount > discountThreshold);
            ViewBag.isHot = productsWithFilterVM.IsHot;
        }
        #endregion

        #region Pagination
        var totalNumberOfPages = Math.Ceiling(product.Count() / totalNumberOfProductInPages);

        if (totalNumberOfPages < page)
            return NotFound();

        product = product.Skip((page - 1) * (int)totalNumberOfProductInPages).Take((int)totalNumberOfProductInPages);

        ViewBag.totalNumberOfPages = totalNumberOfPages;
        ViewBag.currentPage = page;
        #endregion

        #region Categories
        ViewData["listOfCategories"] = categories.ToList(); 
        #endregion

        return View(product.ToList());
    }

    public IActionResult Details(int id)
    {
        var product = _context.Products.Include(e => e.Category).FirstOrDefault(e => e.ProductId == id);

        if(product is not null)
        {
            var relatedProduct = _context.Products.Where(e => e.CategoryId == product.CategoryId && id != e.ProductId).Skip(0).Take(4);

            var topProduct = _context.Products.Where(e => e.ProductId != id).OrderByDescending(e => e.Traffic).Skip(0).Take(4);

            var similarProductsName = _context.Products.Where(e=>e.Name.Contains(product.Name) && id != e.ProductId).Skip(0).Take(4);

            product.Traffic++;
            _context.SaveChanges();

            return View(new ProductWithRelatedVM()
            {
                Product = product,
                RelatedProducts = relatedProduct.ToList(),
                TopProducts = topProduct.ToList(),
                SimilarProductsName = similarProductsName.ToList(),
            });
        }

        return NotFound();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Welcome()
    {
        int age = 27;
        string name = "Mohamed";
        string address = "Mansoura";

        List<string> skills = new List<string>
        {
            new("C++"),
            new("C#"),
            new("SQL SERVER"),
            new("EF"),
        };

        return View(model: new PersonVM {
            Age =  age,
            Name = name,
            Address = address,
            Skills = skills
        });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
