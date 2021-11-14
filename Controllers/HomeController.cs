using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using TMC.AppRepository;
using TMC.Models;

namespace TMC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View("OnlineTheatreFestival");
        }

        public IActionResult AllPlays()
        {
            return View();
        }

        public IActionResult Plays(int Id)
        {
            if(Id>0)
            {
                return View();
            }
            else
            {
                return View("AllPlays");
            }
        }

        public IActionResult OnlineTheatreFestival()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAllPlays()
        {
            var resp = new ajaxResponse()
            {
                data = Play.fn_GetAllExistingPlays().Select(x => new playHomePageListDisplaymodel()
                {
                    TokenID = x.ID,
                    DateCreated = x.DATECREATED.ToString("dddd dd MMMM", CultureInfo.CreateSpecificCulture("en-US")),
                    ThumbnailUrl = x.IMAGEURL,
                    Title = x.TITLE,
                    About = (x.SYNOPSIS.Length > 100 ? x.SYNOPSIS.Substring(0, 100) : x.SYNOPSIS),
                    BookUrl = x.TRAILERLINK
                }).ToList(),
                respstatus = ResponseStatus.success
            };
            return Json(resp);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
