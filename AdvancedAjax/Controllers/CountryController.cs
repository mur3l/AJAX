﻿using Microsoft.AspNetCore.Mvc;

namespace AdvancedAjax.Controllers
{ 
public class CountryController : Controller
{
    private readonly AppDbContext _context;

    public CountryController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        List<Country> countries;
        countries = _context.Countries.ToList();
        return View(countries);
    }

    [HttpGet]

    public IActionResult Create()
    {
        Country country = new Country();
        return View(country);
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public IActionResult Create(Country country)
    {
        _context.Add(country);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult CreateModalForm()
    {
        Country country = new Country();
        return PartialView("_CreateModalForm", country);
    }

    [HttpPost]
    public IActionResult CreateModalForm(Country country)
    {
        _context.Add(country);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpGet]
    public IActionResult Details(int Id)
    {
        Country country = GetCountry(Id);
        return View(country);
    }

    [HttpGet]
    public IActionResult Edit(int Id)
    {
        Country country = GetCountry(Id);
        return View(country);
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public IActionResult Edit(Country country)
    {
        _context.Attach(country);
        _context.Entry(country).State = EntityState.Modified;
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    private Country GetCountry(int Id)
    {
       Country country;
       country = _context.Countries.Where(c => c.Id == Id)
            .FirstOrDefault();
       return country;
    }

        [HttpGet]
    public IActionResult Delete(int Id)
    {
        Country country = GetCountry(Id);
        return View(country);
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public IActionResult Delete(Country country)
    {
        try
        {
            _context.Attach(country);
            _context.Entry(country).State = EntityState.Deleted;
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            _context.Entry(country).Reload();
            ModelState.AddModelError("", e.InnerException.Message);
            return View(country);
        }

        return RedirectToAction(nameof(Index));
    }

    public JsonResult GetCountries()
    {
        var lstCountries = new List<SelectListItem>();
        List<Country> Countries = _context.Countries.ToList();
        lstCountries = Countries.Select(ct => new SelectListItem()
        {
            Value = ct.Id.ToString(),
            Text = ct.Name
        }).ToList();

        var defItem = new SelectListItem()
        {
            Value = "",
            Text = "Select Country"
        };

        lstCountries.Insert(0, defItem);
        return Json(lstCountries);
    }

}
}