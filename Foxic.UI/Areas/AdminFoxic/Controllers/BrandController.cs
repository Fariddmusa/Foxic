using AutoMapper;
using Foxic.Buisness.Exceptions;
using Foxic.Buisness.Services.Interfaces;
using Foxic.Buisness.ViewModels.AreasViewModels.BrandVMs;
using Foxic.Buisness.ViewModels.AreasViewModels.ColorVMs;
using Foxic.Buisness.ViewModels.SliderViewModels;
using Foxic.Core.Entities;
using Foxic.Core.Entities.AreasEntitycontroller;
using Foxic.DataAccess.contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoxicUI.Areas.Admin.Controllers;
[Area("Admin")]
public class BrandController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webEnv;
    private readonly IFileService _fileservice;
    public BrandController(AppDbContext context,IWebHostEnvironment webEnv,IFileService fileservice)
    {
        _context = context;
        _webEnv = webEnv;
        _fileservice = fileservice;
    }
    public async Task<IActionResult> Details(int id)
    {
        Brand? brand = await _context.Brands.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
        if (brand == null) return NotFound();
        return View(brand);
    }
    public IActionResult Brands(int Id)
    {
        Brand brand = _context.Brands.FirstOrDefault(x => x.Id == Id);
        List<Brand> brands = _context.Brands.ToList();
        return View(brands);
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BrandCreateVM brand)
    {
        if (!ModelState.IsValid) return View(brand);
        string filename = string.Empty;
        try
        {
            Brand brand1 = new()
            {
                BrandName = brand.BrandName
            };
            filename = await _fileservice.UploadFile(brand.Image, _webEnv.WebRootPath, 300, "assets", "images", "slider");
            brand1.Image = filename;
            await _context.Brands.AddAsync(brand1);
            await _context.SaveChangesAsync();
        }
        catch (FileSizeException ex)
        {
            ModelState.AddModelError("BrandImage", ex.Message);
            return View(brand);
        }
        catch (FileTypeException ex)
        {
            ModelState.AddModelError("BrandImage", ex.Message);
            return View(brand);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(brand);
        }
        return RedirectToAction(nameof(Brands));
    }
    public async Task<IActionResult> Delete(int id)
    {
        Brand? brand = await _context.Brands.FindAsync(id);
        if (brand == null) return NotFound();
        return View(brand);
    }
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePost(int id)
    {
        Brand? brand = await _context.Brands.FindAsync(id);
        if (brand == null) return NotFound();
        string fileroot = Path.Combine(_webEnv.WebRootPath, brand.Image);
        if (System.IO.File.Exists(fileroot))
        {
            System.IO.File.Delete(fileroot);
        }
        _context.Brands.Remove(brand);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Brands));
    }
}
