using EntitesInterfaces.AppModels;
using EntitesInterfaces.DBEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
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
        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
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
        public IActionResult JoinTheRevolution() => View();
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
        public JsonResult SaveDonation()
        {
            var resp = new ajaxResponse();
            var relationModelObj = new DonationAddModel();
            try
            {
                resp.data = null;
                decimal amtentered = 0;
                decimal.TryParse(Request.Form["TOTAMOUNT"], out amtentered);
                int ID = 0;
                int.TryParse(Request.Form["ID"], out ID);
                relationModelObj = new DonationAddModel()
                {
                    CITY = Request.Form["CITY"],
                    CONTACTNUMBER = Request.Form["CONTACTNUMBER"],
                    EMAILID = Request.Form["EMAILID"],
                    FULLNAME = Request.Form["FULLNAME"],
                    TOTAMOUNT = amtentered
                };

                if (string.IsNullOrEmpty(relationModelObj.CITY) || string.IsNullOrEmpty(relationModelObj.CONTACTNUMBER) || string.IsNullOrEmpty(relationModelObj.EMAILID) || string.IsNullOrEmpty(relationModelObj.FULLNAME) || amtentered == 0)
                {
                    resp = new ajaxResponse()
                    {
                        data = null,
                        respmessage = "Every field is necessary and the amount should be greater than 0.",
                        respstatus = ResponseStatus.error
                    };
                }
                else
                {
                    relationModelObj = Donation.Save(relationModelObj);
                    if (relationModelObj != null)
                    {
                        if (relationModelObj.ID > 0)
                        {
                            relationModelObj.PKey = Donation.GenerateOrder(_config.GetValue<string>("AppSettingParams:RazorPay:SandboxID"), _config.GetValue<string>("AppSettingParams:RazorPay:SandboxKey"), relationModelObj);
                            relationModelObj.PID = _config.GetValue<string>("AppSettingParams:RazorPay:SandboxID");
                            relationModelObj = Donation.Save(relationModelObj);
                            if (relationModelObj != null)
                            {
                                if (relationModelObj.ID > 0)
                                {
                                    resp = new ajaxResponse()
                                    {
                                        data = relationModelObj,
                                        respmessage = "Donation saved",
                                        respstatus = ResponseStatus.success
                                    };
                                }
                                else
                                    resp = new ajaxResponse()
                                    {
                                        data = null,
                                        respmessage = "Something went wrong.",
                                        respstatus = ResponseStatus.error
                                    };
                            }
                            else
                                resp = new ajaxResponse()
                                {
                                    data = null,
                                    respmessage = "Something went wrong.",
                                    respstatus = ResponseStatus.error
                                };
                        }
                        else
                            resp = new ajaxResponse()
                            {
                                data = null,
                                respmessage = "Something went wrong.",
                                respstatus = ResponseStatus.error
                            };
                    }
                    else
                        resp = new ajaxResponse()
                        {
                            data = null,
                            respmessage = "Something went wrong.",
                            respstatus = ResponseStatus.error
                        };
                }
            }
            catch (Exception ex)
            {
                resp = new ajaxResponse()
                {
                    data = null,
                    respmessage = "Something went wrong.",
                    respstatus = ResponseStatus.error
                };

            }

            return Json(resp);
        }

        [HttpPost]
        public JsonResult SavePayment()
        {
            string razorpay_payment_id = "", razorpay_signature = "", code = "", description = "", source = "", step = "", reason = "", order_id = "";
            razorpay_payment_id = Request.Form["razorpay_payment_id"];
            razorpay_signature = Request.Form["razorpay_signature"];
            code = Request.Form["code"];
            description = Request.Form["description"];
            source = Request.Form["source"];
            step = Request.Form["step"];
            reason = Request.Form["reason"];
            order_id = Request.Form["razorpay_order_id"];

            var resp = new ajaxResponse();
            var isSave = false;
            if (!string.IsNullOrEmpty(razorpay_payment_id))
                isSave = Donation.SavePaymentOrder(new TBL_ORDERGENERATORMASTER()
                {
                    RAZORPAY_CODE = code,
                    RAZORPAY_DESCRIPTION = description,
                    RAZORPAY_PAYMENT_ID = razorpay_payment_id,
                    RAZORPAY_REASON = reason,
                    RAZORPAY_SIGNATURE = razorpay_signature,
                    RAZORPAY_SOURCE = source,
                    RAZORPAY_STEP = step,
                    ORDERID = order_id
                });
            resp = new ajaxResponse()
            {
                data = null,
                respmessage = (isSave ? "Payment received" : "Something went wrong"),
                respstatus = (isSave ? ResponseStatus.success : ResponseStatus.error),
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
