using CognoGraphApp.Models;
using CognoGraphApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CognoGraphApp.Controllers;

public class HomeController : Controller
{
    private readonly GraphService _graphService;

    public HomeController(GraphService graphService)
    {
        _graphService = graphService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = new CareerSearchViewModel
        {
            AvailableRoles = await _graphService.GetRolesAsync()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(CareerSearchViewModel model)
    {
        model.AvailableRoles = await _graphService.GetRolesAsync();

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (model.CurrentRole == model.TargetRole)
        {
            model.ErrorMessage =
                "Current role and target role must be different.";

            return View(model);
        }

        try
        {
            model.Result = await _graphService.FindCareerPathAsync(
                model.CurrentRole!,
                model.TargetRole!);

            if (model.Result is null)
            {
                model.ErrorMessage =
                    "No career path was found between these roles.";
            }
        }
        catch
        {
            model.ErrorMessage =
                "The database is currently unavailable. Please try again.";
        }

        return View(model);
    }
}