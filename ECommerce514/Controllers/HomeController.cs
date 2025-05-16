using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ECommerce514.Models;
using ECommerce514.ViewModels;
using ECommerce514.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce514.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context = new();

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var product = _context.Products.Include(e => e.Category);

        // Add Filter
        // Join
        // Skip Take

        return View(product.ToList());
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
