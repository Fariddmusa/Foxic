using Foxic.Buisness.Exceptions;
using Foxic.Buisness.Services.Interfaces;
using Foxic.Buisness.ViewModels.AreasViewModels.BrandVMs;
using Foxic.Buisness.ViewModels.AreasViewModels.ColorVMs;
using Foxic.Core.Entities;
using Foxic.DataAccess.contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Drawing2D;

namespace FoxicUI.Areas.Admin.Controllers;
[Area("Admin")]
public class ColorController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webEnv;
    private readonly IFileService _fileservice;
    public ColorController(AppDbContext context,IWebHostEnvironment webEnv,IFileService fileservice)
    {
        _context = context;
        _webEnv = webEnv;
        _fileservice = fileservice;
    }
    public IActionResult Colors(int Id)
    {
        Color color = _context.Colors.FirstOrDefault(x => x.Id == Id);
        List<Color> colors = _context.Colors.ToList();
        return View(colors);
    }
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ColorCreateVM color)
    {
        if (!ModelState.IsValid) return View(color);
        string filename = string.Empty;

        try
        {
            Color color1 = new()
            {
                Name = color.ColorName
            };
            filename = await _fileservice.UploadFile(color.Image, _webEnv.WebRootPath, 300, "assets", "images", "slider");
            color1.Image = filename;
            await _context.Colors.AddAsync(color1);
            await _context.SaveChangesAsync();
        }
        catch (FileSizeException ex)
        {
            ModelState.AddModelError("BrandImage", ex.Message);
            return View(color);
        }
        catch (FileTypeException ex)
        {
            ModelState.AddModelError("BrandImage", ex.Message);
            return View(color);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(color);
        }
        return RedirectToAction(nameof(Colors));
    }
    public async Task<IActionResult> Delete(int id)
    {
        Color? color = await _context.Colors.FindAsync(id);
        if (color == null) return NotFound();
        return View(color);
    }
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePost(int id)
    {
        Color? color = await _context.Colors.FindAsync(id);
        if (color == null) return NotFound();
        string fileroot = Path.Combine(_webEnv.WebRootPath, color.Image);
        if (System.IO.File.Exists(fileroot))
        {
            System.IO.File.Delete(fileroot);
        }
        _context.Colors.Remove(color);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Colors));

    }
}
