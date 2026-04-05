using CruiseApp.Services.Core.Interfaces;
using CruiseApp.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;

public class AdminShipController : Controller
{
    private readonly IShipImportService importService;

    public AdminShipController(IShipImportService importService)
    {
        this.importService = importService;
    }

    [HttpGet]
    public IActionResult ImportShip()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads/packages");

        var model = new ShipImportViewModel
        {
            Files = Directory.GetFiles(path, "*.zip")
                .Select(Path.GetFileName)
                .ToList()
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ImportShip(ShipImportViewModel model)
    {
        if (string.IsNullOrEmpty(model.SelectedFile))
        {
            TempData["Error"] = "Please select a file";
            return RedirectToAction(nameof(ImportShip));
        }

        var fullPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Uploads/packages",
            Path.GetFileName(model.SelectedFile));

        try
        {
            await importService.ImportShipAsync(fullPath);

            TempData["Success"] = $"Ship '{model.SelectedFile}' imported successfully!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(ImportShip));
    }

}