using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ECommerce514.Models;
using ECommerce514.ViewModels;

namespace ECommerce514.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
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
            new("SQL SERVER")
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
