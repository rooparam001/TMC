using EntitesInterfaces.AppModels;
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
            return View("AllPlays");
            //return View();
        }

        public IActionResult AllPlays() => View();
        public IActionResult AllDirectors() => View();

        public IActionResult Plays(int objToken)
        {
            if (objToken > 0)
            {
                return View();
            }
            else
            {
                return View("AllPlays");
            }
        }

        [HttpGet]
        public JsonResult GetAllGenres()
        {
            var resp = new ajaxResponse()
            {
                data = Play.fn_GetAllGenres(),
                respstatus = ResponseStatus.success
            };
            return Json(resp);
        }

        [HttpGet]
        public JsonResult GetAllLanguages()
        {
            var resp = new ajaxResponse()
            {
                data = Play.fn_GetAllLanguages(),
                respstatus = ResponseStatus.success
            };
            return Json(resp);
        }

        [HttpGet]
        public JsonResult GetAllCities()
        {
            var resp = new ajaxResponse()
            {
                data = Play.fn_GetAllCities(),
                respstatus = ResponseStatus.success
            };
            return Json(resp);
        }

        public IActionResult OnlineTheatreFestival() => View();

        [HttpGet]
        public JsonResult GetAllPlays(string genres = "", string languages = "", string cities = "")
        {
            var resp = new ajaxResponse()
            {
                data = Play.fn_GetAllExistingPlays(genres, languages, cities).Select(x => new playHomePageListDisplaymodel()
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

        [HttpGet]
        public JsonResult GetPlayByID(int obj)
        {
            var resp = new ajaxResponse()
            {
                data = Play.fn_GetSinglePlayByID(obj),
                respstatus = ResponseStatus.success
            };
            return Json(resp);
        }

        [HttpGet]
        public JsonResult GetAllDirectors()
        {
            var resp = new ajaxResponse()
            {
                data = Director.GetAllDirectors().Select(x => new directorModel()
                {
                    ID = x.ID,
                    ImageURL = x.OBJECTIMGURL,
                    Title = x.OBJECTNAME,
                    DateCreated = x.DATECREATED.ToString("dddd dd MMMM", CultureInfo.CreateSpecificCulture("en-US")),
                    Description = x.OBJECTDESCRIPTION
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
