using EntitesInterfaces.AppModels;
using EntitesInterfaces.DBEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
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
            //return View("AllPlays");
            return View();
        }

        public IActionResult AllPlays() => View();
        public IActionResult AllDirectors() => View();
        public IActionResult Giveaway() => View();
        public IActionResult Backstage() => View();
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
        public JsonResult GetAllUpcomingPlay()
        {
            var resp = new ajaxResponse()
            {
                data = Play.fn_GetAllPlays(),
                respstatus = ResponseStatus.success
            };
            return Json(resp);
        }

        [HttpGet]
        public JsonResult GetAllGiveaways(int ID = 0, int city = 0, string searchTxt = "", int giveawayType = 0)
        {
            var resp = new ajaxResponse()
            {
                data = GiveAways.getAll(ID, city, searchTxt, giveawayType),
                respstatus = ResponseStatus.success
            };
            return Json(resp);
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

        [HttpGet]
        public JsonResult GetAllRoles()
        {
            var outputModel = new List<Tbl_RoleMaster>();
            outputModel = AppUsers.fn_GetAllRoles();
            outputModel = outputModel.Where(x => x.RoleName != "VIEWER" && x.RoleName != "ADMINISTRATOR").ToList();
            var resp = new ajaxResponse()
            {
                data = outputModel,
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

        [HttpPost]
        public ActionResult SaveEnquiry(string name = "", string email = "", string subject = "", string message = "")
        {
            var resp = new ajaxResponse()
            {
                data = (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(message)) ? "" :
                Enquiries.Save(new EntitesInterfaces.DBEntities.TBL_ENQUIRYMASTER()
                {
                    USERNAME = name.Trim(),
                    DATECREATED = System.DateTime.Now,
                    EMAILADD = email.Trim(),
                    SEENSTATUS = false,
                    USERMESSAGE = message.Trim(),
                    USERSUBJECT = subject.Trim()
                }),
                respstatus = ResponseStatus.success
            };
            return View("Index");
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

        [HttpGet]
        public JsonResult GetAllProfiles(int city = 0, int role = 0, string gender = "", string language = "")
        {
            var respModel = new List<profileMasterViewModel>();
            respModel = AppProfiles.GetAllProfiles(city, role, gender, language);
            var resp = new ajaxResponse()
            {
                data = respModel,
                respstatus = ResponseStatus.success,
                respmessage = (respModel.Count > 0 ? "Success" : "Something went wrong, please try again later.")
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
