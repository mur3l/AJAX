using Microsoft.AspNetCore.Mvc;

namespace AdvancedAjax.Controllers
{
    public class CityController : Controller
    {
        private readonly AppDbContext _context;

        public CityController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<City> cities = _context.Cities
                 .Include(c => c.Country)
                 .ToList();
            return View(cities);
        }

        [HttpGet]
        public IActionResult Create()
        {
            City City = new City();
            ViewBag.Countries = GetCountries();
            return View(City);
        }

        
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(City City)
        {
            _context.Add(City);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            City City = _context.Cities
                .Include(co => co.Country)
                .Where(c => c.Id == id).FirstOrDefault();

            return View(City);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            City City = _context.Cities.Where(c => c.Id == Id).FirstOrDefault();
            ViewBag.Countries = GetCountries();
            return View(City);
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(City City)
        {
            _context.Attach(City);
            _context.Entry(City).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            City city = _context.Cities
                .Include(co => co.Country)
                .Where(c => c.Id == id).FirstOrDefault();
            return View(city);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Delete(City city)

        {
            try
            {
                _context.Attach(city);
                _context.Entry(city).State = EntityState.Deleted;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _context.Entry(city).Reload();
                ModelState.AddModelError("", ex.InnerException.Message);
                return View(city);
            }
            return RedirectToAction(nameof(Index));

        }

        private List<SelectListItem> GetCountries()
        {
            var lstCountries = new List <SelectListItem>();
            List<Country> Countries = _context.Countries.ToList();
            lstCountries = Countries.Select(ct => new SelectListItem()
            {
                Value = ct.Id.ToString(),
                Text = ct.Name
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Select Country----"
            };
            lstCountries.Insert(0, defItem);
            return lstCountries;
        }

        [HttpGet]
        public IActionResult CreateModalForm(int countryId)
        {
            City city = new City();
            city.CountryId = countryId;
            city.CountryName = GetCountryName(countryId);
            return PartialView("_CreateModalForm", city);
        }

        [HttpPost]
        public IActionResult CreateModalForm(City city)
        {
            _context.Add(city);
            _context.SaveChanges();
            return NoContent();
        }

        private string GetCountryName(int countryId)
        {
            if (countryId == 0)
                return "";

            string strCountryName = _context.Countries
                .Where(c => c.Id == countryId)
                .Select(n => n.Name).Single().ToString();

            return strCountryName;
        }
    }
}